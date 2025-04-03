using MVC.Base.Runtime.Concrete.Context;
using MVC.Base.Runtime.Extensions;
using Runtime.Signals;
using Runtime.Views.Minimap;
using Runtime.Views.Screens;

namespace Runtime.Context
{
    public class UIContext : BaseUIContext
    {
        private UISignals _uiSignals;
        
        protected override void mapBindings()
        {
            base.mapBindings();

            _uiSignals = injectionBinder.BindCrossContextSingletonSafely<UISignals>();
            
            mediationBinder.Bind<GamePlayScreensView>().To<GamePlayScreensMediator>();
            mediationBinder.Bind<MinimapView>().To<MinimapMediator>();
            
        }

        public override void Launch()
        {
            
        }
    }
}