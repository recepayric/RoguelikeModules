using Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.ParticleWithDuration
{
    public class ParticleWithDurationMediator : Mediator
    {
        [Inject] public ParticleWithDurationView view { get; set; }
        [Inject] public EntitySignals EntitySignals { get; set; }


        private void OnStartParticle(GameObject particleObject, float duration)
        {
	        if (gameObject != particleObject) return;
	        
	        view.StartParticle(duration);
        }
        
		private void OnEndParticleEvent()
		{
			
		}

        public override void OnRegister()
        {
            base.OnRegister();
			view.EndParticleEvent+=OnEndParticleEvent;
			EntitySignals.StartParticleWithDuration.AddListener(OnStartParticle);
        }

        public override void OnRemove()
        {
            base.OnRemove();
			view.EndParticleEvent-=OnEndParticleEvent;
			EntitySignals.StartParticleWithDuration.RemoveListener(OnStartParticle);
        }
    }
}
