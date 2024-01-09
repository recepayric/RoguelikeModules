using System;
using Data.EnemyDataRelated;
using DG.Tweening;
using EnemyMoveBehaviours;
using Runtime.Configs;
using Runtime.DamageRelated;
using Runtime.EnemyRelated;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Managers;
using Runtime.PlayerRelated;
using Runtime.SpellsRelated;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyStats))]
    [RequireComponent(typeof(EnemyDamageTaker))]
    public class Enemy : MonoBehaviour, IPoolObject, IDamageable, IShooter, ISpellCaster
    {
        //[Header("Data")] public EnemyData enemyData;
        public Transform Transform { get; set; }

        public GameObject spellPosition;

        public Health health;
        public GameObject projectilePrefab;
        public GameObject projectilePoint;

        //Todo move these to stats!!
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

        public EnemyStats _stats;
        private EnemyMovement _enemyMovement;
        private EnemyDamageTaker _enemyDamageTaker;

        public Animator animator;
        public BoxCollider2D boxCollider2D;

        private bool _isDead = false;

        public float damageTaken;
        public float dieTimer;

        public Player playerScript;
        public GameObject playerObject;

        public Spells spellToCast;
        public bool isCastingSpell;
        private Spell spellScript;
        public bool isAuraOn = false;

        //Current States
        [Header("Current States")] public bool isAttackingEnemy;


        private void Awake()
        {
            Transform = transform;
            _stats = GetComponent<EnemyStats>();
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyDamageTaker = GetComponent<EnemyDamageTaker>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAilments();
            UpdateDeath();

            if (_stats.AttackType == AttackType.AuraUser && !isAuraOn)
            {
                CastSpell();
            }
        }

        //Perform an animation and attack!
        public void AttackEnemy(bool isFirstAttack)
        {
            var attackTime = isFirstAttack ? 0.1f : _stats.currentAttackSpeed;
            isAttackingEnemy = true;
            //todo change this to regular timer to get rid of dotween
            DOVirtual.DelayedCall(attackTime, () =>
            {
                if (_enemyMovement.IsCloseToEnemy())
                {
                    HandleAttack();
                }

                isAttackingEnemy = false;
            });
        }

        public void HandleAttack()
        {
            if (_stats.AttackType == AttackType.Melee)
                playerScript.DealDamage(_stats.currentDamage, false);
            else if (_stats.AttackType == AttackType.Magic)
            {
                FireProjectile();
            }
            else if (_stats.AttackType == AttackType.Charge)
            {
                _enemyMovement.StartCharging(playerObject.transform.position);
            }else if (_stats.AttackType == AttackType.Spell || _stats.AttackType == AttackType.AuraUser)
            {
                CastSpell();
            }
        }

        public void FireProjectile()
        {
            for (int i = 0; i < _stats.currentProjectileNumber; i++)
            {
                Debug.Log("Firing Projectile!");
                //todo change this to pool and dictionary.
                var projectile = Instantiate(projectilePrefab);
                projectile.transform.position = projectilePoint.transform.position;
                projectile.transform.right = playerObject.transform.position - projectile.transform.position;

                var sc = projectile.GetComponent<Projectile>();
                sc.pierceNum = 0;
                sc.criticalHitChance = _stats.currentCriticalHitChance / 100f;
                sc.criticalHitDamage = _stats.criticalDamageIncrease;
                //sc.SetModifiers(modifiers);
                sc.SetMaxDistance(_stats.currentAttackRange * GameConfig.RangeToRadius * 2);
                sc.SetShooter(this);
            }
        }

        private void CastSpell()
        {
            if (spellToCast == Spells.None) return;

            if (spellScript != null)
            {
                spellScript.StartSpell();
                return;
            }
            
            PoolKeys key = (PoolKeys)Enum.Parse(typeof(PoolKeys), spellToCast.ToString());
            var spell = BasicPool.instance.Get(key);
            
            //todo spell register itself to dicitonary here
            spellScript = spell.GetComponent<Spell>();
            spellScript.SetOwner(Owners.Enemy);
            spellScript.FollowsOwner = true;
            spellScript.SetOwnerScript(this);
            spellScript.StartSpell();
            
            isAuraOn = true;
        }

        private void FinishAllAilments()
        {
            FinishBurn();
            FinishFreeze();
            FinishShock();
        }

        private void UpdateAilments()
        {
            if (burnTime > 0)
            {
                burningTimer += Time.deltaTime;
                if (burningTimer >= 1)
                {
                    burningTimer -= 1;
                    DealDamage((int)burningDamagePerSecond, false);
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
            burnTime = 0;
            burningTimer = 0;
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
            freezeTime = 0;
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
            shockTime = 0;
            shockEffect = 0;
            shockAilmentObject.SetActive(false);
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Projectile"))
            {
                //todo change this!!!
                //col.GetComponent<Projectile>().HitTarget(this);
            }
        }


        public void DealDamage(float damage, bool isCriticalHit)
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
            _isDead = true;
            boxCollider2D.enabled = false;
            animator.SetBool("IsDead", true);
            animator.SetFloat("DieSpeed", 1f / AnimationConfig.DieAnimationTime);
            animator.SetTrigger("Die");
            dieTimer = AnimationConfig.DieAnimationTime * 1.25f;
        }

        private void ReturnObject()
        {
            ItemDropManager.instance.DropItemFromEnemy(this);
            BasicPool.instance.Return(gameObject);
        }

        private void SetStats()
        {
            //if (enemyData == null) return;
            if (health == null) health = GetComponent<Health>();

            playerScript = ScriptDictionaryHolder.Player;
            playerObject = playerScript.gameObject;

            _stats.SetStats();

            _isDead = false;
            boxCollider2D.enabled = true;
            animator.SetBool("IsDead", false);
        }


        public float castTime;
        public float castTimer;

        public float accuracyRange;

        [Button]
        private void StartCastingSpell()
        {
            if (spellToCast == Spells.None) return;

            castTimer += Time.deltaTime;
            if (castTimer < castTime) return;

            castTimer -= castTime;
            PoolKeys key = (PoolKeys)Enum.Parse(typeof(PoolKeys), spellToCast.ToString());

            var spell = BasicPool.instance.Get(key);

            //todo spell register itself to dicitonary here
            var spellScript = spell.GetComponent<Spell>();

            var player = ScriptDictionaryHolder.Player;
            spellScript.SetOwner(Owners.Enemy);

            var pos = player.transform.position;
            var randX = Random.Range(-accuracyRange, accuracyRange);
            var randY = Random.Range(-accuracyRange, accuracyRange);
            pos += new Vector3(randX, randY, 0);
            spellScript.SetPosition(pos);
            spellScript.Activate();
        }

        public bool IsAvailable()
        {
            return !_isDead;
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            FinishAllAilments();
            ScriptDictionaryHolder.Enemies.Remove(gameObject);
        }

        public void OnGet()
        {
            ScriptDictionaryHolder.Enemies.Add(gameObject, this);
            SetStats();
        }

        private void UpdateDeath()
        {
            if (_isDead)
            {
                dieTimer -= Time.deltaTime;
                if (dieTimer <= 0)
                {
                    ReturnObject();
                }
            }
        }

        public float GetDamage()
        {
            return _stats.currentDamage;
        }

        public float GetCriticalDamageChance()
        {
            return _stats.currentCriticalHitChance;
        }

        public float GetRange()
        {
            return _stats.currentAttackRange * GameConfig.RangeToRadius;
        }

        public Vector3 GetSpellPosition()
        {
            return spellPosition.transform.position;
        }

        public void AddGambleStat(AllStats stat, float increase)
        {
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}