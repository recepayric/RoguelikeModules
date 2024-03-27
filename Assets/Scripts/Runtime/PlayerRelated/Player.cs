using System;
using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.ItemsRelated;
using Runtime.Managers;
using Runtime.Minions;
using Runtime.Modifiers;
using Runtime.SpellsRelated;
using Runtime.StatValue;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.PlayerRelated
{
    public class Player : MonoBehaviour, ISpellCaster, IDamageable, IPoolObject, IWeaponCarrier, ICursable
    {
        public Transform Transform { get; set; }
        public GameObject HitPoint { get; set; }
        public CharacterDataSo characterDataSo;

        //Helper Scripts
        public SpecialModifierHelper specialModifierHelper;
        public PlayerLevel playerLevel;
        public Health healthBar;
        public Stats stats;

        //Stat Related
        public List<LevelUpStats> levelUpStatsList;
        public Dictionary<AllStats, float> statsFromModifiers = new Dictionary<AllStats, float>();
        public Dictionary<AllStats, float> statsFromCurses = new Dictionary<AllStats, float>();
        //Spell Gamble
        public AllStats statFromGamble;
        public float gambleStatIncrease;
        
        public GameObject spellPosition;
        public GameObject rotatingWeaponParent;
        public ActiveCharacterSo activeCharacterSo;
        public List<Item> items;
        public List<Weapon> weapons;
        public List<Minion> minions;
        public List<GameObject> weaponObjects;
        public List<Weapon> allWeapons;
        public List<Spells> spellsToAdd;
        public List<Spell> spells;
        public float weaponRadius =  2;
        public float baseRange = 10;
        public float damageTaken;
        public float maxHealth;
        public float currentHealth;

        public List<SpecialModifiers> specialModifiersList;
        
        //Status
        private bool isDead = false;

        public void InitialiseVariables()
        {
            activeCharacterSo = Resources.Load<ActiveCharacterSo>("CharacterData/ActiveCharacterData");
            healthBar = GetComponent<Health>();
            playerLevel = GetComponent<PlayerLevel>();
            specialModifierHelper = GetComponent<SpecialModifierHelper>();
        }

        private void Start()
        {
            Initialise();
            AddEvents();
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }

        private void Initialise()
        {
            statsFromModifiers = new Dictionary<AllStats, float>();
            statsFromCurses = new Dictionary<AllStats, float>();
            ResetEverything();
            Transform = transform;
            playerLevel.SetPlayerData(characterDataSo, stats);
            specialModifierHelper.SetSpecialModifiers(specialModifiersList);
            CalculateBaseStats();
            ApplySpecialModifiers();
            DictionaryHolder.Player = this;
            healthBar.SetMaxHealth(stats.GetStat(AllStats.MaxHealth));
            SetActiveCharacter();
            SetStarterSpells();
            AddSpells();
            
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
            //activeCharacterSo = Resources.Load<ActiveCharacterSo>("CharacterData/ActiveCharacterData");
            activeCharacterSo.playerObject = gameObject;
            activeCharacterSo.playerScript = this;
            activeCharacterSo.playerWeapons = weapons;
            activeCharacterSo.playerItems = items;
        }

        // Update is called once per frame
        void Update()
        {
            CheckForCollectables();
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
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            if (maxHealth == 0) return;
            
            currentHealth = maxHealth - damageTaken;

            if (currentHealth <= 0 && !isDead)
            {
                currentHealth = 0;
                EventManager.Instance.PlayerDies();
                isDead = true;
            }
            
            healthBar.UpdateHealth(currentHealth);

            specialModifierHelper.UpdateModifiersOnHealthChange();
        }

        [Button]
        public void DealDamage(float damage, bool isCriticalDamage, Weapon weapon, float knockbackAmount = 0)
        {
            damageTaken += damage;
            specialModifierHelper.UpdateModifiersOnGetHit();
        }

        public void DealDamage(float damage, bool isCriticalHit, float knockbackAmount = 0)
        {
            damageTaken += damage;
            specialModifierHelper.UpdateModifiersOnGetHit();
        }

        public void AddElementalAilment(ElementModifiers element, float time, float effect, int spreadAmount)
        {
            
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
            specialModifierHelper.UpdateModifiersOnStart();
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
        
        [Button]
        public void AddWeapon(GameObject weapon)
        {
            Debug.Log("Adding minion with a weapon!");
            var weaponScript = weapon.GetComponent<Weapon>();
            weaponObjects.Add(weapon);
            weapons.Add(weaponScript);
            var minion = BasicPool.instance.Get(PoolKeys.Minion1);
            minion.transform.position = transform.position + new Vector3(1, 0, 1);
            
            var minionScript = DictionaryHolder.Minions[minion];
            minionScript.EquipWeapon(weaponScript);
            minions.Add(minionScript);
            OrganiseWeapons();
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
                var posZ = weaponRadius * Mathf.Sin(angle);
                Debug.Log(posX + "  " + posZ + "   " + weaponRadius + "   " + angle);
                minions[i].SetDefaultPosition(new Vector3(posX, 0, posZ), transform);
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

       

        public float GetRange()
        {
            return stats.GetStat(AllStats.Range);
        }

        public Vector3 GetSpellPosition()
        {
            return spellPosition.transform.position;
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
            
            weapons.Clear();
            weaponObjects.Clear();
            minions.Clear();
            items.Clear();

            for (int i = 0; i < spells.Count; i++)
            {
                spells[i].StopSpell();
            }
            spells.Clear();
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
            return rotatingWeaponParent.transform;
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
    }
}