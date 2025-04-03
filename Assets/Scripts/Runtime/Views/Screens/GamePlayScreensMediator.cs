using Runtime.Data.UnityObject;
using Runtime.Enums;
using Runtime.Model.Interfaces;
using Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.Screens
{
    public class GamePlayScreensMediator : Mediator
    {
        [Inject] public GamePlayScreensView view { get; set; }
        [Inject] public UISignals UISignals { get; set; }
        [Inject] public IPlayerModel PlayerModel { get; set; }




        private void GetCurrentExperience()
        {
            var expVo = PlayerModel.PlayerVos[PlayerModel.ActivePlayer].playerExperienceVo;

            var startLevel = expVo.currentLevel;
            var experience = expVo.currentExperience;
            var requiredExperience = expVo.requiredExperience;
            var levelPercentage = 0f;
            
            if (expVo.requiredExperience == 0)
                levelPercentage = 0;
            else
                levelPercentage = experience / requiredExperience;
            
            
            view.SetStartLevel(startLevel);
            view.SetExperienceBar(startLevel, levelPercentage);
        }
     


        private void OnHealthUpdate(float currentHealth, float maxHealth)
        {
            //Debug.LogError("Health updated!");
            view.UpdateHealth(currentHealth, maxHealth);
        }
		

        private void OnResetSkills()
        {
            view.ResetSkills();
        }
        
        public override void OnRegister()
        {
            base.OnRegister();
            Debug.Log("Registering gameplay screen");
            UISignals.UpdateHealthUISignal.AddListener(OnHealthUpdate);
            
            OnResetSkills();
            GetCurrentExperience();
        }

       
        public override void OnRemove()
        {
            base.OnRemove();
            UISignals.UpdateHealthUISignal.RemoveListener(OnHealthUpdate);
        }
    }
}
