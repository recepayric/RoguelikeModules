using System;
using System.Collections.Generic;
using Data.WeaponDataRelated;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Managers;
using Runtime.Modifiers;
using Runtime.ParticleShaderScripts;
using Runtime.PlayerRelated;
using Runtime.ProjectileRelated;
using Runtime.SpellsRelated;
using Runtime.WeaponRelated;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    [RequireComponent(typeof(WeaponLevelSystem))]
    [RequireComponent(typeof(WeaponUpgradeTree))]
    public class Weapon : MonoBehaviour, IShooter, IPoolObject
    {
        //todo delete this later bitch
        public int projectileTier;
        public WeaponLevelSystem weaponLevelSystem;
        public WeaponUpgradeTree weaponUpgradeTree;

        //public WeaponDatasSo weaponDatsSo;
        public WeaponDataSo weaponDataSo;
        public WeaponStats weaponStats;
        public PlayerSwordSwinger swordSwinger;
        public AttackHelper AttackHelper;
        public IWeaponCarrier WeaponCarrier;
        private float _timer;
        private float distanceToEnemy;

        //todo change to poolkey
        public GameObject[] projectilePrefabsPerTier;
        public GameObject projectilePrefab;
        public GameObject explodingProjectile;
        public GameObject sphereProjectile;
        public GameObject projectilePoint;
        public PoolKeys slashKey;

        public GameObject targetEnemy;
        public GameObject rotatingWeaponParent;

        public Dictionary<AllStats, float> statsFromTree;
        public List<SpecialModifiers> specialModifiersFromTree;
        public List<SpecialModifiers> specialModifiersList;
        public List<Modifier> modifiers;
        public List<Modifier> modifiersOnStart;
        public List<Modifier> modifiersOnGetHit;
        public List<Modifier> modifiersOnHealthChange;
        public List<Modifier> modifiersOnItemBuy;
        public List<Modifier> modifiersOnBeforeHit;
        public List<GameObject> rotatingSwords;

        public static float RotationGlobal;

        public SwirlObject swirlObject;

        public bool isAttacking = false;

        public List<GameObject> styles;
        public int styleCount;

        public List<SpellV2> spells;
        
        private void Update()
        {
            UpdateSpells();

            if (rotatingWeaponParent != null)
            {
                RotationGlobal += Time.deltaTime * (360f / weaponStats.attackSpeed);
                rotatingWeaponParent.transform.rotation = Quaternion.Euler(0, 0, RotationGlobal);
                rotatingWeaponParent.transform.position = WeaponCarrier.GetRotationgWeaponParent().position;
            }

            if (CanAttack())
            {
                if (!isAttacking)
                {
                    HandleAttack();
                }

                _timer += Time.deltaTime;
                if (_timer >= weaponStats.attackSpeed)
                {
                    CreateProjectile();
                    _timer = 0;
                }
            }
            else
            {
                if (isAttacking)
                {
                    HandleStopAttack();
                }

                _timer = 0;
            }

            if (swirlObject != null)
                swirlObject.transform.position = projectilePoint.transform.position;
        }

        private void UpdateSpells()
        {
            for (int i = 0; i < spells.Count; i++)
            {
                spells[i].Update();
            }
        }

        private void HandleAttack()
        {
            isAttacking = true;
            if (weaponDataSo.WeaponType == WeaponType.Wand)
                WandAttack();
        }

        private void HandleStopAttack()
        {
            isAttacking = false;
            if (weaponDataSo.WeaponType == WeaponType.Wand)
                StopWandAttack();
        }

        private void SwordAttack()
        {
            if (weaponStats.isRotatinSword) return;
            swordSwinger.Swing(weaponStats.attackSpeed);
        }

        private void BowAttack()
        {
            //AttackHelper.weaponType = WeaponType.Bow;
            //AttackHelper.attackSpeed = weaponStats.attackSpeed;
            //AttackHelper.StartAttack();
        }

        private void WandAttack()
        {
            EventManager.Instance.LiftWand(weaponStats.attackSpeed);
            swirlObject.StartSwirling(weaponStats.attackSpeed);
        }

        private void StopWandAttack()
        {
            swirlObject.StopSwirling();
            isAttacking = false;
            EventManager.Instance.DownWand();
        }

        private void ActivateRotatingWeapons()
        {
            if (!weaponStats.isRotatinSword) return;

            if (rotatingWeaponParent == null)
                rotatingWeaponParent = new GameObject();

            rotatingWeaponParent.transform.rotation = Quaternion.identity;

            var amountOfRotatingWeapons = weaponStats.projectileAmount;

            var angleBetween = 360f / amountOfRotatingWeapons;
            var rad = Mathf.Deg2Rad * angleBetween;

            for (int i = 0; i < amountOfRotatingWeapons; i++)
            {
                var projectile = BasicPool.instance.Get(weaponStats.rotatingWeaponKey);
                projectile.transform.SetParent(rotatingWeaponParent.transform);

                var distanceX = weaponStats.rotationDistanceFromPlayer * Mathf.Cos(rad * i);
                var distanceY = weaponStats.rotationDistanceFromPlayer * Mathf.Sin(rad * i);

                projectile.transform.localPosition = new Vector3(distanceX, distanceY, 0);
                projectile.transform.rotation = Quaternion.Euler(0, 0, angleBetween * i + 90);

                var script = DictionaryHolder.RotatingMeleeWeapons[projectile];
                script.SetShooter(this);
            }
        }

        public void AddStatFromTree(AllStats stat, float value)
        {
            if (statsFromTree == null)
                statsFromTree = new Dictionary<AllStats, float>();

            if (statsFromTree.ContainsKey(stat))
                statsFromTree[stat] += value;
            else
                statsFromTree.Add(stat, value);

            weaponStats.SetStats();
        }

        public void AddModifierFromTree(SpecialModifiers specialModifier)
        {
            specialModifiersFromTree.Add(specialModifier);
            AddSpecialModifier(specialModifier);
        }

        public void RemoveModifierFromTree(SpecialModifiers specialModifier)
        {
            specialModifiersFromTree.Remove(specialModifier);
            RemoveSpecialModifier(specialModifier);
        }

        public void SetSlash(Slash slash)
        {
            slash.slashSpeedMultiplier = 1f / weaponStats.attackSpeed;
            slash.SetRange(weaponStats.range);
            slash.SetShooter(this);
            slash.weapon = this;
        }

        public void AddStyle(int number)
        {
            styleCount++;
            UpdateStyle();
        }

        private void UpdateStyle()
        {
            for (int i = 0; i < styles.Count; i++)
            {
                if (i == styleCount)
                    styles[i].SetActive(true);
                else
                    styles[i].SetActive(false);
            }
        }

        private void ActivateModifiers()
        {
            for (var i = 0; i < modifiersOnStart.Count; i++)
            {
                modifiersOnStart[i].ApplyEffect(this);
            }
        }

        private void SetSpecialModifiers(List<SpecialModifiers> pSpecialModifiersList)
        {
            for (int i = 0; i < pSpecialModifiersList.Count; i++)
            {
                AddSpecialModifier(pSpecialModifiersList[i]);
            }
        }
        
        //todo another script!
        private void AddSpecialModifier(SpecialModifiers specialModifier)
        {
            var modifier = ModifierCreator.GetModifier(specialModifier);
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
                case ModifierUseArea.OnBeforeHit:
                    if (!modifiersOnBeforeHit.Contains(modifier))
                        modifiersOnBeforeHit.Add(modifier);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //todo another script!
        private void RemoveSpecialModifier(SpecialModifiers specialModifier)
        {
            var modifier = ModifierCreator.GetModifier(specialModifier);
            modifier.RemoveRegisteredUser(gameObject);
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

                case ModifierUseArea.OnBeforeHit:
                    if (modifiersOnBeforeHit.Contains(modifier))
                        modifiersOnBeforeHit.Remove(modifier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateProjectile()
        {
            if (targetEnemy == null || !targetEnemy.activeSelf || !DictionaryHolder.Enemies[targetEnemy].IsAvailable())
                return;

            weaponStats.totalAttackNumber++;
            weaponStats.totalAttackNumberWithoutGettingHit++;

            EventManager.Instance.PlaySoundOnce(weaponStats.attackSound, 1);

            if (weaponDataSo.WeaponType == WeaponType.Sword)
            {
                SwordAttack();
                return;
            }

            for (int i = 0; i < modifiersOnBeforeHit.Count; i++)
            {
                modifiersOnBeforeHit[i].ApplyEffect(this);
            }

            var totalAngle = Mathf.PI * 2;
            var projectileAmount = (weaponStats.projectileAmount - 1);
            var angleBetweenProjectiles = 10f;
            var middlePoint = angleBetweenProjectiles * projectileAmount;
            var halfMiddle = middlePoint / 2;
            var startingAngle = -halfMiddle;

            for (int i = 0; i < weaponStats.projectileAmount; i++)
            {
                if (weaponDataSo.WeaponType == WeaponType.Sword) break;

                var angleToAdd = startingAngle + i * angleBetweenProjectiles;

                if (weaponDataSo.WeaponType == WeaponType.Wand)
                    CreateMagicProjectile(angleToAdd, Random.Range(0, projectilePrefabsPerTier.Length));
                else if (weaponDataSo.WeaponType == WeaponType.Bow)
                    CreateMagicProjectile(angleToAdd,Random.Range(0, projectilePrefabsPerTier.Length));
            }

            if (weaponDataSo.WeaponType == WeaponType.Bow)
            {
                BowAttack();
            }
        }
        
        private void CreateMagicProjectile(float angleToRotate, int projectileIndex = 0)
        {
            //todo change this to pool and dictionary
            GameObject projectile = null;

            if (weaponStats.hasSphereProjectile)
                projectile = Instantiate(sphereProjectile);
            else if (weaponStats.explodingProjectile)
                projectile = Instantiate(explodingProjectile);
            else
            {
                var totalIndex = projectilePrefabsPerTier.Length; // 3
                var changeLevel = 5; // 7
                var index = weaponLevelSystem.weaponLevel/changeLevel;
                if (index >= totalIndex)
                    index = totalIndex - 1;
                
                projectile = Instantiate(projectilePrefabsPerTier[projectileTier]);
            }

            projectile.transform.position = projectilePoint.transform.position;
            
            projectile.transform.forward = DictionaryHolder.Enemies[targetEnemy].HitPoint.transform.position - projectile.transform.position;
            //projectile.transform.rotation = Quaternion.Euler(new Vector3(0, projectile.transform.eulerAngles.y, 0));
            projectile.transform.Rotate(new Vector3(0, 0, 1), angleToRotate);

            var sc = projectile.GetComponent<Projectile>();

            sc.bounceNum = weaponStats.hasSphereProjectile ? 0 : weaponStats.bounceNum;
            sc.pierceNum = weaponStats.hasSphereProjectile ? 999 : weaponStats.pierceNum;
            sc.criticalHitChance = weaponStats.criticalHitChance / 100f;
            sc.criticalHitDamage = weaponStats.criticalHitDamage;
            sc.weapon = this;
            sc.SetModifiers(modifiers);
            sc.SetMaxDistance(weaponStats.range);
            sc.SetHomingProjectile(weaponStats.hasHomingProjectiles, targetEnemy);
            sc.isRotating = weaponStats.hasRotatingProjectiles;
            sc.SetShooter(this);
            sc.isActive = true;

            if (weaponStats.explodingProjectile)
            {
                sc.SetExplodingProjectile(weaponStats.explosionDamageMultiplier);
            }
        }

        public void SetSwordSwinger(PlayerSwordSwinger pSwordSwinger)
        {
            return;
            swordSwinger = pSwordSwinger;
            swordSwinger.SetWeapon(this);
            swordSwinger.SetSlashPosition(projectilePoint);
            if (weaponDataSo.WeaponType == WeaponType.Sword)
                swordSwinger.ActivateEnemyFollow();
            else
                swordSwinger.DeActivateEnemyFollow();
        }

        public void SetEnemy(GameObject enemy, float distance)
        {
            if (enemy == null || !DictionaryHolder.Enemies[enemy].IsAvailable())
            {
                //swordSwinger.SetTarget(null);
                targetEnemy = null;
                return;
            }

            targetEnemy = enemy;

            distanceToEnemy = distance;
            EventManager.Instance.SetDistanceBetweenEnemy(distance * GameConfig.RangeToRadius);
            //swordSwinger.SetTarget(targetEnemy);
        }

        public void UpdateDamageHit(float damageHit)
        {
            weaponStats.totalDamageInThisTower += damageHit;
            weaponStats.totalDamageInThisFloor += damageHit;

            //todo save this value!!!!!
            weaponStats.totalDamageWithThisWeapon += damageHit;
        }

        public void UpdateKillCount()
        {
            weaponStats.totalKillInThisTower++;
            weaponStats.totalKillInThisFloor++;
            weaponStats.totalKillWithoutGotHit++;

            //todo save this value!
            weaponStats.totalKillWithThisWeapon++;
        }

        public void OnFloorStart()
        {
            weaponStats.SetStats();
            ActivateModifiers();
            ActivateRotatingWeapons();
            ResetKillDamageValuesForFloor();

            for (int i = 0; i < spells.Count; i++)
                spells[i].ActiavateSpell();
        }

        public void OnFloorEnds()
        {
            for (int i = 0; i < spells.Count; i++)
                spells[i].Deactivate();
        }

        private bool CanAttack()
        {
            return targetEnemy != null && distanceToEnemy <= weaponStats.range / GameConfig.RangeToRadius;
        }
        
        public float GetDamage()
        {
            return weaponStats.damage;
        }

        public float GetCriticalDamageChance()
        {
            return weaponStats.criticalHitChance;
        }

        private void RemoveModifiers(List<SpecialModifiers> list)
        {
            for (int i = 0; i < list.Count; i++)
                RemoveSpecialModifier(list[i]);
        }

        private void ResetWeapon()
        {
            RemoveModifiers(specialModifiersList);
            RemoveModifiers(weaponStats.specialModifiers);
            spells.Clear();
            
            if (statsFromTree != null)
                statsFromTree.Clear();
            
            weaponUpgradeTree.ResetTree();
        }

        private void ResetKillDamageValues()
        {
            weaponStats.totalDamageInThisFloor = 0;
            weaponStats.totalDamageInThisTower = 0;
            weaponStats.totalKillInThisTower = 0;
            weaponStats.totalKillInThisFloor = 0;
            weaponStats.totalKillWithoutGotHit = 0;
            weaponStats.totalAttackNumber = 0;
            weaponStats.totalAttackNumberWithoutGettingHit = 0;
        }

        private void ResetKillDamageValuesForFloor()
        {
            weaponStats.totalDamageInThisFloor = 0;
            weaponStats.totalKillInThisFloor = 0;
            weaponStats.totalAttackNumber = 0;
            weaponStats.totalAttackNumberWithoutGettingHit = 0;
        }

        private void InitialiseWeapon()
        {
            SetSpecialModifiers(specialModifiersList);
            SetSpecialModifiers(weaponStats.specialModifiers);
            ActivateModifiers();
            UpdateStyle();
            weaponUpgradeTree.ResetTree();
            ResetKillDamageValues();
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            ResetWeapon();
        }

        public void OnGet()
        {
            InitialiseWeapon();
            weaponStats._weaponDataSo = weaponDataSo;
            weaponStats.statsFromTree = statsFromTree;
            weaponStats.SetStats();
            weaponStats.CreateSpells();
            spells = weaponStats.spells;
        }
    }
}