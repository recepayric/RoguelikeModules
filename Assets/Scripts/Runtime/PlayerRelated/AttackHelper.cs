using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.PlayerRelated
{
    public class AttackHelper : MonoBehaviour
    {
        public WeaponType weaponType;
        public bool isAttacking = false;
        public float attackSpeed;

        public float attackAnimationTime = 0;
        public float drawDistanceMax = 1;
        public float drawDistance;
        public Projectile projectileToShoot;
        public GameObject projectilePoint;
        public GameObject targetEnemy;

        public void Reset()
        {

            if (isAttacking)
            {
                if (projectileToShoot != null)
                    projectileToShoot.isActive = true;
                //BasicPool.instance.Return(projectileToShoot.gameObject);
            }
            isAttacking = false;
        }
        
        private void Update()
        {
            if (!isAttacking) return;
            
            if(weaponType == WeaponType.Bow)
                UpdateBow();
        }

        public void StartAttack()
        {
            isAttacking = true;
        }

        private void UpdateBow()
        {
            drawDistance += drawDistanceMax*Time.deltaTime/attackAnimationTime;
            
            if (drawDistance > drawDistanceMax)
            {
                drawDistance = drawDistanceMax;
                projectileToShoot.isActive = true;
                isAttacking = false;
                return;
            }

            if (targetEnemy != null)
                projectileToShoot.transform.right = targetEnemy.transform.position - projectileToShoot.transform.position;
            projectileToShoot.transform.position = projectilePoint.transform.position - projectileToShoot.transform.right*drawDistance;
        }

        public void SetBowArrowAnimation(Projectile projectile, GameObject arrowPoint, GameObject target)
        {
            projectileToShoot = projectile;
            projectilePoint = arrowPoint;
            targetEnemy = target;
            drawDistance = 0;
            attackAnimationTime = attackSpeed * 0.8f;
        }
    }
}