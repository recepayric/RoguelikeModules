using System;
using System.Collections.Generic;
using Data.EnemyDataRelated;
using DG.Tweening;
using EnemyMoveBehaviours;
using Runtime.AilmentsRelated;
using Runtime.Configs;
using Runtime.DamageRelated;
using Runtime.Effects;
using Runtime.EnemyRelated;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Managers;
using Runtime.Modifiers;
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
    public class Enemy : MonoBehaviour, IPoolObject, IDamageable, IShooter, ISpellCaster, ICursable
    {
        
        //[Header("Data")] public EnemyData enemyData;
        public Transform Transform { get; set; }
        public EnemyBossBehaviour bossBehaviour;

        public GameObject spellPosition;

        public Health health;
        public GameObject projectilePrefab;
        public GameObject projectilePoint;

        //Ailments
        [Header("Ailment Objects")] public GameObject burnAilmentObject;
        public GameObject freezeAilmentObject;
        public GameObject shockAilmentObject;

        public Ailments ailments;

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
        public List<SpellV2> spells;

        public PoolKeys minionPoolKey;

        public List<SpecialModifiers> specialModifiersList;

        //public List<Modifier> modifiers;
        public List<Modifier> modifiersOnStart;
        public List<Modifier> modifiersOnGetHit;
        public List<Modifier> modifiersOnHealthChange;
        public List<Modifier> modifiersOnItemBuy;

        //Current States
        [Header("Current States")] public bool isAttackingEnemy;

        private void Awake()
        {
            SetUpAilments();
            Transform = transform;
            _stats = GetComponent<EnemyStats>();
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyDamageTaker = GetComponent<EnemyDamageTaker>();

            _enemyMovement.Ailments = ailments;
        }

        private void SetUpAilments()
        {
            ailments = new Ailments
            {
                damageable = this,
                gameObject = gameObject,
                burnAilmentObject = burnAilmentObject,
                freezeAilmentObject = freezeAilmentObject,
                shockAilmentObject = shockAilmentObject
            };
            ailments.Initialise();
        }

        private void SetSpecialModifiers()
        {
            for (int i = 0; i < specialModifiersList.Count; i++)
            {
                var modifier = ModifierCreator.GetModifier(specialModifiersList[i]);
                modifier.RegisterUser(gameObject);
                Debug.Log("Adding special modifiers to the enemy! " + modifier.useArea);
                switch (modifier.useArea)
                {
                    case ModifierUseArea.OnStart:
                        if (!modifiersOnStart.Contains(modifier))
                            modifiersOnStart.Add(modifier);
                        break;

                    case ModifierUseArea.OnHit:
                        break;

                    case ModifierUseArea.OnGetHit:
                        Debug.Log("On Get Hit Modifier!");
                        if (!modifiersOnGetHit.Contains(modifier))
                            modifiersOnGetHit.Add(modifier);
                        break;

                    case ModifierUseArea.OnBuyItem:
                        if (!modifiersOnItemBuy.Contains(modifier))
                            modifiersOnItemBuy.Add(modifier);
                        break;

                    case ModifierUseArea.OnUpdate:
                        break;

                    case ModifierUseArea.OnHealthChange:
                        if (!modifiersOnHealthChange.Contains(modifier))
                            modifiersOnHealthChange.Add(modifier);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //modifiers.Add();
            }
        }

        private void RemoveSpecialModifiers()
        {
            modifiersOnStart.Clear();
            modifiersOnGetHit.Clear();
            modifiersOnHealthChange.Clear();
            modifiersOnItemBuy.Clear();
            for (int i = 0; i < specialModifiersList.Count; i++)
            {
                var modifier = ModifierCreator.GetModifier(specialModifiersList[i]);
                modifier.RemoveRegisteredUser(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            ailments.UpdateAilments();
            UpdateDeath();

            if (_stats.AttackType == AttackType.AuraUser && !isAuraOn)
            {
                CastSpell();
            }
        }

        //Perform an animation and attack!
        public void AttackEnemy(bool isFirstAttack)
        {
            if (ailments.isStunned) return;
            var attackTime = isFirstAttack ? 0.1f : _stats.currentAttackSpeed;
            isAttackingEnemy = true;
            //todo change this to regular timer to get rid of dotween
            DOVirtual.DelayedCall(attackTime, () =>
            {
                if (!gameObject.activeSelf) return;
                
                if (_enemyMovement.IsCloseToEnemy())
                {
                    HandleAttack();
                }

                isAttackingEnemy = false;
            });
        }

        public void HandleAttack()
        {
            if (_isDead)
                return;

            if (_stats.AttackType == AttackType.Melee)
                playerScript.DealDamage(_stats.currentDamage, false);
            else if (_stats.AttackType == AttackType.Magic)
            {
                FireProjectile();
            }
            else if (_stats.AttackType == AttackType.Charge)
            {
                _enemyMovement.StartCharging(playerObject.transform.position);
            }
            else if (_stats.AttackType == AttackType.Spell || _stats.AttackType == AttackType.AuraUser)
            {
                CastSpell();
            }
            else if (_stats.AttackType == AttackType.Bomber)
            {
                Explode();
            }
            else if (_stats.AttackType == AttackType.Hive)
            {
                SpawnMinions();
            }
        }

        private void SpawnMinions()
        {
            var projNumber = _stats.currentProjectileNumber;
            var minRadious = 1f;
            var maxRadious = 3f;
            Debug.Log("Spawning Minions! " + projNumber);
            for (int i = 0; i < projNumber; i++)
            {
                var angle = Random.Range(0, 360);
                var radious = Random.Range(minRadious, maxRadious);
                var rad = Mathf.Deg2Rad * angle;
                var x = radious * Mathf.Cos(rad);
                var y = radious * Mathf.Sin(rad);

                var minion = BasicPool.instance.Get(minionPoolKey);
                minion.transform.position = transform.position + new Vector3(x, y, 0);
            }
        }

        [Button]
        public void Explode()
        {
            var explosion = BasicPool.instance.Get(_stats.explosionKey);
            explosion.transform.position = transform.position;
            DictionaryHolder.Explosions[explosion].explosionDamage = _stats.currentDamage;
            DictionaryHolder.Explosions[explosion].SetRange(_stats.currentMaxAttackRange);
            DictionaryHolder.Explosions[explosion].Explode();

            //Destroy(gameObject);
            BasicPool.instance.Return(gameObject);
        }

        public void FireProjectile()
        {
            for (int i = 0; i < _stats.currentProjectileNumber; i++)
            {
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
                sc.isActive = true;
            }
        }

        public GameObject spellObject;

        private void CastSpell()
        {
            if (spellToCast == Spells.None) return;

            if (spellScript != null && spellScript.gameObject.activeSelf)
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Projectile"))
            {
                //todo change this!!!
                //col.GetComponent<Projectile>().HitTarget(this);
            }
        }

        public void DealDamage(float damage, bool isCriticalHit, float knockbackAmount = 0)
        {
            damageTaken += damage;
            UIController.instance.AddDamageText(gameObject, damage, isCriticalHit);
            UpdateHealth();
            //_enemyDamageTaker.DamageTaken();

            if (knockbackAmount > 0 )
                _enemyMovement.AddKnockback(1);

            if (_stats.currentHealth <= 0)
                Die();
            else
            {
                _enemyDamageTaker.DamageTaken();

                for (int i = 0; i < modifiersOnGetHit.Count; i++)
                {
                    modifiersOnGetHit[i].ApplyEffect(this);
                }
            }
        }

        public void AddElementalAilment(ElementModifiers element, float time, float effect, int spreadAmount)
        {
            ailments.AddElementalAilment(element, time, effect, spreadAmount);
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

            if (spellScript != null)
            {
                spellScript.DeActivate();
                isAuraOn = false;
            }

            if (_stats.AttackType == AttackType.Hive)
                SpawnMinions();
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

            playerScript = DictionaryHolder.Player;
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

            var player = DictionaryHolder.Player;
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
            return !_isDead && gameObject.activeSelf;
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            ailments.FinishAllAilments();
            RemoveSpecialModifiers();
            DictionaryHolder.Enemies.Remove(gameObject);
            DictionaryHolder.Damageables.Remove(gameObject);
            isAttackingEnemy = false;
            _isDead = false;
            
            if (_stats.AttackType == AttackType.Boss)
            {
                bossBehaviour.Stop();
            }
        }

        private void Initialise()
        {
            if (_stats.AttackType == AttackType.Boss)
            {
                bossBehaviour.SetUp(this);
            }
        }

        public void OnGet()
        {
            DictionaryHolder.Enemies.Add(gameObject, this);
            DictionaryHolder.Damageables.Add(gameObject, this);
            damageTaken = 0;
            SetSpecialModifiers();
            SetStats();
            Initialise();
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

        public void AddCurse(AllStats stat, float amount)
        {
        }

        public void RemoveCurse(AllStats stat, float amount)
        {
        }
    }
}