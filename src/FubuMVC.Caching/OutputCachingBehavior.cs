using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Caching;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Caching
{
    public class OutputCachingBehavior : IActionBehavior
    {
        private readonly IActionBehavior _inner;
        private readonly IContentCache _cache;
        private readonly IOutputWriter _writer;
        private readonly IResourceHash _resourceHash;

        public OutputCachingBehavior(IActionBehavior inner, IContentCache cache, IOutputWriter writer, IResourceHash resourceHash)
        {
            _inner = inner;
            _cache = cache;
            _writer = writer;
            _resourceHash = resourceHash;

            Invoker = x => x.Invoke();
            PartialInvoker = x => x.InvokePartial();
        }

        public Action<IActionBehavior> Invoker { get; private set; } 
        public Action<IActionBehavior> PartialInvoker { get; private set; } 

        public void Invoke()
        {
            generateOutput(Invoker);
        }

        public void InvokePartial()
        {
            generateOutput(PartialInvoker);
        }

        public virtual IRecordedOutput CreateOutput(string resourceHash, Action<IActionBehavior> innerInvocation)
        {
            var newOutput = _writer.Record(() => innerInvocation(_inner)); 
            newOutput.ForHeader(HttpResponseHeaders.ETag, etag => _cache.Register(resourceHash, etag));
            return newOutput;
        }

        // consider culture

        private void generateOutput(Action<IActionBehavior> innerInvocation)
        {
            var key = _resourceHash.Generate();
            var output = _cache.Retrieve(key, () => CreateOutput(key, innerInvocation));
            _writer.Replay(output);
        }
    }
}