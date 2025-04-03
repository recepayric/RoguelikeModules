using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Context;
using MVC.Base.Runtime.Concrete.Model;
using MVC.Base.Runtime.Extensions;
using Runtime.Controller;
using Runtime.Model;
using Runtime.Model.Interfaces;
using Runtime.Signals;
using Runtime.Views.PlayerFollowCamera;
using Runtime.Views.PlayerMovement;
using Runtime.Views.Players.Player;
using Runtime.Views.Players.PlayerInput;
using Runtime.Views.SceneAsset;
using Runtime.Views.Global.Health;
using Runtime.Views.MinimapObject;


namespace Runtime.Context
{
    public class GameContext :  MVCContext
    {
        private GameInitializeSignals _gameInitializeSignals;
        private EntitySignals _entitySignals;
        private UISignals _uiSignals;

        
        private PlayerContexts _playerContexts;
        
        protected override void mapBindings()
        {
            base.mapBindings();
            _playerContexts = injectionBinder.BindCrossContextSingletonSafely<PlayerContexts>();
            _gameInitializeSignals = injectionBinder.BindCrossContextSingletonSafely<GameInitializeSignals>();
            _entitySignals = injectionBinder.BindCrossContextSingletonSafely<EntitySignals>();
            _uiSignals = injectionBinder.BindCrossContextSingletonSafely<UISignals>();

            injectionBinder.BindCrossContextSingletonSafely<IGameModel, GameModel>();
            injectionBinder.BindCrossContextSingletonSafely<IObjectPoolModel, ObjectPoolModel>();
            injectionBinder.BindCrossContextSingletonSafely<IStatModel, StatModel>();
            injectionBinder.BindCrossContextSingletonSafely<IAimModel, AimModel>();
            injectionBinder.BindCrossContextSingletonSafely<ILoaderModel, LoaderModel>();
            injectionBinder.BindCrossContextSingletonSafely<IHealthModel, HealthModel>();
            injectionBinder.BindCrossContextSingletonSafely<ISoundModel, SoundModel>();
            injectionBinder.BindCrossContextSingletonSafely<IMinimapModel, MinimapModel>();

            _playerContexts.BindModels(this);
            mediationBinder.Bind<SceneAssetView>().To<SceneAssetMediator>();
            mediationBinder.Bind<PlayerView>().To<PlayerMediator>();
            mediationBinder.Bind<PlayerInputView>().To<PlayerInputMediator>();
            mediationBinder.Bind<PlayerMovementView>().To<PlayerMovementMediator>();
            mediationBinder.Bind<PlayerFollowCameraView>().To<PlayerFollowCameraMediator>();
            mediationBinder.Bind<HealthView>().To<HealthMediator>();
            mediationBinder.Bind<MinimapObjectView>().To<MinimapObjectMediator>();
 

            _playerContexts.BindViews(this);
            
            commandBinder.Bind(_gameInitializeSignals.InitializeGameSignal).InSequence().To<InitPoolCommand>().To<InitializeGameCommand>();
            commandBinder.Bind(_entitySignals.CalculateStatsSignal).To<CalculateStatsCommand>();
            
        }

        public override void Launch()
        {
            _gameInitializeSignals.InitializeGameSignal.Dispatch();
        }
    }
}
