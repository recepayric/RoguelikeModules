using System;
using DG.Tweening;
using Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.PlayerRelated
{
    public class PlayerTargetFollower : MonoBehaviour
    {
        public GameObject TargetPoint;
        public GameObject Effector;
        public PlayerMovement PlayerMovement;
        public GameObject BoneToRotate;

        public Vector3 effectorInitialPosition;

        public bool isTargeting;
        public GameObject targetEnemy;

        public float angle_rad;
        public float angle_deg;
        public float angle_try;

        public float y1, y2;
        public float x1, x2;

        private void Start()
        {
            effectorInitialPosition = Effector.transform.localPosition;
            SetEvents();
        }

        public float liftAmount;
        public float liftTime;
        public float wavePercentage;
        public float waveTime;
        public float resetTime;

        [Button]
        public void LiftUpWeapon()
        {
            DOTween.Kill("WeaponLift");
            Effector.transform.DOLocalMoveY(liftAmount, liftTime).SetId("WeaponLift").OnComplete(() =>
            {
                Effector.transform.DOLocalMoveY(liftAmount * wavePercentage, waveTime)
                    .SetId("WeaponLift")
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            });
        }


        [Button]
        public void ResetWeaponPos()
        {
            DOTween.Kill("WeaponLift");
            Effector.transform.DOLocalMoveY(effectorInitialPosition.y, resetTime).SetId("WeaponLift");
        }

        private void Update()
        {
            if (!isTargeting || targetEnemy == null || !targetEnemy.activeSelf)
                return;


            y1 = transform.position.y;
            x1 = transform.position.x;

            y2 = targetEnemy.transform.position.y;
            x2 = targetEnemy.transform.position.x;

            angle_try = Vector2.Angle(targetEnemy.transform.position - BoneToRotate.transform.position, Vector2.right);

            if (x1 < x2)
            {
                FaceTowards(1);
                angle_rad = Mathf.Atan2(y1 - y2, x2 - x1);
            }
            else
            {
                FaceTowards(-1);
                angle_rad = Mathf.Atan2(y1 - y2, x1 - x2);
            }

            angle_deg = Mathf.Rad2Deg * angle_rad + 90;

            //BoneToRotate.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle_deg));
            TargetPoint.transform.position = targetEnemy.transform.position;
        }

        private void FaceTowards(int side)
        {
            //-1 left, stay original.
            //1 right, turn.

            var scaleX = transform.localScale.x;
            var scaleY = transform.localScale.y;
            var scaleZ = transform.localScale.z;

            if (side == -1)
                scaleX = scaleX < 0 ? -scaleX : scaleX;
            else
                scaleX = scaleX > 0 ? -scaleX : scaleX;

            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }

        public void SetTargeting(bool isTargeting)
        {
            this.isTargeting = isTargeting;
            PlayerMovement.isTargetingEnemy = isTargeting;
        }

        public void SetTarget(GameObject targetEnemy)
        {
            this.targetEnemy = targetEnemy;
        }

        public void OnLiftWand(float timeToLift)
        {
            liftTime = timeToLift;
            LiftUpWeapon();
        }

        public void OnDownWand()
        {
            ResetWeaponPos();
        }

        private void SetEvents()
        {
            EventManager.Instance.LiftWandEvent += OnLiftWand;
            EventManager.Instance.DownWandEvent += OnDownWand;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.LiftWandEvent -= OnLiftWand;
            EventManager.Instance.DownWandEvent -= OnDownWand;
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }
    }
}