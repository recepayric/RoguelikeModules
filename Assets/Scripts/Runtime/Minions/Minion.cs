using System.Collections.Generic;
using DG.Tweening;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Minions
{
    public class Minion : MonoBehaviour, IPoolObject
    {
        public Transform playerTransform;
        public Transform weaponParent;
        public Vector3 positionRelatedToPlayer;
        public Vector3 targetPosition;

        public GameObject targetEnemy;
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
        
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            CheckForTarget();
            CheckDirection();
            CheckForDistance();

            if (!isCloseToTarget)
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * walkSpeed);
        }

        public void EquipWeapon(Weapon weaponToEquip)
        {
            weapon = weaponToEquip;
            weapon.transform.SetParent(weaponParent);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
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
            GetClosestEnemy();
            
            if(!isHitAnimationEnded)
                return;
            
            if (!isMelee || targetEnemy == null)
            {
                walkSpeed = walkSpeedToFollow;
                targetPosition = playerTransform.position + positionRelatedToPlayer;
            }
            else
            {
                walkSpeed = walkSpeedToEnemy;
                targetPosition = GetEnemyPosition();
            }
        }

        private Vector3 GetEnemyPosition()
        {
            var targetPos = targetEnemy.transform.position;

            return targetPos;
        }

        public void ActivateWeapon()
        {
            //Debug.Log("Weapon is activated");
            //weapon.isActivated = true;
            weapon.Activate();
        }

        public void DeactivateWeapon()
        {
            //Debug.Log("Weapon is deactivated");
            //weapon.isActivated = false;
            weapon.DeActivate();
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

        private void GetClosestEnemy()
        {
            if (enemiesInRadius.Count == 0)
            {
                targetEnemy = null;
                weapon.SetEnemy(null);
                return;
            }

            //Check if curent enemy is in the list of enemies
            if (targetEnemy != null && !enemiesInRadius.Contains(targetEnemy))
            {
                targetEnemy = null;
                weapon.SetEnemy(null);
            }

            //update current distance with current enemy
            if (targetEnemy != null)
                closestEnemyDistance = Vector3.Distance(transform.position, targetEnemy.transform.position);

            for (int i = 0; i < enemiesInRadius.Count; i++)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemiesInRadius[i].gameObject;
                    closestEnemyDistance = Vector3.Distance(transform.position, targetEnemy.transform.position);
                    continue;
                }

                var distanceToCompare = Vector3.Distance(transform.position, enemiesInRadius[i].transform.position);
                if (distanceToCompare < closestEnemyDistance)
                {
                    closestEnemyDistance = distanceToCompare;
                    targetEnemy = enemiesInRadius[i].gameObject;
                }
            }

            weapon.SetEnemy(targetEnemy);
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


        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
        }

        public void OnGet()
        {
            ScriptDictionaryHolder.Minions.Add(gameObject, this);
        }
    }
}