using System;
using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Function;
using UnityEngine.Events;
using MVC.Base.Runtime.Abstract.View;
using UnityEngine;

namespace Runtime.Views.ParticleWithDuration
{
    public class ParticleWithDurationView : MVCView, IPoolable
    {
        public event UnityAction EndParticleEvent;

        public List<ParticleSystem> particleRenderers;

        private bool isEnded;
        private float particleTime;
        private float particleTimer;
        private float endTimer;


        private void Update()
        {
	        if (isEnded)
	        {
		        endTimer += Time.deltaTime;
		        
		        if(endTimer >= 1)
			        EndParticleEvent?.Invoke();
		        
		        return;
	        }
	        
	        particleTimer += Time.deltaTime;
	        if(particleTimer >= particleTime)
		        StopParticles();
        }
        
        private void StartParticles()
        {
	        isEnded = true;

	        for (int i = 0; i < particleRenderers.Count; i++)
	        {
		        var emission = particleRenderers[i].emission;
		        emission.enabled = true;
                
		        particleRenderers[i].Play();
	        }
        }
        
        private void StopParticles()
        {
	        for (int i = 0; i < particleRenderers.Count; i++)
	        {
		        var emission = particleRenderers[i].emission;
		        emission.enabled = false;
	        }
        }

        public void StartParticle(float duration)
        {
	        isEnded = false;
	        particleTime = duration;
	        particleTimer = 0;
	        StartParticles();
        }

        public void OnGetFromPool()
        {
	        
        }

        public void OnReturnToPool()
        {
        }

        public string PoolKey { get; set; }
    }
}
