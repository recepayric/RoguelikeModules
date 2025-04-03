using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.View;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Views.Players.Player
{
    public class PlayerView : MVCView, IPoolable
    {
        public event UnityAction SpawnedEvent;
        public event UnityAction StartAttackEvent;
        public event UnityAction StopAttackEvent;
        public event UnityAction OnGetFromPoolEvent;
		public event UnityAction OnReturnToPoolEvent;
		public event UnityAction CheckForCollectablesEvent;
		public event UnityAction<GameObject, PoolKey, Vector3> DealDamageEvent;

		//Required Vo's
		public RD_Player playerData;
		public PlayerVO playerVo;
		public EntityStatsVO stats;
		public AimVO aimVo;
		
		public float timerCollectables;
		public float timeToCheckCollectables;

		public List<Collider> attackColliders;
		public List<Collider> attackCollidersUlti;
		public List<GameObject> attackedEnemies;

		public bool isAttackStarted;

		public void OnAttackStarted()
		{
			isAttackStarted = true;
		}

		public void OnAttackStopped()
		{
			isAttackStarted = false;
			
			DeActivateTriggers();
			attackedEnemies.Clear();
		}

		public void ActivateTriggers()
		{
			if (!isAttackStarted) return;
			
			attackedEnemies.Clear();

			for (int i = 0; i < attackColliders.Count; i++)
			{
				attackColliders[i].enabled = true;
			}
		}
		
		public void ActivateTriggersUlti()
		{
			if (!isAttackStarted) return;

			attackedEnemies.Clear();

			for (int i = 0; i < attackCollidersUlti.Count; i++)
			{
				attackCollidersUlti[i].enabled = true;
			}
		}
		
		public void DeActivateTriggers()
		{
			for (int i = 0; i < attackColliders.Count; i++)
			{
				attackColliders[i].enabled = false;
			}
			
			for (int i = 0; i < attackCollidersUlti.Count; i++)
			{
				attackCollidersUlti[i].enabled = false;
			}
		}

		public void EnemyCollided(GameObject enemyHit, PoolKey hitKey, Vector3 hitPoint)
		{
			if (!isAttackStarted) return;
			DealDamageEvent?.Invoke(enemyHit, hitKey, hitPoint);
		}

		private void FixedUpdate()
		{
		
		}
		

		private void Update()
		{
			timerCollectables += Time.deltaTime;
			if (timerCollectables >= timeToCheckCollectables)
			{
				timerCollectables -= timeToCheckCollectables;
				CheckForCollectable();
			}
		}
		
		private async UniTask Move(GameObject gameObject, Vector3 pos)
		{
			int timerLeft = 30;
			while (timerLeft >= 0)
			{
				gameObject.transform.position = pos;
				await UniTask.WaitForFixedUpdate();
				timerLeft--;
			}
		}

		public void PlayerDies()
		{
			
		}

		private void CheckForCollectable()
		{
			CheckForCollectablesEvent?.Invoke();
		}

		private void UpdateWeaponAttackTimer()
		{
			StartAttackEvent?.Invoke();
	    }
		
		public void OnGetFromPool()
		{
			OnGetFromPoolEvent?.Invoke();
		}

		public void OnReturnToPool()
		{
			OnReturnToPoolEvent?.Invoke();
		}

		public string PoolKey { get; set; }
    }
}
