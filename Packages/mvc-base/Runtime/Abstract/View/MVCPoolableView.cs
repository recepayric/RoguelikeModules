
using MVC.Base.Runtime.Abstract.Function;

namespace MVC.Base.Runtime.Abstract.View
{
    public class MVCPoolableView : MVCView, IPoolable
    {
        public virtual void OnGetFromPool()
        {
            
        }

        public virtual void OnReturnToPool()
        {
            RemoveRegistration();
        }

        public string PoolKey { get; set; }

        public override bool autoRegisterWithContext
        {
            get => false;
        }
    }
}