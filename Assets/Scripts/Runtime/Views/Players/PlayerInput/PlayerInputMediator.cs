using Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.Players.PlayerInput
{
    public class PlayerInputMediator : Mediator
    {
        [Inject] public PlayerInputView view { get; set; }
        [Inject] public EntitySignals EntitySignals { get; set; }


        private void TryToCastSkill(int skillIndex)
        {
        }

        private void GetPlayerSkills(GameObject playerObject)
        {
        }
        
        
        private void OnCastSpellEvent(Vector3 skillCastPosition, int index)
        {
            
        }
        
        private void OnUsePotion()
        {
            EntitySignals.UseHealthPotionSignal.Dispatch(gameObject);
        }

        private void OnPlayerDashStarted(GameObject gameObject)
        {
            view.PlayerDashed();
        }

        public override void OnRegister()
        {
            base.OnRegister();
            view.UsePotionEvent += OnUsePotion;
            view.CastSpellEvent += OnCastSpellEvent;
            view.TryToCastSpellEvent += TryToCastSkill;
            EntitySignals.PlayerDashedSignal.AddListener(OnPlayerDashStarted);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            view.UsePotionEvent -= OnUsePotion;
            view.CastSpellEvent -= OnCastSpellEvent;
            view.TryToCastSpellEvent -= TryToCastSkill;
            EntitySignals.PlayerDashedSignal.RemoveListener(OnPlayerDashStarted);
        }
    }
}
