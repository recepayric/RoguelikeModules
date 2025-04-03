using DG.Tweening;
using MVC.Base.Runtime.Signals;
using Runtime.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Controller
{
    public class InitializeGameCommand : Command
    {
        [Inject] public GameInitializeSignals GameInitializeSignals { get; set; }
        [Inject] public ScreenSignals ScreenSignals { get; set; }
        
        public override void Execute()
        {
            DOTween.SetTweensCapacity(1000,500);
            QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 120;
            
            //MenuSignals.UpdateSettingsSignal.Dispatch(SettingsAction.Initialize);
            
            //Time.timeScale = 0.3f;
            //GameInitializeSignals.StartGameSignal.Dispatch();

          
        }
    }
}