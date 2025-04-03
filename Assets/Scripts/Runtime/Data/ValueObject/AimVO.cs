using System;
using Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class AimVO
    {
        public event UnityAction CheckForNewTargetEvent;
        public event UnityAction<GameObject> SetClosestTargetEvent;

        public GameObject targetObject;
        public EntityType targetType;
        public float aimRange;
        public bool isAiming;
        public bool isAimed;
        public bool isAttacking = false;
        public bool isAttackStopped = false;
        public bool canTurnToNewEnemy = false;

        public void CheckForNewTarget()
        {
            CheckForNewTargetEvent?.Invoke();
        }

        public void SetClosestTarget(GameObject gameObject)
        {
            SetClosestTargetEvent?.Invoke(gameObject);
        }
    }
}