using System;
using System.Collections.Generic;
using Data;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.ItemsRelated;
using Runtime.Managers;
using Runtime.Minions;
using Runtime.Modifiers;
using Runtime.PlayerRelated;
using Runtime.SpellsRelated;
using Runtime.StatValue;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime
{
    public class Player : MonoBehaviour, ISpellCaster, IDamageable, IPoolObject, IWeaponCarrier, ICursable
    {
        public Transform Transform { get; set; }
        public GameObject SpellPosition;
        public GameObject RotatingWeaponParent;
        public ActiveCharacterSo activeCharacterSo;
        public PlayerLevel playerLevel;
        public Health healthBar;
        public CharacterDataSo characterDataSo;
        public PlayerTargetFollower playerTargetFollower;
        public PlayerSwordSwinger PlayerSwordSwinger;
        public AttackHelper AttackHelper;
        public GameObject WeaponPoint;

        public Stats stats;

        public List<Item> items;
        public List<Weapon> weapons;
        public List<Minion> minions;
        public List<GameObject> weaponObjects;
        public List<Weapon> allWeapons;
        public List<LevelUpStats> levelUpStatsList;
        public List<Spells> spellsToAdd;
        public List<Spell> spells;
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

        public Dictionary<AllStats, float> statsFromModifiers = new Dictionary<AllStats, float>();
        public Dictionary<AllStats, float> statsFromCurses = new Dictionary<AllStats, float>();
        
        //Spell Gamble
        public AllStats statFromGamble;
        public float gambleStatIncrease;
        
        //Status
        private bool isDead = false;


        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void Initialise()
        {
            statsFromModifiers = new Dictionary<AllStats, float>();
            statsFromCurses = new Dictionary<AllStats, float>();
            ResetModifiers();
            ResetWeapon();
            ResetEverything();
            Transform = transform;
            playerLevel.SetPlayerData(characterDataSo, stats);
            SetSpecialModifiers();
            CalculateBaseStats();
            ApplySpecialModifiers();
            finalRange = baseRange * GameConfig.RangeToRadius + stats.range;
            DictionaryHolder.Player = this;
            healthBar.SetMaxHealth(stats.GetStat(AllStats.MaxHealth));
            SetActiveCharacter();
            SetStarterSpells();
            AddSpells();
            
        }

        private void ResetModifiers()
        {
            modifiersOnStart.Clear();
            modifiersOnGetHit.Clear();
            modifiersOnHealthChange.Clear();
            modifiersOnItemBuy.Clear();
        }

        private void ResetWeapon()
        {
            //todo send weapon to the pool after adding weapon to poolable!!!
        }

        private void ResetEverything()
        {
            //todo send them to the pools!!!!
            items.Clear();
            spellsToAdd.Clear();
            spells.Clear();
            levelUpStatsList.Clear();
        }

        public void SetStarterWeapon(PoolKeys poolKeyWeapon)
        {
            var weapon = BasicPool.instance.Get(poolKeyWeapon);
            AddWeapon(weapon);
        }

        public void BuyWeapon(PoolKeys weaponPoolKey)
        {
            var weapon = BasicPool.instance.Get(weaponPoolKey);
            AddWeapon(weapon);
        }
        
        private void SetStarterSpells()
        {
            spellsToAdd.AddRange(characterDataSo.starterSpells);
        }
        
        private void SetActiveCharacter()
        {
            activeCharacterSo = Resources.Load<ActiveCharacterSo>("CharacterData/ActiveCharacterData");
            activeCharacterSo.playerObject = gameObject;
            activeCharacterSo.playerScript = this;
            activeCharacterSo.playerWeapons = weapons;
            activeCharacterSo.playerItems = items;
        }

        // Update is called once per frame
        void Update()
        {
            CheckForCollectables();
            //CheckEnemies();
            UpdateHealth();
        }

        public void CalculateBaseStats()
        {
            stats.SetBaseStat(characterDataSo);
            stats.SetBaseStats();
            stats.CalculateStats();

            damageTaken = 0;
            maxHealth = stats.GetStat(AllStats.MaxHealth);
        }

        private void ResetHealth()
        {
            damageTaken = 0;
            currentHealth = maxHealth;
            //healthBar.UpdateHealth(currentHealth);
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            currentHealth = maxHealth - damageTaken;

            if (currentHealth <= 0 && !isDead)
            {
                currentHealth = 0;
                EventManager.Instance.PlayerDies();
                isDead = true;
            }
            
            healthBar.UpdateHealth(currentHealth);

            for (int i = 0; i < modifiersOnHealthChange.Count; i++)
            {
                modifiersOnHealthChange[i].ApplyEffect(this);
            }
        }

        [Button]
        public void DealDamage(float damage, bool isCriticalDamage, Weapon weapon, float knockbackAmount = 0)
        {
            damageTaken += damage;

            for (int i = 0; i < modifiersOnGetHit.Count; i++)
            {
                modifiersOnGetHit[i].ApplyEffect(this);
            }
        }

        public void DealDamage(float damage, bool isCriticalHit, float knockbackAmount = 0)
        {
            damageTaken += damage;

            for (int i = 0; i < modifiersOnGetHit.Count; i++)
            {
                modifiersOnGetHit[i].ApplyEffect(this);
            }
        }

        public void AddElementalAilment(ElementModifiers element, float time, float effect, int spreadAmount)
        {
            
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
        
        private void RemoveSpecialModifiers()
        {
            for (int i = 0; i < specialModifiersList.Count; i++)
            {
                var modifier = ModifierCreator.GetModifier(specialModifiersList[i]);
                modifier.RegisterUser(gameObject);
                modifier.RemoveEffect(this);
                switch (modifier.useArea)
                {
                    case ModifierUseArea.OnStart:
                        if (modifiersOnStart.Contains(modifier))
                            modifiersOnStart.Remove(modifier);
                        break;
                    
                    case ModifierUseArea.OnHit:
                        break;
                    
                    case ModifierUseArea.OnGetHit:
                        if (modifiersOnGetHit.Contains(modifier))
                            modifiersOnGetHit.Remove(modifier);
                        break;
                    
                    case ModifierUseArea.OnBuyItem:
                        if (modifiersOnItemBuy.Contains(modifier))
                            modifiersOnItemBuy.Remove(modifier);
                        break;
                    
                    case ModifierUseArea.OnUpdate:
                        break;
                    
                    case ModifierUseArea.OnHealthChange:
                        if (modifiersOnHealthChange.Contains(modifier))
                            modifiersOnHealthChange.Remove(modifier);
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
                weapons[i].weaponStats.UpdateAttackSpeed();
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
            foreach (var collectable in DictionaryHolder.Collectables)
            {
                //todo change to magnitude!!
                var dist = Vector3.Distance(transform.position, collectable.Key.transform.position);
                if (dist <= 5+stats.collectRange)
                    collectable.Value.Collect(transform);
            }
        }

        public void AddStatFromModifier(AllStats stat, float value)
        {
            if (statsFromModifiers == null)
                statsFromModifiers = new Dictionary<AllStats, float>();

            if (statsFromModifiers.ContainsKey(stat))
                statsFromModifiers[stat] += value;
            else
                statsFromModifiers.Add(stat, value);
            
            
            CalculateStats();
        }
        
        public void CalculateStats()
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

            foreach (var stat in statsFromModifiers)
            {
                stats.IncreaseStat(stat.Key, stat.Value);
            }
            
            foreach (var stat in statsFromCurses)
            {
                stats.IncreaseStat(stat.Key, stat.Value);
            }

            if (statFromGamble == AllStats.Strength || statFromGamble == AllStats.Dexterity ||
                statFromGamble == AllStats.Intelligence || statFromGamble == AllStats.Magic)
            {
                var currentValue = stats.GetStat(statFromGamble);
                var increase = currentValue * (gambleStatIncrease / 100f);
                
                stats.IncreaseStat(statFromGamble, increase);
                stats.CalculateStats();
            }
            else
            {
                stats.CalculateStats();
                var currentValue = stats.GetStat(statFromGamble);
                var increase = currentValue * (gambleStatIncrease / 100f);
                stats.IncreaseStat(statFromGamble, increase);
            }
            stats.UpdateStatsWithMultipliers();
            stats.SetStatValues();
            healthBar.UpdateMaxHealth(stats.GetStat(AllStats.MaxHealth));
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

        private void OnWeaponBuy(PoolKeys poolKeys)
        {
            BuyWeapon(poolKeys);
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
            {
                this.closestEnemy = closestEnemy;
                playerTargetFollower.SetTarget(closestEnemy);
                playerTargetFollower.SetTargeting(true);
                SetWeaponEnemy(closestEnemy, closestDistance);
            }
            else
            {
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
        
        [Button]
        public void AddWeapon(GameObject weapon)
        {
            //var weapon = Instantiate(weaponPrefab, transform, true);
            //weapon.transform.SetParent(transform);
            var weaponScript = weapon.GetComponent<Weapon>();

            //weaponScript.characterStats = stats;

            weaponObjects.Add(weapon);
            weapons.Add(weaponScript);

            if (weaponObjects.Count == 0)
            {
                weapon.transform.SetParent(WeaponPoint.transform);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localScale = Vector3.one;
                weapon.transform.localRotation = Quaternion.identity;
                weaponScript.SetSwordSwinger(PlayerSwordSwinger);
                weaponScript.AttackHelper = AttackHelper;
                weaponScript.WeaponCarrier = this;
                equippedWeapon = weaponScript;
            }
            else
            {
                Debug.Log("Adding minion with a weapon!");
                var minion = BasicPool.instance.Get(PoolKeys.Minion1);
                var minionScript = DictionaryHolder.Minions[minion];
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
            //if (weapons.Count == 0) return;
            
            float angleBetween = 360f / (weapons.Count);
            for (int i = 0; i < weapons.Count; i++)
            {
                var angle = angleBetween * i * Mathf.Deg2Rad;
                var posX = weaponRadius * Mathf.Cos(angle);
                var posY = weaponRadius * Mathf.Sin(angle);
                Debug.Log(posX + "  " + posY + "   " + weaponRadius + "   " + angle);
                //weaponObjects[i].transform.localPosition = new Vector3(posX, posY, 0);
                minions[i].SetDefaultPosition(new Vector3(posX, posY, 0), transform);
            }
        }

        private void OnFloorStarts()
        {
            CalculateStats();
            AddSpells();
            ResetHealth();
            isDead = false;
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].OnFloorStart();
            }
            
            CastSpells();
        }

        private void OnFloorEnds(int floorNum)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].OnFloorEnds();
            }
        }

        public void ResetStatsForMarket()
        {
            ResetHealth();
            statsFromCurses.Clear();
            CalculateStats();
        }

        private void AddEvents()
        {
            EventManager.Instance.FloorStartsEvent += OnFloorStarts;
            EventManager.Instance.FloorEndsEvent += OnFloorEnds;
            EventManager.Instance.LevelUpStatSelectedEvent += OnLevelUpStatSelected;
            EventManager.Instance.ItemBuyEvent += OnItemBuy;
            EventManager.Instance.WeaponBuyEvent += OnWeaponBuy;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.FloorStartsEvent -= OnFloorStarts;
            EventManager.Instance.FloorEndsEvent -= OnFloorEnds;
            EventManager.Instance.LevelUpStatSelectedEvent -= OnLevelUpStatSelected;
            EventManager.Instance.ItemBuyEvent -= OnItemBuy;
            EventManager.Instance.WeaponBuyEvent -= OnWeaponBuy;
        }

        public float GetRange()
        {
            return stats.GetStat(AllStats.Range);
        }

        public Vector3 GetSpellPosition()
        {
            return SpellPosition.transform.position;
        }

        public void AddGambleStat(AllStats stat, float increase)
        {
            statFromGamble = stat;
            gambleStatIncrease = increase;
            CalculateStats();
        }

        public void AddSpells()
        {
            for (int i = 0; i < spellsToAdd.Count; i++)
            {
                spells.Add(CreateSpell(spellsToAdd[i]));
            }
            
            spellsToAdd.Clear();
        }

        private Spell CreateSpell(Spells spellToCreate)
        {
            var poolKey = PoolKeys.BurnAura;
            switch (spellToCreate)
            {
                case Spells.RockFall:
                    break;
                case Spells.BurnAura:
                    poolKey = PoolKeys.BurnAura;
                    break;
                case Spells.ColdAura:
                    poolKey = PoolKeys.ColdAura;
                    break;
                case Spells.ShockAura:
                    poolKey = PoolKeys.ShockAura;
                    break;
                case Spells.GambleForStat:
                    poolKey = PoolKeys.GambleForStat;
                    break;
            }

            var spell = BasicPool.instance.Get(poolKey);
            return spell.GetComponent<Spell>();
        }

        [Button]
        public void CastSpells()
        {
            for (int i = 0; i < spells.Count; i++)
            {
                spells[i].Prepare();
                spells[i].SetOwnerScript(this);
                spells[i].FollowsOwner = true;
                spells[i].StartSpell();
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void ClearPlayer()
        {
            for (int i = 0; i < minions.Count; i++)
            {
                BasicPool.instance.Return(minions[i].gameObject);
            }
            //BasicPool.instance.Return(equippedWeapon.gameObject);
            
            RemoveSpecialModifiers();
            weapons.Clear();
            weaponObjects.Clear();
            minions.Clear();
            items.Clear();

            for (int i = 0; i < spells.Count; i++)
            {
                spells[i].StopSpell();
            }
            spells.Clear();
            AttackHelper.Reset();
        }

        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
            RemoveEvents();
            DictionaryHolder.Damageables.Remove(gameObject);
            DictionaryHolder.Cursable.Remove(gameObject);
            ClearPlayer();
        }

        public void OnGet()
        {
            AddEvents();
            Initialise();
            DictionaryHolder.Damageables.Add(gameObject, this);
            DictionaryHolder.Cursable.Add(gameObject, this);
        }

        public Transform GetRotationgWeaponParent()
        {
            return RotatingWeaponParent.transform;
        }

        public void AddCurse(AllStats stat, float amount)
        {
            Debug.Log("Adding curse!");
            statsFromCurses.Add(stat, amount);
            
            CalculateStats();
            UpdateWeaponStats();
        }

        public void RemoveCurse(AllStats stat, float amount)
        {
            if(statsFromCurses.ContainsKey(stat))
                statsFromCurses.Remove(stat);
            CalculateStats();
            UpdateWeaponStats();
        }
    }
}