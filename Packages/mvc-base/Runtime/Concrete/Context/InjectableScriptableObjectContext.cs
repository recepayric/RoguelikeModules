using MVC.Base.Runtime.Abstract.Injectable;
using MVC.Base.Runtime.Abstract.Injectable.Binder;
using MVC.Base.Runtime.Concrete.Injectable.Binder;
using strange.framework.api;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.injector.api;

namespace MVC.Base.Runtime.Concrete.Context
{
    public class InjectableScriptableObjectContext : CrossContext
    {
        public InjectableScriptableObjectContext() : base()
        {
            if (firstContext == null || firstContext.GetContextView() == null)
            {
                firstContext = this;
            }
            else
            {
                firstContext.AddContext(this);
            }
            SetContextView(null);
            addCoreComponents();
            this.autoStartup = true;
            Start();
        }
        
        [Inject]
        public IScriptableObjectBinder scriptableObjectBinder { get; set; }

        public void Inject(IInjectableScriptableObject injectableScriptableObject)
        {
            scriptableObjectBinder.Inject(injectableScriptableObject);
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
	        
            injectionBinder.Bind<IInstanceProvider>().Bind<IInjectionBinder>().ToValue(injectionBinder);
            injectionBinder.Bind<IContext>().ToValue(this).ToName(ContextKeys.CONTEXT);
            injectionBinder.Bind<IScriptableObjectBinder>().To<ScriptableObjectBinder>().CrossContext().ToSingleton();
        }
        protected override void instantiateCoreComponents()
        {
            base.instantiateCoreComponents();
            scriptableObjectBinder = injectionBinder.GetInstance<IScriptableObjectBinder>();
        }
    }
}