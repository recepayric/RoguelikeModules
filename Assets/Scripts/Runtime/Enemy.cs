using System;
using Data.EnemyDataRelated;
using DG.Tweening;
using EnemyMoveBehaviours;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.PlayerRelated;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour, IPoolObject
    {
        [Header("Data")] public EnemyData enemyData;

        public Health health;

        //Ailments
        [Header("Ailment Objects")] public GameObject burnAilmentObject;
        public GameObject freezeAilmentObject;
        public GameObject shockAilmentObject;

        [Header("Ailments")] public bool isBurning;
        public bool isFrozen;
        public bool isShocked;

        //Ailment Times
        [Header("Ailment Times")] public float burnTime;
        public float freezeTime;
        public float shockTime;

        //Ailment Effects
        [Header("Ailment Effects")] public float burningDamagePerSecond;
        public float freezeEffect;
        public float shockEffect;

        public float burningTimer;

        public float xBound = 8.5f;
        public float yBound = 4.5f;

        public bool moveAround = true;
        public bool isImmortal = false;

        [Header("Stats")] public float currentHealth;
        public float currentMaxHealth;
        public float currentSpeed;
        public float currentDamage;
        public float currentAttackSpeed;
        public float currentEvasion;
        public float currentDefence;
        public float currentAttackRange;
        public float currentMaxAttackRange;
        public AttackType AttackType;

        public float damageTaken;

        public Player playerScript;
        public GameObject playerObject;

        //Current States
        [Header("Current States")] public bool isAttackingEnemy;

        // Start is called before the first frame update
        void Start()
        {
            //health = GetComponent<Health>();
            if (moveAround)
                StartRandomMoving();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAilments();
        }

        private void FixedUpdate()
        {
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            if (isAttackingEnemy) return;

            if (IsCloseToEnemy())
            {
                isAttackingEnemy = true;
                AttackEnemy();
                return;
            }

            var posX = transform.position.x;
            var posY = transform.position.y;
            var playerX = playerObject.transform.position.x;
            var playerY = playerObject.transform.position.y;

            var angle = Mathf.Atan2(playerY - posY, playerX - posX);

            var deltaX = Time.deltaTime * currentSpeed * Mathf.Cos(angle);
            var deltaY = Time.deltaTime * currentSpeed * Mathf.Sin(angle);

            transform.position += new Vector3(deltaX, deltaY, 0);
        }

        //Perform an animation and attack!
        private void AttackEnemy()
        {
            DOVirtual.DelayedCall(currentAttackSpeed, () =>
            {
                if (IsCloseToEnemy())
                {
                    playerScript.Hit(currentDamage, AttackType);
                }

                isAttackingEnemy = false;
            });
        }

        private bool IsCloseToEnemy()
        {
            var distance = Vector3.Distance(transform.position, playerObject.transform.position);
            var distanceToCheck = isAttackingEnemy ? currentMaxAttackRange : currentAttackRange;
            if (distance <= distanceToCheck)
                return true;
            else
                return false;
        }

        private void UpdateAilments()
        {
            if (burnTime > 0)
            {
                burningTimer += Time.deltaTime;
                if (burningTimer >= 1)
                {
                    burningTimer -= 1;
                    GetHit((int)burningDamagePerSecond, false);
                }

                burnTime -= Time.deltaTime;
                if (burnTime <= 0)
                {
                    isBurning = false;
                    FinishBurn();
                }
            }


            if (freezeTime > 0)
            {
                freezeTime -= Time.deltaTime;
                if (freezeTime <= 0)
                {
                    isFrozen = false;
                    FinishFreeze();
                }
            }


            if (shockTime > 0)
            {
                shockTime -= Time.deltaTime;
                if (shockTime <= 0)
                {
                    isShocked = false;
                    FinishShock();
                }
            }
        }

        #region Burn

        public void AddBurning(float burnTimeToAdd, float burningDamage)
        {
            burningDamagePerSecond = burningDamage;
            burnTime = burnTimeToAdd;
            burnAilmentObject.SetActive(true);
        }

        private void FinishBurn()
        {
            burningTimer = 1;
            burningDamagePerSecond = 0;
            burnAilmentObject.SetActive(false);
        }

        #endregion

        #region Freeze

        public void AddFreeze(float freezeTimeToAdd, float freezeEffect)
        {
            freezeTime = freezeTimeToAdd;
            this.freezeEffect = freezeEffect;
            freezeAilmentObject.SetActive(true);
        }

        private void FinishFreeze()
        {
            freezeEffect = 0;
            freezeAilmentObject.SetActive(false);
        }

        #endregion

        #region Shock

        public void AddShock(float shockTimeToAdd, float shockEffect)
        {
            shockTime = shockTimeToAdd;
            this.shockEffect = shockEffect;
            shockAilmentObject.SetActive(true);
        }

        private void FinishShock()
        {
            shockEffect = 0;
            shockAilmentObject.SetActive(false);
        }

        #endregion


        public void StartRandomMoving()
        {
            var randX = Random.Range(-xBound, xBound);
            var randY = Random.Range(-yBound, yBound);

            if (randX > transform.position.x)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                if (randX > transform.position.x)
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
            }

            transform.DOMove(new Vector3(randX, randY, 0), 3).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1, () => StartRandomMoving());
            });
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Projectile"))
            {
                //todo change this!!!
                col.GetComponent<Projectile>().HitTarget(this);
            }
        }

        public void GetHit(int damage, bool isCriticalHit)
        {
            damageTaken += damage;
            UIController.instance.AddDamageText(gameObject, damage, isCriticalHit);
            UpdateHealth();
            if (!isImmortal)
            {
                //BasicPool.instance.Return(gameObject);
                Die();
            }
        }

        private void UpdateHealth()
        {
            currentHealth = currentMaxHealth - damageTaken;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }

            health.UpdateHealth(currentHealth);
        }

        private void Die()
        {
            BasicPool.instance.Return(gameObject);
            ItemDropManager.instance.DropItemFromEnemy(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
        }

        private void SetStats()
        {
            if (enemyData == null) return;
            if (health == null) health = GetComponent<Health>();

            playerScript = ScriptDictionaryHolder.Player;
            playerObject = playerScript.gameObject;

            currentDamage = enemyData.baseDamage;
            currentMaxHealth = enemyData.baseHealth;
            currentHealth = currentMaxHealth;
            currentSpeed = enemyData.baseMoveSpeed;
            currentAttackSpeed = enemyData.baseAttackSpeed;
            currentAttackRange = enemyData.baseAttackRange;
            currentMaxAttackRange = enemyData.baseMaxAttackRange;
            currentEvasion = enemyData.baseEvasion;
            currentDefence = enemyData.baseDefence;
            AttackType = enemyData.attackType;

            SetTowerStats();

            health.SetMaxHealth(currentMaxHealth);
        }

        private void SetTowerStats()
        {
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            ScriptDictionaryHolder.Enemies.Remove(gameObject);
        }

        public void OnGet()
        {
            ScriptDictionaryHolder.Enemies.Add(gameObject, this);
            SetStats();
        }
    }
}