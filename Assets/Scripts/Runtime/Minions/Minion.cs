using System.Collections.Generic;
using DG.Tweening;
using Runtime.Enums;
using Runtime.PlayerRelated;
using UnityEngine;

namespace Runtime.Minions
{
    public class Minion : MonoBehaviour, IPoolObject
    {
        public PlayerSwordSwinger SwordSwinger;
        public AttackHelper AttackHelper;
        public Transform playerTransform;
        public Transform weaponParent;
        public Vector3 positionRelatedToPlayer;
        public Vector3 targetPosition;

        public GameObject targetEnemy;
        public GameObject closestEnemy;
        public float closestEnemyDistance;
        public bool isMelee = true;
        public bool isCloseToTarget = false;
        public float rangeToAttack;
        public float moveDistanceThreshold;
        public float walkSpeed;
        public float walkSpeedToFollow;
        public float walkSpeedToEnemy;

        public Weapon weapon;

        public List<GameObject> enemiesInRadius;
        public Animator animator;

        public float attackSpeed = 1;

        public bool isHitAnimationEnded = false;
        
        void Update()
        {
            CheckForTarget();
            CheckEnemies();
            CheckDirection();
            CheckForDistance();

            if (!isCloseToTarget)
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * walkSpeed);
        }
        
        private void CheckEnemies()
        {
            GameObject closestEnemy = null;
            float closestDistance = 10000;

            foreach (var enemies in DictionaryHolder.Enemies)
            {
                if (!enemies.Value.IsAvailable())
                    continue;
                
                var distance = Vector3.Distance(transform.position, enemies.Key.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemies.Key;
                }
            }

            if (closestEnemy != null)
                this.closestEnemy = closestEnemy;
            
            SetWeaponEnemy(closestEnemy, closestDistance);
            
        }

        public void SetWeaponEnemy(GameObject enemy, float distance)
        {
            if (weapon != null)
                weapon.SetEnemy(enemy, distance);
        }

        public void EquipWeapon(Weapon weaponToEquip)
        {
            weapon = weaponToEquip;
            weapon.transform.SetParent(weaponParent);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
            weapon.SetSwordSwinger(SwordSwinger);
            weapon.AttackHelper = AttackHelper;
        }

        public void SetDefaultPosition(Vector3 positionRelatedToPlayer, Transform playerTransform)
        {
            this.positionRelatedToPlayer = positionRelatedToPlayer;
            this.playerTransform = playerTransform;
        }

        public void SetEnemyList(List<GameObject> enemies)
        {
            enemiesInRadius = enemies;
        }

        private void CheckForTarget()
        {
            targetPosition = playerTransform.position + positionRelatedToPlayer;
            return;
            
        }

        private Vector3 GetEnemyPosition()
        {
            var targetPos = targetEnemy.transform.position;

            return targetPos;
        }

        public void ActivateWeapon()
        {
        }

        public void DeactivateWeapon()
        {
        }

        private void StartAttackLoop()
        {
            if (DOTween.IsTweening(gameObject.GetInstanceID() + "AttackLoop")) return;
            
            if (targetEnemy == null || !isCloseToTarget)
            {
                StopAttackLoop();
            }


            Debug.Log("Starting Attack Loop!");

            float timeToAttack = attackSpeed;
            animator.SetTrigger("StartAttack");

            DOVirtual.DelayedCall(timeToAttack, () =>
            {
                if (targetEnemy == null || !isCloseToTarget)
                {
                    StopAttackLoop();
                }
                else
                {
                    animator.SetTrigger("StartAttack");
                }
                
            }).SetId(gameObject.GetInstanceID() + "AttackLoop").SetLoops(-1);
        }

        private void StopAttackLoop()
        {
            Debug.Log("Stopping Attack Loop!");
            DOTween.Kill(gameObject.GetInstanceID() + "AttackLoop");
        }

        private void CheckForDistance()
        {
            var distance = Vector3.Distance(transform.position, targetPosition);

            if (targetEnemy != null && distance < rangeToAttack)
            {
                isCloseToTarget = true;
                animator.SetBool("IsRunning", false);
                //animator.SetBool("IsAttacking", true);
                StartAttackLoop();
            }
            else if (distance < moveDistanceThreshold)
            {
                isCloseToTarget = true;
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsAttacking", false);
            }
            else
            {
                isCloseToTarget = false;
                animator.SetBool("IsRunning", true);
            }
        }
        
        private void CheckDirection()
        {
            var isLookingLeft = transform.localScale.x > 0;
            var isTargetOnLeft = (transform.position.x - targetPosition.x) >= 0;

            //Debug.Log(isLookingLeft + " " + isTargetOnLeft);
            if (isTargetOnLeft != isLookingLeft)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

        private void ResetMinion()
        {
            BasicPool.instance.Return(weapon.gameObject);
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            ResetMinion();
            DictionaryHolder.Minions.Remove(gameObject);

        }

        public void OnGet()
        {
            DictionaryHolder.Minions.Add(gameObject, this);
        }
    }
}