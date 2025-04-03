using System;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject.Modifier;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class PlayerVO : EntityVO
    {
        public event UnityAction PlayerStartedToMoveEvent;
        public event UnityAction PlayerStoppedMovingEvent;
        public event UnityAction PlayerStartedToAimEvent;
        public event UnityAction PlayerStoppedToAimEvent;
        public event UnityAction<GameObject> StartAttackEvent;
        public event UnityAction StopAttackEvent;
        public event UnityAction<Vector3> SetMoveEvent;


        public PlayerModifiers playerModifiers;
        public PlayerExperienceVO playerExperienceVo;
        public GameObject activeWeapon;
        //public bool isAttacking = false;
        //public bool isAttackStopped = false;
        public bool isAiming = false;
        public bool isMoving = false;
        public bool isDead = false;

        public PlayerVO()
        {
        }

        public void Initialize()
        {
        }

        public void SetMove(Vector3 pos)
        {
            SetMoveEvent?.Invoke(pos);
        }

        public void StopAttack()
        {
            StopAttackEvent?.Invoke();
        }
        
        public void StartAttack(GameObject targetObject)
        {
            StartAttackEvent?.Invoke(targetObject);
        }

        public void PlayerStartedToMove()
        {
            PlayerStartedToMoveEvent?.Invoke();
        }
        
        public void PlayerStoppedMoving()
        {
            PlayerStoppedMovingEvent?.Invoke();
        }
        
        public void PlayerStartedToAim()
        {
            PlayerStartedToAimEvent?.Invoke();
        }
        
        public void PlayerStoppedToAim()
        {
            PlayerStoppedToAimEvent?.Invoke();
        }
    }
}