using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Interfaces;
using Runtime.Modifiers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.ProjectileRelated
{
    public class Slash : MonoBehaviour
    {
        public Renderer renderer;
        public MaterialPropertyBlock PropertyBlock;
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

        #region Damage Related Variables

        private IDamageable _ignoredEnemy;
        private IShooter _shooter;
        private float _damage;
        private List<Modifier> _modifiers;

        #endregion

        private void Start()
        {
            PropertyBlock = new MaterialPropertyBlock();
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

            enemy.DealDamage((int)_damage, isCrit);

            //Apply Ailments!
            //todo redo this!!!
            //
            // if (weapon.weaponStats.addBurn)
            //     enemy.AddBurning(weapon.weaponStats.burnTime, weapon.weaponStats.burnDamage);
            //
            // if (weapon.weaponStats.addFreeze)
            //     enemy.AddFreeze(weapon.weaponStats.freezeTime, weapon.weaponStats.freezeEffect);
            //
            // if (weapon.weaponStats.addShock)
            //     enemy.AddShock(weapon.weaponStats.shockTime, weapon.weaponStats.shockEffect);

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
            maxTravel = range/GameConfig.RangeToRadius;
        }
        
        private void Move()
        {
            if (!isMoving) return;
            deltaTravel = Time.deltaTime * (slashSpeed*slashSpeedMultiplier*slashDir) * transform.right;
            distanceTraveled += Vector2.Distance(transform.position, transform.position + deltaTravel);
            transform.position += deltaTravel;

            if (distanceTraveled >= maxTravel)
            {
                //Debug.Log("Travelled: " + distanceTraveled + "    " + maxTravel);
                //Destroy(gameObject);
                PropertyBlock.SetFloat(FillAmount, 0);
                renderer.SetPropertyBlock(PropertyBlock);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
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
            if(target != null)
                transform.right = target.transform.position - transform.position;

            if (target.transform.position.x > transform.position.x)
                PropertyBlock.SetInteger(IsReverse, 1);
            else
                PropertyBlock.SetInteger(IsReverse, 0);
            
            PropertyBlock.SetFloat(FillAmount, createAmount);
            renderer.SetPropertyBlock(PropertyBlock);
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
    }
}