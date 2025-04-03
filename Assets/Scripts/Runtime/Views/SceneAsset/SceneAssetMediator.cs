using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Signals;
using Runtime.Constants;
using Runtime.Model;
using Runtime.Model.Interfaces;
using Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.SceneAsset
{
    public class SceneAssetMediator : Mediator
    {
        [Inject] public SceneAssetView view { get; set; }
        [Inject] public IGameModel GameModel { get; set; }
        
        
        public override void OnRegister()
        {
            base.OnRegister();
            GameModel.SceneAsset.MainCamera = view.MainCamera;
            GameModel.SceneAsset.GameRoot = view.GameRoot.transform;
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }
    }
}
