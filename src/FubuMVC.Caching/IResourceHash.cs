using FubuMVC.Core.Http;

namespace FubuMVC.Caching
{
    public interface IResourceHash
    {
        string Generate();
    }

    public class SimpleResourceHash : IResourceHash
    {
        private readonly ICurrentChain _currentChain;
        public SimpleResourceHash(ICurrentChain currentChain)
        {
            _currentChain = currentChain;
        }

        public string Generate()
        {
            return _currentChain.ResourceHash();
        }
    }
}