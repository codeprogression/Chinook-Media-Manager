using Microsoft.Practices.Prism.Commands;
using NHibernate;

namespace ChinookMediaManager.Core.DynamicViewModel
{
    public abstract class ViewModelProxy<VIEWMODEL, MODEL> : ModelProxy<VIEWMODEL, MODEL> where MODEL : class, new()
    {
        protected ViewModelProxy(MODEL wrappedEntity)
        {
            Entity = wrappedEntity;
            RefreshCommand = new DelegateCommand<object>(Refresh, CanRefresh);
        }

        protected virtual void Load() { }
        public DelegateCommand<object> RefreshCommand { get; private set; }
        protected virtual void Refresh(object obj) { Load(); }
        protected virtual bool CanRefresh(object obj) { return true; }

        public int GetId()
        {
            return ((dynamic)this).Id;
        }
        protected override void RaiseCanExecuteEvents()
        {
            RefreshCommand.RaiseCanExecuteChanged();
        }

        public virtual bool CanDelete()
        {
            return true;
        }
        public virtual void Delete(ISession session)
        {
            session.Delete(Entity);
        }

        public virtual void Update(ISession session)
        {
            session.SaveOrUpdate(Entity);
        }
    }
}