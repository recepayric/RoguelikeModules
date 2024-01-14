using System;
using Data.EnemyDataRelated;
using Runtime.DamageRelated;
using Runtime.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.EnemyRelated
{
    /// <summary>
    /// Only attached to Enemy Objects!!!
    /// Responsible for player movements.
    /// Changes behaviour based on what kind of enemy this is!
    /// </summary>
    public class EnemyMovement : MonoBehaviour
    {
        public Rigidbody2D rigidBody;
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

        private void Start()
        {
            targetObject = DictionaryHolder.Player.gameObject;
            //todo change these todos based on performance check!
            enemy = GetComponent<Enemy>();
            stats = GetComponent<EnemyStats>();
            rigidBody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void CheckForPlayerRotation()
        {
            TurnToPlayer();
        }

        public void StartCharging(Vector3 targetPosition)
        {
            chargePosition = targetPosition;
            windShield = BasicPool.instance.Get(PoolKeys.WindShield);
            windShield.transform.SetParent(windShieldHolder.transform);
            windShield.transform.localPosition = Vector3.zero;
            isCharging = true;
            lastChargeDistance = 9999999;
        }

        private void FinishCharging()
        {
            BasicPool.instance.Return(windShield);
            isCharging = false;
        }

        private void TurnToPlayer()
        {
            if (isCharging) return;

            if (targetObject.transform.position.x <= transform.position.x)
            {
                if (transform.localScale.x < 0)
                {
                    var scale = transform.localScale;
                    transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
            }
            else
            {
                if (transform.localScale.x > 0)
                {
                    var scale = transform.localScale;
                    transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
            }
        }

        private void FixedUpdate()
        {
            //return;
            if (isCharging)
            {
                Charge();
                return;
            }
            
            CheckForPlayerRotation();
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

            var speedMult = isCharging ? 2 : 1;

            var deltaX = Time.deltaTime * stats.currentSpeed * speedMult * Mathf.Cos(angle);
            var deltaY = Time.deltaTime * stats.currentSpeed * speedMult * Mathf.Sin(angle);

            //transform.position += new Vector3(deltaX, deltaY, 0);
            rigidBody.MovePosition(transform.position + new Vector3(deltaX, deltaY, 0));
        }

        
        private void Charge()
        {
            var posX = transform.position.x;
            var posY = transform.position.y;
            var playerX = chargePosition.x;
            var playerY = chargePosition.y;

            var angle = Mathf.Atan2(playerY - posY, playerX - posX);

            var speedMult = isCharging ? 2 : 1;

            var deltaX = Time.deltaTime * stats.currentSpeed * speedMult * Mathf.Cos(angle);
            var deltaY = Time.deltaTime * stats.currentSpeed * speedMult * Mathf.Sin(angle);

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