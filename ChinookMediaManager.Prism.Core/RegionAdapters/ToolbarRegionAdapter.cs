using System.Collections.Specialized;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

namespace ChinookMediaManager.Prism.Core.RegionAdapters
{
    public class ToolbarRegionAdapter : RegionAdapterBase<ToolBar>
    {
        public ToolbarRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, ToolBar regionTarget)
        {
            region.ActiveViews.CollectionChanged += (s, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var view in args.NewItems)
                            AddViewToRegion(view, regionTarget);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var view in args.OldItems)
                            RemoveViewFromRegion(view, regionTarget);
                        break;
                    default:
                        break;
                }
            };
        }

        private void RemoveViewFromRegion(object view, ToolBar regionTarget)
        {
            regionTarget.Items.Remove(view);
        }

        private void AddViewToRegion(object view, ToolBar regionTarget)
        {
                regionTarget.Items.Add(view);
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}