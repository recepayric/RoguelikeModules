using System;
using Data.EnemyDataRelated;
using DG.Tweening;
using EnemyMoveBehaviours;
using Runtime.Configs;
using Runtime.DamageRelated;
using Runtime.EnemyRelated;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.PlayerRelated;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyStats))]
    [RequireComponent(typeof(EnemyDamageTaker))]
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

        private EnemyStats _stats;
        private EnemyMovement _enemyMovement;
        private EnemyDamageTaker _enemyDamageTaker;

        public Animator animator;
        public BoxCollider2D boxCollider2D;

        public bool isDead = false;
        
        public float damageTaken;
        public float dieTimer;

        public Player playerScript;
        public GameObject playerObject;
        

        //Current States
        [Header("Current States")] public bool isAttackingEnemy;


        private void Awake()
        {
            _stats = GetComponent<EnemyStats>();
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyDamageTaker = GetComponent<EnemyDamageTaker>();
        }

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
            
            UpdateDeath();
        }

        private void UpdateDeath()
        {
            if (isDead)
            {
                dieTimer -= Time.deltaTime;
                if (dieTimer <= 0)
                {
                    ReturnObject();
                }
            }
        }

        private void FixedUpdate()
        {
        }


        //Perform an animation and attack!
        public void AttackEnemy()
        {
            isAttackingEnemy = true;
            DOVirtual.DelayedCall(_stats.currentAttackSpeed, () =>
            {
                if (_enemyMovement.IsCloseToEnemy())
                {
                    playerScript.Hit(_stats.currentDamage, _stats.AttackType);
                }

                isAttackingEnemy = false;
            });
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
            //_enemyDamageTaker.DamageTaken();

            if (_stats.currentHealth <= 0)
                Die();
            else
                _enemyDamageTaker.DamageTaken();
            
            
        }

        private void UpdateHealth()
        {
            _stats.currentHealth = _stats.currentMaxHealth - damageTaken;

            if (_stats.currentHealth <= 0)
            {
                _stats.currentHealth = 0;
                Die();
            }

            health.UpdateHealth(_stats.currentHealth);
        }

        [Header("Immortal")] public bool isImmortal;

        private void Die()
        {
            if (isImmortal) return;
            isDead = true;
            boxCollider2D.enabled = false;
            animator.SetBool("IsDead", true);
            animator.SetFloat("DieSpeed", 1f/AnimationConfig.DieAnimationTime);
            animator.SetTrigger("Die");
            dieTimer = AnimationConfig.DieAnimationTime*1.25f;
            
            //BasicPool.instance.Return(gameObject);
            //ItemDropManager.instance.DropItemFromEnemy(this);
        }

        private void ReturnObject()
        {
            BasicPool.instance.Return(gameObject);
            ItemDropManager.instance.DropItemFromEnemy(this);
        }
        
        
        private void SetStats()
        {
            if (enemyData == null) return;
            if (health == null) health = GetComponent<Health>();

            playerScript = ScriptDictionaryHolder.Player;
            playerObject = playerScript.gameObject;

            _stats.SetStats();
            
            isDead = false;
            boxCollider2D.enabled = true;
            animator.SetBool("IsDead", false);
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