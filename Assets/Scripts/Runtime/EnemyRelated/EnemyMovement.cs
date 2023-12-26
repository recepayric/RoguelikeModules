using System;
using Data.EnemyDataRelated;
using Runtime.DamageRelated;
using UnityEngine;

namespace Runtime.EnemyRelated
{
    /// <summary>
    /// Only attached to Enemy Objects!!!
    /// Responsible for player movements.
    /// Changes behaviour based on what kind of enemy this is!
    /// </summary>
    public class EnemyMovement : MonoBehaviour
    {
        public GameObject targetObject;
        public Enemy enemy;
        public EnemyStats stats;
        public bool isLookingPlayer;
        
        private void Start()
        {
            targetObject = ScriptDictionaryHolder.Player.gameObject;
            //todo change these todos based on performance check!
            enemy = GetComponent<Enemy>();
            stats = GetComponent<EnemyStats>();
        }
        
        private void CheckForPlayerRotation()
        {
            TurnToPlayer();
        }

        private void TurnToPlayer()
        {
            if (targetObject.transform.position.x <= transform.position.x )
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
            CheckForPlayerRotation();
            //Check player rotation
            //return;
            if (enemy.isDead) return;
            if (enemy.isAttackingEnemy) return;
            if (targetObject == null) return;

            if (IsCloseToEnemy())
            {
                enemy.AttackEnemy();
                return;
            }

            var posX = transform.position.x;
            var posY = transform.position.y;
            var playerX = targetObject.transform.position.x;
            var playerY = targetObject.transform.position.y;

            var angle = Mathf.Atan2(playerY - posY, playerX - posX);

            var deltaX = Time.deltaTime * stats.currentSpeed * Mathf.Cos(angle);
            var deltaY = Time.deltaTime * stats.currentSpeed * Mathf.Sin(angle);

            transform.position += new Vector3(deltaX, deltaY, 0);
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