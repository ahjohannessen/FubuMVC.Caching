using System;
using System.Threading;
using FubuMVC.Core.Caching;
using FubuMVC.Core.Resources.Etags;

namespace FubuMVC.Caching
{
    public interface IContentCache 
    {
        IRecordedOutput Retrieve(string key, Func<IRecordedOutput> onMissing);
        void Register(string key, string etag);
    }

    public class SimpleContentCache : IContentCache
    {
        private readonly IEtagCache _etagCache;
        private readonly IOutputCache _outputCache;

        private readonly ReaderWriterLock _lock = new ReaderWriterLock();

        public SimpleContentCache(IEtagCache etagCache, IOutputCache outputCache)
        {
            _etagCache = etagCache;
            _outputCache = outputCache;
        }

        public IRecordedOutput Retrieve(string key, Func<IRecordedOutput> onMissing)
        {
            return read(() => _outputCache.Retrieve(key, onMissing));
        }

        public void Register(string key, string etag)
        {
            write = () => _etagCache.Register(key, etag);
        }

        private T read<T>(Func<T> findValue)
        {
            try
            {
                _lock.AcquireReaderLock(1000);
                return findValue();
            }
            finally
            {
                _lock.ReleaseReaderLock();
            }
        }

        private Action write
        {
            set
            {
                try
                {
                    _lock.AcquireWriterLock(1000);
                    value();
                }
                finally
                {
                    _lock.ReleaseWriterLock();
                }
            }
        }
    }
}