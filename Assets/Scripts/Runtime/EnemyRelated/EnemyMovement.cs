using System;
using Data.EnemyDataRelated;
using Runtime.AilmentsRelated;
using Runtime.Configs;
using Runtime.DamageRelated;
using Runtime.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime.EnemyRelated
{
    /// <summary>
    /// Only attached to Enemy Objects!!!
    /// Responsible for player movements.
    /// Changes behaviour based on what kind of enemy this is!
    /// </summary>
    public class EnemyMovement : MonoBehaviour
    {
        public Ailments Ailments;
        public Rigidbody rigidBody;
        public GameObject targetObject;
        public Enemy enemy;
        public EnemyStats stats;
        public bool isLookingPlayer;

        public Vector3 chargePosition;
        public bool isCharging;

        public GameObject windShield;
        public GameObject windShieldHolder;

        public float lastChargeDistance;
        
        public bool isFirstAttack = true;
        private Animator _animator;
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        public bool isKnockbacked = false;
        public float knockbackAmount = -1f;
        public float targetKnockbackAmount;
        public float knockbackTime = 0.25f;

        public float speedAfterFreeze;

        public bool hasNoTarget;

        private void Start()
        {
            if (DictionaryHolder.Player != null)
                targetObject = DictionaryHolder.Player.gameObject;
            //todo change these todos based on performance check!
            enemy = GetComponent<Enemy>();
            stats = GetComponent<EnemyStats>();
            rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        public void AddKnockback(float knockbackAmount)
        {
            if (isCharging) return;
            isKnockbacked = true;
            this.knockbackAmount = knockbackAmount;
            targetKnockbackAmount = -knockbackAmount;
        }

        private void UpdateKnockback()
        {
            if (!isKnockbacked) return;
            knockbackAmount -= Time.deltaTime / knockbackTime;
            if (knockbackAmount <= -1f)
            {
                knockbackAmount = -1f;
                isKnockbacked = false;
            }
        }

        
        public void StartCharging(Vector3 targetPosition)
        {
            chargePosition = targetPosition;
            windShield = BasicPool.instance.Get(PoolKeys.WindShield);
            windShield.transform.SetParent(windShieldHolder.transform);
            windShield.transform.localPosition = Vector3.zero;
            isCharging = true;
            lastChargeDistance = 9999999;
            rigidBody.mass = 300;
        }

        private void FinishCharging()
        {
            BasicPool.instance.Return(windShield);
            isCharging = false;
            rigidBody.mass = 1;
        }
        
        private void FixedUpdate()
        {

            speedAfterFreeze = stats.currentSpeed - stats.currentSpeed * (Ailments.freezeEffect/100f);
            //return;
            
            UpdateKnockback();
            
            if (isCharging)
            {
                Charge();
                return;
            }
            if (Ailments.isStunned) return;

            if(!hasNoTarget)
                MoveTowardsPlayer();
            else
                MoveRandomly();
        }

        private float timeToChangeTarget = 3;
        private float changeTargetTimer = 3;
        private float randomTargetX;
        private float randomTargetZ;
        private void MoveRandomly()
        {
            changeTargetTimer -= Time.deltaTime;

            if (changeTargetTimer <= 0)
            {
                changeTargetTimer = timeToChangeTarget;
                GetRandomPosition();
            }
            
            var posX = transform.position.x;
            var posY = transform.position.z;
            
            var angle = Mathf.Atan2(randomTargetZ - posY, randomTargetX - posX);
            angle *= Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            transform.forward = new Vector3(randomTargetX, 0, randomTargetZ) - transform.position;

            var speedMult = isCharging ? 2 : 1;

            var deltaX = Time.deltaTime * speedAfterFreeze * speedMult * Mathf.Cos(angle);
            var deltaY = Time.deltaTime * speedAfterFreeze * speedMult * Mathf.Sin(angle);

            var MoveAmount = new Vector3(deltaX, 0, deltaY);
            var knockbackSpeed = isKnockbacked ?  -knockbackAmount : 1;
            MoveAmount *= knockbackSpeed;

            //transform.position += new Vector3(deltaX, deltaY, 0);
            rigidBody.MovePosition(transform.position + MoveAmount);
        }


        private void MoveTowardsPlayer()
        {
            if (targetObject == null)
            {
                hasNoTarget = true;
                return;
            }
            //Check player rotation
            //return;
            if (!enemy.IsAvailable()) return;
            if (enemy.isAttackingEnemy) return;
            if (targetObject == null) return;

            if (IsCloseToEnemy())
            {
                enemy.AttackEnemy(isFirstAttack);
                isFirstAttack = false;
                _animator.SetBool(IsRunning, false);
                return;
            }

            _animator.SetBool(IsRunning, true);
            isFirstAttack = true;
            var posX = transform.position.x;
            var posY = transform.position.y;
            var playerX = targetObject.transform.position.x;
            var playerY = targetObject.transform.position.y;

            var angle = Mathf.Atan2(playerY - posY, playerX - posX);
            transform.forward =  targetObject.transform.position - transform.position;

            var speedMult = isCharging ? 2 : 1;

            var deltaX = Time.deltaTime * speedAfterFreeze * speedMult * Mathf.Cos(angle);
            var deltaY = Time.deltaTime * speedAfterFreeze * speedMult * Mathf.Sin(angle);

            var MoveAmount = new Vector3(deltaX, deltaY, 0);
            //Debug.Log(transform.forward);
            MoveAmount = transform.forward * Time.deltaTime * speedAfterFreeze * speedMult;
            var knockbackSpeed = isKnockbacked ?  -knockbackAmount : 1;
            MoveAmount *= knockbackSpeed;

            //transform.position += new Vector3(deltaX, deltaY, 0);
            rigidBody.MovePosition(transform.position + MoveAmount);
        }
        
        
        private void GetRandomPosition()
        {
            randomTargetX = Random.Range(-GameConfig.MapWidth, GameConfig.MapWidth);
            randomTargetZ = Random.Range(-GameConfig.MapHeight, GameConfig.MapHeight);
        }

        
        private void Charge()
        {
            if (Ailments.isStunned)
            {
                FinishCharging();
                return;
            }

            var posX = transform.position.x;
            var posY = transform.position.y;
            var playerX = chargePosition.x;
            var playerY = chargePosition.y;

            var angle = Mathf.Atan2(playerY - posY, playerX - posX);

            var speedMult = isCharging ? 2 : 1;

            var deltaX = Time.deltaTime * speedAfterFreeze * speedMult * Mathf.Cos(angle);
            var deltaY = Time.deltaTime * speedAfterFreeze * speedMult * Mathf.Sin(angle);

            transform.position += new Vector3(deltaX, deltaY, 0);

            var distance = Vector3.Distance(transform.position, chargePosition);
            
            if (distance > lastChargeDistance)
            {
                FinishCharging();
            }
            else
            {
                lastChargeDistance = distance;
            }
        }

        public bool IsCloseToEnemy()
        {
            var distance = Vector3.Distance(transform.position, targetObject.transform.position);
            var distanceToCheck = enemy.isAttackingEnemy ? stats.currentMaxAttackRange : stats.currentAttackRange;
            if (distance <= distanceToCheck)
                return true;

            return false;
        }
    }
}