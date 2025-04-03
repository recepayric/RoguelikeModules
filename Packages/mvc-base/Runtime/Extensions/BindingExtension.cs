using strange.extensions.command.api;
using strange.extensions.injector.api;

namespace MVC.Base.Runtime.Extensions
{
    public static class BindingExtension
    {
        public static TAbstract BindCrossContextSingletonSafely<TAbstract, TConcrete>(this ICrossContextInjectionBinder injectionBinder)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TAbstract>();
            if (binding == null)
            {
                injectionBinder.Bind<TAbstract>().To<TConcrete>().ToSingleton().CrossContext();
            }
        
            TAbstract instance = injectionBinder.GetInstance<TAbstract>();
            return instance;
        }
        public static TDirect BindCrossContextSingletonSafely<TDirect>(this ICrossContextInjectionBinder injectionBinder)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TDirect>();
            if (binding == null)
            {
                injectionBinder.Bind<TDirect>().ToSingleton().CrossContext();
            }
        
            TDirect instance = injectionBinder.GetInstance<TDirect>();

            return instance;
        }
    
        public static TDirect BindCrossContextSingletonSafely<TDirect>(this ICrossContextInjectionBinder injectionBinder, object toObject)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TDirect>();
            if (binding == null)
            {
                injectionBinder.Bind<TDirect>().To(toObject).ToSingleton().CrossContext();
            }
        
            TDirect instance = injectionBinder.GetInstance<TDirect>();

            return instance;
        }

        public static TAbstract BindSingletonSafely<TAbstract, TConcrete>(this ICrossContextInjectionBinder injectionBinder)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TAbstract>();
            if (binding == null)
            {
                injectionBinder.Bind<TAbstract>().To<TConcrete>().ToSingleton();
            }
        
            TAbstract instance = injectionBinder.GetInstance<TAbstract>();
            return instance;
        }
        
        public static TDirect BindSingletonSafely<TDirect>(this ICrossContextInjectionBinder injectionBinder)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TDirect>();
            if (binding == null)
            {
                injectionBinder.Bind<TDirect>().ToSingleton();
            }
        
            TDirect instance = injectionBinder.GetInstance<TDirect>();

            return instance;
        }
    
        public static TDirect BindSingletonSafely<TDirect>(this ICrossContextInjectionBinder injectionBinder, object toObject)
        {
            IInjectionBinding binding = injectionBinder.GetBinding<TDirect>();
            if (binding == null)
            {
                injectionBinder.Bind<TDirect>().To(toObject).ToSingleton();
            }
        
            TDirect instance = injectionBinder.GetInstance<TDirect>();

            return instance;
        }

        public static void BindCommandSafely<TDirect>(this ICommandBinder commandBinder, object toObject)
        {
            ICommandBinding binding = commandBinder.GetBinding<TDirect>();
            if (binding == null)
            {
                commandBinder.Bind<TDirect>().To(toObject);
            }
        }
    }
}