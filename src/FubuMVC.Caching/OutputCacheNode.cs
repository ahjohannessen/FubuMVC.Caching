using FubuCore;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;

namespace FubuMVC.Caching
{
    public class OutputCacheNode : BehaviorNode
    {
        public override BehaviorCategory Category
        {
            get { return BehaviorCategory.Cache; }
        }

        public ObjectDef ContentCache { get; set; }
        public ObjectDef ResourceHash { get; set; }

        protected override ObjectDef buildObjectDef()
        {
            var def = ObjectDef.ForType<OutputCachingBehavior>();

            ContentCache.CallIfNotNull(cc => def.Dependency(typeof (IContentCache), cc));
            ResourceHash.CallIfNotNull(rg => def.Dependency(typeof(IResourceHash), rg));
            
            return def;
        }
    }
}