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
using Runtime.WeaponRelated;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(WeaponLevelSystem))]
    [RequireComponent(typeof(WeaponUpgradeTree))]
    public class Weapon : MonoBehaviour, IShooter, IPoolObject
    {
        public WeaponLevelSystem weaponLevelSystem;
        public WeaponUpgradeTree weaponUpgradeTree;

        //public WeaponDatasSo weaponDatsSo;
        public WeaponDataSo weaponDataSo;
        public WeaponStats weaponStats;
        public PlayerSwordSwinger swordSwinger;
        public IWeaponCarrier WeaponCarrier;
        private float _timer;
        private float distanceToEnemy;

        //todo change to poolkey
        public GameObject projectilePrefab;
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
        public List<GameObject> rotatingSwords;

        public static float RotationGlobal;

        public SwirlObject swirlObject;

        public bool isAttacking = false;

        public List<GameObject> styles;
        public int styleCount;

        private void Start()
        {
            //todo move these to get from pool!!
            weaponStats.SetStats();
            //SetSpecialModifiers();
            SetSpecialModifiers(specialModifiersList);
            SetSpecialModifiers(weaponStats.specialModifiers);
            ActivateModifiers();
            UpdateStyle();
        }

        private void Update()
        {
            if (rotatingWeaponParent != null)
            {
                RotationGlobal += Time.deltaTime * (360f/weaponStats.attackSpeed);
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
                projectile.transform.rotation = Quaternion.Euler(0, 0, angleBetween*i+90);

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

        public void SetSlash(Slash slash)
        {
            slash.slashSpeedMultiplier = 1f / weaponStats.attackSpeed;
            slash.SetRange(weaponStats.range);
            slash.SetShooter(this);
            slash.weapon = this;
        }

        public void AddStyle(int number)
        {
            Debug.Log("Don't forget to change your weapon's style!!!");
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

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void CreateProjectile()
        {
            if (targetEnemy == null || !targetEnemy.activeSelf || !DictionaryHolder.Enemies[targetEnemy].IsAvailable())
                return;

            if (weaponDataSo.WeaponType == WeaponType.Sword)
            {
                SwordAttack();
                return;
            }

            //todo when multiple projectiles, send them with angle
            for (int i = 0; i < weaponStats.projectileAmount; i++)
            {
                if (weaponDataSo.WeaponType == WeaponType.Sword) break;

                //todo change this to pool and dictionary.
                var projectile = Instantiate(projectilePrefab);
                projectile.transform.position = projectilePoint.transform.position;
                projectile.transform.right = targetEnemy.transform.position - projectile.transform.position;

                var sc = projectile.GetComponent<Projectile>();

                sc.bounceNum = weaponStats.bounceNum;
                sc.pierceNum = weaponStats.pierceNum;
                sc.criticalHitChance = weaponStats.criticalHitChance / 100f;
                sc.criticalHitDamage = weaponStats.criticalHitDamage;
                sc.weapon = this;
                sc.SetModifiers(modifiers);
                sc.SetMaxDistance(weaponStats.range);
                sc.SetHomingProjectile(weaponStats.hasHomingProjectiles, targetEnemy);
                sc.isRotating = weaponStats.hasRotatingProjectiles;
                sc.SetShooter(this);
            }
        }

        public void SetSwordSwinger(PlayerSwordSwinger pSwordSwinger)
        {
            swordSwinger = pSwordSwinger;
            swordSwinger.SetWeapon(this);
            swordSwinger.SetSlashPosition(projectilePoint);
            if (weaponDataSo.WeaponType == WeaponType.Sword)
                swordSwinger.ActivateEnemyFollow();
            else
                swordSwinger.DeActivateEnemyFollow();
        }

        public void SetEnemy(GameObject enemy)
        {
            if (enemy == null || !DictionaryHolder.Enemies[enemy].IsAvailable())
            {
                targetEnemy = null;
                swordSwinger.SetTarget(null);
                return;
            }

            targetEnemy = enemy;
            swordSwinger.SetTarget(targetEnemy);
        }

        public void SetEnemy(GameObject enemy, float distance)
        {
            if (enemy == null || !DictionaryHolder.Enemies[enemy].IsAvailable())
            {
                swordSwinger.SetTarget(null);
                targetEnemy = null;
                return;
            }

            targetEnemy = enemy;

            distanceToEnemy = distance;
            EventManager.Instance.SetDistanceBetweenEnemy(distance * GameConfig.RangeToRadius);
            swordSwinger.SetTarget(targetEnemy);
        }

        public void OnFloorStart()
        {
            // weaponStats._weaponDataSo = weaponDataSo;
            // weaponStats.statsFromTree = statsFromTree;
            weaponStats.SetStats();
            ActivateModifiers();
            ActivateRotatingWeapons();
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

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
        }

        public void OnGet()
        {
            weaponStats._weaponDataSo = weaponDataSo;
            weaponStats.statsFromTree = statsFromTree;
            weaponStats.SetStats();
            //ActivateModifiers();
            //ActivateRotatingWeapons();
        }
    }
}