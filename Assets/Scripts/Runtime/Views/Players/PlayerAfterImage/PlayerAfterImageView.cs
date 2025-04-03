using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.View;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Views.Players.PlayerAfterImage
{
    public class PlayerAfterImageView : MVCView, IPoolable
    {
        public event UnityAction GetFromPool;
		public event UnityAction ReturnToPool;
		public event UnityAction DestroyAfterimageEvent;
		public float removeTime = 0.1f;
		public float removeTimer = 0;
		public float maxCutoff;
		public float minCutoff;
		public List<Renderer> renderers;
		public MaterialPropertyBlock PropertyBlock;

		private void Update()
		{
			float percentage = removeTimer / removeTime;

			var currentCutoff = Mathf.Lerp(maxCutoff, minCutoff, 1-percentage);
			PropertyBlock.SetFloat("_Cutoff_Height", currentCutoff);
			
			for (int i = 0; i < renderers.Count; i++)
				renderers[i].SetPropertyBlock(PropertyBlock);
			
			
			removeTimer -= Time.deltaTime;
			if(removeTimer <= 0)
				DestroyAfterimageEvent?.Invoke();
		}
		
		[Button]
		public void GetTrails()
		{
			renderers.Clear();
			var childs = GetComponentsInChildren<Transform>();

			for (int i = 0; i < childs.Length; i++)
			{
				var trail = childs[i].GetComponent<Renderer>();
				if (trail != null)
				{
					renderers.Add(trail);
				}
			}
		}
		
        public void OnGetFromPool()
		{
			removeTimer = removeTime;
			PropertyBlock = new MaterialPropertyBlock();
			GetFromPool?.Invoke();
		}
		public void OnReturnToPool()
		{
			ReturnToPool?.Invoke();
		}

		public string PoolKey { get; set; }
    }
}
