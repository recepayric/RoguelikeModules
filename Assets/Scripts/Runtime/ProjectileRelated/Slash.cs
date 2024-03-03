using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Modifiers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.ProjectileRelated
{
    public class Slash : MonoBehaviour, IPoolObject
    {
        public Renderer renderer;
        private MaterialPropertyBlock _propertyBlock;
        public GameObject target;
        public GameObject objectToFollow;

        public bool isCreating = false;
        public bool isMoving = false;
        public float createTime;
        public float createAmount;
        public float slashSpeedMultiplier;
        private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
        private static readonly int IsReverse = Shader.PropertyToID("_IsReverse");

        private Vector3 deltaTravel;
        public float slashSpeed;
        public float slashDir;
        public float distanceTraveled;
        public float maxTravel;
        public Weapon weapon;

        #region Damage Related Variables

        private IDamageable _ignoredEnemy;
        private IShooter _shooter;
        private float _damage;
        private List<Modifier> _modifiers;

        #endregion

        private void Start()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            Create();
            Move();
        }

        public void HitTarget(IDamageable enemy)
        {
            if (enemy == _ignoredEnemy)
                return;

            _damage = _shooter?.GetDamage() ?? 0;
            //ResetTravelData();
            var isCrit = Random.Range(0, 1f) <= _shooter?.GetCriticalDamageChance();

            enemy.DealDamage((int)_damage, isCrit, 1);

            //Apply Ailments!
            //todo redo this!!!
            //todo move this to seperate class for all entities that can damage!!!
            if (weapon != null && weapon.weaponStats.addBurn)
                enemy.AddElementalAilment(ElementModifiers.Fire, weapon.weaponStats.burnTime,
                    weapon.weaponStats.burnDamage, weapon.weaponStats.burnSpreadAmount);

            if (weapon != null && weapon.weaponStats.addFreeze)
                enemy.AddElementalAilment(ElementModifiers.Ice, weapon.weaponStats.freezeTime,
                    weapon.weaponStats.freezeEffect);

            if (weapon != null && weapon.weaponStats.addShock)
                enemy.AddElementalAilment(ElementModifiers.Lightning, weapon.weaponStats.shockTime,
                    weapon.weaponStats.shockEffect);

            if (weapon != null && weapon.weaponStats.addBleed)
                enemy.AddElementalAilment(ElementModifiers.Bleed, weapon.weaponStats.bleedTime,
                    weapon.weaponStats.bleedDamage);

            if (weapon != null && weapon.weaponStats.addStun)
            {
                var doesStun = Random.Range(0, 100f) <= weapon.weaponStats.stunChance;
                if (doesStun)
                    enemy.AddElementalAilment(ElementModifiers.Stun, weapon.weaponStats.stunTime,
                        1f);
            }
            
            if (_modifiers != null)
            {
                foreach (var modifier in _modifiers)
                {
                    modifier.ApplyEffect(gameObject, this, isCrit);
                }
            }
        }

        public void SetRange(float range)
        {
            maxTravel = range / GameConfig.RangeToRadius;
        }

        private void Move()
        {
            if (!isMoving) return;
            deltaTravel = Time.deltaTime * (slashSpeed * slashSpeedMultiplier * slashDir) * transform.right;
            distanceTraveled += Vector2.Distance(transform.position, transform.position + deltaTravel);
            transform.position += deltaTravel;

            if (distanceTraveled >= maxTravel)
            {
                //Debug.Log("Travelled: " + distanceTraveled + "    " + maxTravel);
                //Destroy(gameObject);
                _propertyBlock.SetFloat(FillAmount, 0);
                renderer.SetPropertyBlock(_propertyBlock);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
                HitTarget(damageable);
        }

        public void SetModifiers(List<Modifier> modifiers)
        {
            _modifiers = modifiers;
        }

        public void SetShooter(IShooter shooter)
        {
            _shooter = shooter;
        }

        private void Create()
        {
            if (!isCreating) return;

            createAmount += Time.deltaTime / createTime;
            if (createAmount > 1)
            {
                createAmount = 1;
                isCreating = false;
                isMoving = true;
                distanceTraveled = 0;
                transform.SetParent(null);
            }

            transform.position = objectToFollow.transform.position;
            var isReverse = 0;
            
            if (target != null)
            {
                isReverse = target.transform.position.x > transform.position.x ? 1 : 0;
                transform.right = target.transform.position - transform.position;
            }
            
            _propertyBlock.SetInteger(IsReverse, isReverse);

            _propertyBlock.SetFloat(FillAmount, createAmount);
            renderer.SetPropertyBlock(_propertyBlock);
        }

        public void SetPosition(Vector3 position)
        {
            isMoving = false;
            transform.position = position;
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }

        [Button]
        public void SetAngle(GameObject target)
        {
            //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public void StartCreating(float pCreateTime)
        {
            createTime = pCreateTime;
            createAmount = 0;
            isCreating = true;
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
        }

        public void OnGet()
        {
        }
    }
}