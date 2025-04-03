using Runtime.Model;
using Runtime.Model.Interfaces;
using Runtime.Signals;
using strange.extensions.mediation.impl;

namespace Runtime.Views.PlayerFollowCamera
{
    public class PlayerFollowCameraMediator : Mediator
    {
        [Inject] public PlayerFollowCameraView view { get; set; }
        [Inject] public EntitySignals EntitySignals { get; set; }
        [Inject] public IPlayerModel PlayerModel { get; set; }
        
        public void OnInitializePlayerCamera()
        {
            var player = PlayerModel.ActivePlayer;
            view.playerObject = player;
            //view.StartFollowingPlayer();
        }

        public override void OnRegister()
        {
            base.OnRegister();
            EntitySignals.InitializePlayerCameraSignal.AddListener(OnInitializePlayerCamera);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            EntitySignals.InitializePlayerCameraSignal.RemoveListener(OnInitializePlayerCamera);
        }
    }
}
