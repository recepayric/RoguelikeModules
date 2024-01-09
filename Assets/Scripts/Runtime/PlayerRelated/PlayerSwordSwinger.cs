using System;
using Runtime.ProjectileRelated;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.IK;

namespace Runtime.PlayerRelated
{
    public class PlayerSwordSwinger : MonoBehaviour
    {
        public GameObject effector;
        public GameObject targetEnemy;
        public GameObject BoneToRotate;
        public GameObject BoneToRotate2;
        public GameObject slashPosition;
        public GameObject slashParentHolder;
        public GameObject slashParent;
        public Vector3 slashPositionVector;
        public float swingAmount;
        public float angleOffset;
        public float targetAngleOffset;
        public float targetAngleOffsetDifference;
        public float bone2Angle;
        public float bone2AngleMultiplier;

        public float animationTime;
        public float attackTime;
        public float swingReadyTimeBase;
        public float swingReadyTime;
        public float swingTimeBase;
        public float swingTime;
        public float swingToNormalTimeBase;
        public float swingToNormalTime;

        public int queuedSwingAmount = 0;
        public bool isArmTargeting;
        public bool isSwinging;
        public bool isFirstSwingUp;
        public bool isSwingingUp;
        public bool isSwingingDown;
        public bool isSwingToNormal;

        public SwingType lastSwingType;

        public float x1, x2;
        public float y1, y2;
        public float angle_rad;
        public float angle_deg;

        public Slash slash;
        public Weapon weapon;
        public IKManager2D ikManager2D;

        public void ActivateEnemyFollow()
        {
            ikManager2D.weight = 0;
            isArmTargeting = true;
        }

        public void DeActivateEnemyFollow()
        {
            ikManager2D.weight = 1;
            isArmTargeting = false;
        }

        public void SetTarget(GameObject target)
        {
            targetEnemy = target;
        }

        public void SetWeapon(Weapon pWeapon)
        {
            weapon = pWeapon;
        }

        public void SetSlashPosition(GameObject pSlashPosition)
        {
            slashPosition = pSlashPosition;
        }

        private void FixedUpdate()
        {
            
            if (!isArmTargeting) return;

            UpdateAngle();

            if (targetEnemy == null)
                return;

            y1 = transform.position.y;
            x1 = transform.position.x;

            y2 = targetEnemy.transform.position.y;
            x2 = targetEnemy.transform.position.x;

            if (x1 < x2)
            {
                angle_rad = Mathf.Atan2(y1 - y2, x2 - x1);
                slashParentHolder.transform.right = -(targetEnemy.transform.position - slashParentHolder.transform.position);
            }
            else
            {
                angle_rad = Mathf.Atan2(y1 - y2, x1 - x2);
                slashParentHolder.transform.right = targetEnemy.transform.position - slashParentHolder.transform.position;
            }

            angle_deg = Mathf.Rad2Deg * angle_rad + 90;

            BoneToRotate.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle_deg + angleOffset));
            BoneToRotate2.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, bone2Angle));
        }

        //target -10, c 0
        //difference = -10
        //
        public float swingUpTime = 0;

        private void UpdateAngle()
        {
            if (isSwingingUp)
            {
                swingUpTime = 1;
                swingUpTime = isFirstSwingUp ? swingReadyTime : swingUpTime;
                swingUpTime = isSwingToNormal ? swingToNormalTime : swingUpTime;

                angleOffset += targetAngleOffsetDifference * (Time.deltaTime / swingUpTime);

                bone2Angle = angleOffset / bone2AngleMultiplier;
                if (bone2Angle > 0)
                    bone2Angle = 0;

                if (angleOffset <= targetAngleOffset)
                {
                    angleOffset = targetAngleOffset;
                    bone2Angle = angleOffset / bone2AngleMultiplier;
                    if (isFirstSwingUp)
                        isFirstSwingUp = false;
                    DecideNextMove();
                }
            }

            if (isSwingingDown)
            {
                bone2Angle = angleOffset / bone2AngleMultiplier;
                if (bone2Angle > 0)
                    bone2Angle = 0;
                angleOffset += targetAngleOffsetDifference * (Time.deltaTime / swingTime);
                if (angleOffset >= targetAngleOffset)
                {
                    angleOffset = targetAngleOffset;
                    DecideNextMove();
                }
            }
        }

        private void CheckForOtherSwing()
        {
            if (queuedSwingAmount == 0) return;

            queuedSwingAmount--;
            Swing(attackTime);
        }

        public void DecideNextMove()
        {
            if (isSwingToNormal)
            {
                isSwinging = false;
                isSwingToNormal = false;
                isSwingingUp = false;
                CheckForOtherSwing();
            }
            else if (isSwingingUp)
            {
                isSwingingUp = false;
                SwingDown();
                CreateSlash();
            }
            else if (isSwingingDown)
            {
                isSwingingDown = false;
                SwingToNormal();
            }
        }

        private void CreateSlash()
        {
            //Todo change to pool!!!
            var slashObject = Instantiate(slash.gameObject);
            //slashObject.transform.SetParent(slashParent.transform);
            var slashSc = slashObject.GetComponent<Slash>();
            slashSc.StartCreating(swingTime);
            slashSc.SetPosition(slashPositionVector);
            slashSc.SetTarget(targetEnemy);
            slashSc.objectToFollow = slashParent;
            weapon.SetSlash(slashSc);

            /*if (x2 >= x1)
            {
                Vector3 v = Vector3.one;
                v.x = -1;
                slashSc.transform.localScale = v;
                slashSc.slashDir = -1;
            }*/
        }

        [Button]
        public void CancelSwing()
        {
            queuedSwingAmount = 0;
        }

        [Button]
        public void SwingToNormal()
        {
            targetAngleOffset = 0;
            targetAngleOffsetDifference = targetAngleOffset - angleOffset;
            isSwingToNormal = true;
            isSwingingUp = true;
        }

        [Button]
        public void Swing(float pAttackTime)
        {
            attackTime = pAttackTime;
            if (isSwinging)
            {
                queuedSwingAmount++;
                return;
            }

            var dist = Vector3.Distance(BoneToRotate.transform.position, slashPosition.transform.position);
            dist /= BoneToRotate.transform.lossyScale.x;
            if (dist < 0)
                dist *= -1f;
            slashParent.transform.localPosition = new Vector3(dist, 0, 0);

            slashPositionVector = slashPosition.transform.position;
            isFirstSwingUp = true;
            isSwinging = true;
            PrepareTimes();
            SwingUp();
        }

        private void PrepareTimes()
        {
            var timeMult = attackTime * animationTime;
            swingReadyTime = swingReadyTimeBase * timeMult;
            swingTime = swingTimeBase * timeMult;
            swingToNormalTime = swingToNormalTimeBase * timeMult;
        }

        [Button]
        public void SwingUp()
        {
            targetAngleOffset = -swingAmount * (1f / 3f);
            targetAngleOffsetDifference = targetAngleOffset - angleOffset;
            isSwingingUp = true;
            lastSwingType = SwingType.SwingUp;
        }

        [Button]
        public void SwingDown()
        {
            targetAngleOffset = swingAmount * (2f / 3f);
            targetAngleOffsetDifference = targetAngleOffset - angleOffset;
            isSwingingDown = true;
            lastSwingType = SwingType.SwingDown;
        }
    }

    public enum SwingType
    {
        SwingUp,
        SwingDown
    }
}