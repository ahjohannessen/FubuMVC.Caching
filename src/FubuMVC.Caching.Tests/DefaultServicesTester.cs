using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Caching.Tests
{
    [TestFixture]
    public class DefaultServicesTester : InteractionContext<CacheExtension>
    {
        private ServiceGraph _services;
        protected override void beforeEach()
        {
            var registry = new FubuRegistry();
            ClassUnderTest.Configure(registry);
            _services = registry.BuildLightGraph().Services;
        }

        [Test]
        public void smoke()
        {
            Assert.Pass();
        }
    }
}
