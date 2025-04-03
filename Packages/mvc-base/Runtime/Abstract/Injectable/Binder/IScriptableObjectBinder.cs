using strange.framework.api;

namespace MVC.Base.Runtime.Abstract.Injectable.Binder
{
	public interface IScriptableObjectBinder : IBinder
    {
        void Inject(IInjectableScriptableObject injectable);
    }
}