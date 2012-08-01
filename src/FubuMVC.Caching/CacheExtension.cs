using FubuMVC.Core;

namespace FubuMVC.Caching
{
    public class CacheExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Add<CachingConfiguration>();
            registry.Services(services =>
            {
                services.SetServiceIfNone<ICache, DefaultCache>();
            });
        }
    }
}
