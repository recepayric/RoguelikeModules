using System;
using System.Collections.Generic;
using Data;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.ItemsRelated;
using Runtime.Managers;
using Runtime.Minions;
using Runtime.Modifiers;
using Runtime.PlayerRelated;
using Runtime.StatValue;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime
{
    public class Player : MonoBehaviour
    {
        public ActiveCharacterSo activeCharacterSo;
        public PlayerLevel playerLevel;
        public Health healthBar;
        public CharacterDataSo characterDataSo;
        public PlayerTargetFollower playerTargetFollower;
        public LineRenderer LineRenderer;
        public GameObject WeaponPoint;

        public Stats stats;

        public List<Item> items;
        public List<Weapon> weapons;
        public List<Minion> minions;
        public List<GameObject> weaponObjects;
        public List<Weapon> allWeapons;
        public List<LevelUpStats> levelUpStatsList;
        public Weapon equippedWeapon;

        public GameObject weaponPrefab;

        public float weaponRadius;

        public float baseRange = 10;
        public float finalRange;

        public float damageTaken;
        public float maxHealth;
        public float currentHealth;

        public List<GameObject> enemiesInRadius;

        public GameObject closestEnemy;
        public bool isEnemyWithinRange;

        public List<SpecialModifiers> specialModifiersList;
        //public List<Modifier> modifiers;
        public List<Modifier> modifiersOnStart;
        public List<Modifier> modifiersOnGetHit;
        public List<Modifier> modifiersOnHealthChange;
        public List<Modifier> modifiersOnItemBuy;


        // Start is called before the first frame update
        void Start()
        {
            playerLevel.SetPlayerData(characterDataSo, stats);
            SetSpecialModifiers();
            CalculateBaseStats();
            ApplySpecialModifiers();

            var circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.radius = baseRange + (stats.range / GameConfig.RangeToRadius);

            finalRange = baseRange * GameConfig.RangeToRadius + stats.range;

            ScriptDictionaryHolder.Player = this;

            //stats.SetStats();
            //UpdateStatsWithItems();
            
            healthBar.SetMaxHealth(stats.GetStat(AllStats.MaxHealth));
            
            AddEvents();
            
            SetActiveCharacter();
        }

        private void SetActiveCharacter()
        {
            activeCharacterSo = Resources.Load<ActiveCharacterSo>("CharacterData/ActiveCharacterData");
            activeCharacterSo.playerObject = gameObject;
            activeCharacterSo.playerScript = this;
            activeCharacterSo.playerWeapons = weapons;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                AddWeapon();

            CheckForCollectables();
            CheckEnemies();
            UpdateHealth();
        }

        private void CalculateBaseStats()
        {
            stats.SetBaseStat(characterDataSo);
            stats.SetBaseStats();
            stats.CalculateStats();

            damageTaken = 0;
            maxHealth = stats.GetStat(AllStats.MaxHealth);
        }

        private void UpdateHealth()
        {
            currentHealth = maxHealth - damageTaken;
            healthBar.UpdateHealth(currentHealth);

            for (int i = 0; i < modifiersOnHealthChange.Count; i++)
            {
                modifiersOnHealthChange[i].ApplyEffect(this);
            }
        }

        [Button]
        public void Hit(float damage, AttackType attackType)
        {
            damageTaken += damage;

            for (int i = 0; i < modifiersOnGetHit.Count; i++)
            {
                modifiersOnGetHit[i].ApplyEffect(this);
            }
        }

        private void SetSpecialModifiers()
        {
            for (int i = 0; i < specialModifiersList.Count; i++)
            {
                var modifier = ModifierCreator.GetModifier(specialModifiersList[i]);
                modifier.RegisterUser(gameObject);
                switch (modifier.useArea)
                {
                    case ModifierUseArea.OnStart:
                        if (!modifiersOnStart.Contains(modifier))
                            modifiersOnStart.Add(modifier);
                        break;
                    
                    case ModifierUseArea.OnHit:
                        break;
                    
                    case ModifierUseArea.OnGetHit:
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

        public void UpdateWeaponStats()
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].UpdateAttackSpeed();
            }
        }

        private void ApplySpecialModifiers()
        {
            for (int i = 0; i < modifiersOnStart.Count; i++)
            {
                modifiersOnStart[i].ApplyEffect(this);
            }
        }

        private void CheckForCollectables()
        {
            foreach (var collectable in ScriptDictionaryHolder.Collectables)
            {
                var dist = Vector3.Distance(transform.position, collectable.Key.transform.position);
                if (dist <= 2+stats.collectRange)
                    collectable.Value.Collect(transform);
            }
        }

        private void UpdateStatsWithItems()
        {
            stats.SetBaseStats();

            for (int i = 0; i < items.Count; i++)
            {
                //for (int j = 0; j < items[i].quantity; j++)
                // {
                //stats.AddStats(items[i].itemStats);
                // }
            }
            
            stats.CalculateStats();
        }

        private void CalculateStats()
        {
            stats.SetBaseStats();

            for (int i = 0; i < items.Count; i++)
            {
                stats.AddItemStats(items[i]);
            }

            for (int i = 0; i < levelUpStatsList.Count; i++)
            {
                stats.AddLevelUpStat(levelUpStatsList[i]);
            }
            
            stats.CalculateStats();
        }

        public void AddItem(Item itemToAdd)
        {
            items.Add(itemToAdd);
            //UpdateStatsWithItems();
            
            CalculateStats();
        }

        private void OnItemBuy(Item item)
        {
            items.Add(item);
            CalculateStats();
        }

        public void OnLevelUpStatSelected(LevelUpStats levelUpStat)
        {
            if(!levelUpStatsList.Contains(levelUpStat))
                levelUpStatsList.Add(levelUpStat);
            
            CalculateStats();
        }

        //todo need optimisation! 
        private void CheckEnemies()
        {
            GameObject closestEnemy = null;
            float closestDistance = 10000;

            foreach (var enemies in ScriptDictionaryHolder.Enemies)
            {
                var distance = Vector3.Distance(transform.position, enemies.Key.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemies.Key;
                }
            }

            if (closestEnemy != null)
            {
                this.closestEnemy = closestEnemy;
                LineRenderer.startColor = Color.red;
                LineRenderer.endColor = Color.red;
                LineRenderer.SetPosition(0, transform.position);
                LineRenderer.SetPosition(1, closestEnemy.transform.position);

                playerTargetFollower.SetTarget(closestEnemy);
                playerTargetFollower.SetTargeting(true);
                SetWeaponEnemy(closestEnemy, closestDistance);
            }
            else
            {
                LineRenderer.SetPosition(0, transform.position);
                LineRenderer.SetPosition(1, transform.position);
                SetWeaponEnemy(closestEnemy, closestDistance);
                playerTargetFollower.SetTarget(null);
                playerTargetFollower.SetTargeting(false);
            }
        }

        public void SetWeaponEnemy(GameObject enemy, float distance)
        {
            if (equippedWeapon != null)
                equippedWeapon.SetEnemy(enemy, distance);
        }

        public void CheckIfCanAttack()
        {
        }

        public void SendMinionsToEnemy()
        {
        }

        public Transform explosionPoint;

        [Button]
        public void GetExplosion()
        {
            var obj = BasicPool.instance.Get(PoolKeys.Explosion1);
            obj.transform.position = explosionPoint.position;

            ScriptDictionaryHolder.Explosions[obj].explosionRange = 1;
            ScriptDictionaryHolder.Explosions[obj].Explode();
        }

        [Button]
        public void AddWeapon()
        {
            var weapon = Instantiate(weaponPrefab, transform, true);
            var weaponScript = weapon.GetComponent<Weapon>();

            weaponScript.characterStats = stats;

            weaponObjects.Add(weapon);
            weapons.Add(weaponScript);

            if (weaponObjects.Count == 1)
            {
                weapon.transform.SetParent(WeaponPoint.transform);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localScale = Vector3.one;
                weapon.transform.localRotation = Quaternion.identity;
                equippedWeapon = weaponScript;
            }
            else
            {
                var minion = BasicPool.instance.Get(PoolKeys.Minion1);
                var minionScript = ScriptDictionaryHolder.Minions[minion];
                minionScript.EquipWeapon(weaponScript);
                minionScript.SetEnemyList(enemiesInRadius);
                minions.Add(minionScript);
                OrganiseWeapons();
            }
            
            EventManager.Instance.WeaponsUpdated();
        }

        [Button]
        public void RemoveWeapons()
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                Destroy(weaponObjects[i]);
            }

            weapons.Clear();
            weaponObjects.Clear();

            OrganiseWeapons();
        }

        [Button]
        public void OrganiseWeapons()
        {
            float angleBetween = 360f / weapons.Count;
            for (int i = 0; i < weapons.Count; i++)
            {
                var angle = angleBetween * i * Mathf.Deg2Rad;
                var posX = weaponRadius * Mathf.Cos(angle);
                var posY = weaponRadius * Mathf.Sin(angle);
                //weaponObjects[i].transform.localPosition = new Vector3(posX, posY, 0);
                minions[i].SetDefaultPosition(new Vector3(posX, posY, 0), transform);
            }
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                enemiesInRadius.Add(col.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemiesInRadius.Remove(other.gameObject);
            }
        }

        private void AddEvents()
        {
            EventManager.Instance.LevelUpStatSelectedEvent += OnLevelUpStatSelected;
            EventManager.Instance.ItemBuyEvent += OnItemBuy;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.LevelUpStatSelectedEvent -= OnLevelUpStatSelected;
            EventManager.Instance.ItemBuyEvent -= OnItemBuy;
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }
    }
}