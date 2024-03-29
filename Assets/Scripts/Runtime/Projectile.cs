using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Managers;
using Runtime.Modifiers;
using Runtime.ProjectileRelated;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class Projectile : MonoBehaviour, IPoolObject
    {
        public Weapon weapon;
        public Sounds hitSound;
        public Sounds pierceSound;
        [Header("Properties")] public bool doesSpin;
        public float spinSpeed;
        public Vector3 spinAxis;

        [Header("General")] public GameObject projectileObject;

        //todo change it into pool!
        [Header("General")] public GameObject hitParticleEffect;

        public bool destroyAfterTravelingMax = true;
        public bool followTarget = false;

        public int bounceNum;
        public int pierceNum;

        public float criticalHitChance;
        public float criticalHitDamage;

        public float projectileSpeed = 3;
        public Transform pTransform;
        public float distanceTraveled = 0;
        public Vector3 deltaTravel;

        public float maxTravel = 200;

        public Collider2D[] collider2Ds;

        private List<Modifier> modifiers;
        public List<GameObject> enemiesWasHit;
        private GameObject lastHitEnemy;

        public bool isActive = false;
        public bool isStickingToEnemy = false;
        public bool doesExplode = false;
        public float explosionDamageMultiplier;
        public PoolKeys explosionPoolKey;

        public TargetType targetType;
        public IShooter Shooter;
        public IDamageable Damagable;

        private int _piercedEnemyCount = 0;

        //Specs - Homing
        [Header("Homing")] public bool isHomingProjectile = false;
        public GameObject targetEnemy;
        public float timeToMove;
        public float moveTimer;
        public Vector3 initialPos;
        public int direction;
        public float homingTurnSpeed;

        //Split
        public bool split;
        public int splitAmount;
        public IDamageable ignoredEnemy;
        public PoolKeys splitProjectileKey;

        private ProjectileCurve _projectileCurve;

        //Components
        private SpriteRenderer _renderer;
        private CircleCollider2D _collider2D;

        //Damage Related
        [Header("Damages")] public float damage;

        //RotatingData
        public bool isRotating;
        public float rotatingDistance;
        public float rotationAngle;
        public float rotationTimer;

        private void Start()
        {
            collider2Ds = new Collider2D[100];
            distanceTraveled = 0;
            //maxTravel = 200f / GameConfig.RangeToRadius;
            pTransform = transform;
            //Debug.Log("Projectile created!");
        }

        private void Update()
        {
            if (!isActive) return;


            if (isHomingProjectile)
                UpdateHomingV3();
            
            
            UpdateRegularMove();

            if (doesSpin)
                Spin();
        }

        public void SetExplodingProjectile(float explodingDamageMultiplier)
        {
            doesExplode = true;
            explosionDamageMultiplier = explodingDamageMultiplier;
        }

        public void SetShooter(IShooter pShooter)
        {
            Shooter = pShooter;
        }

        public void SetMaxDistance(float weaponDistance)
        {
            maxTravel = weaponDistance / GameConfig.RangeToRadius;
        }

        public void SetModifiers(List<Modifier> pModifiers)
        {
            modifiers = pModifiers;

            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].ApplyEffect(this);
            }
        }

        public void SetHomingProjectile(bool isHoming, GameObject targetEnemy)
        {
            this.targetEnemy = targetEnemy;
            isHomingProjectile = isHoming;

            if (isHoming)
            {
                _projectileCurve = GetComponent<ProjectileCurve>();
                _projectileCurve.RandomiseCurve();
            }
            else
            {
                return;
            }

            var distance = Vector3.Distance(transform.position, this.targetEnemy.transform.position);
            timeToMove = distance / projectileSpeed;
            moveTimer = 0;
            initialPos = transform.position;
            direction = Random.Range(0f, 1f) < 0.5f ? 1 : -1;

            //Add Random Angle
            var angleToAdd = Random.Range(-120, 120);
            if (Random.Range(0, 1f) < 0.5f)
                angleToAdd = angleToAdd;

            transform.Rotate(new Vector3(0, 0, 1), angleToAdd);

            var initialRight = transform.right;
            var initialPosition = transform.position;

            var targetRight = targetEnemy.transform.position - initialPosition;

            var difference = targetRight - initialRight;

            var timeToTurn = 1f;
            var deltaTime = Time.fixedDeltaTime;
            var steps = (int)(timeToTurn / deltaTime);
            var timer = 0f;

            var deltaStepTime = timeToTurn / steps;
        }

        private void ResetTravelData()
        {
            distanceTraveled = 0;
        }

        private void StopTravelDestroy()
        {
            destroyAfterTravelingMax = false;
        }

        private void StartTravelDestroy()
        {
            ResetTravelData();
            destroyAfterTravelingMax = true;
        }

        private void Spin()
        {
            projectileObject.transform.Rotate(spinAxis, Time.deltaTime * spinSpeed);
        }

        private float turnMultiplier = 0f;
        private float turnTime = 1f;
        private float turningTimer;
        private Vector3 initialRotation;

        private void UpdateHomingV3()
        {
            if (targetEnemy == null)
            {
                isHomingProjectile = false;
                return;
            }

            if (!DictionaryHolder.Enemies.ContainsKey(targetEnemy))
            {
                isHomingProjectile = false;
                return;
            }
            
            turningTimer += Time.deltaTime/turnTime*turnMultiplier;

            turnMultiplier += Time.deltaTime/0.125f;
            
            if (turningTimer > 1)
                turningTimer = 1;
            
            var targetRotation = DictionaryHolder.Enemies[targetEnemy].HitPoint.transform.position - transform.position;

            var angle = Vector3.Lerp(initialRotation, targetRotation, turningTimer);
            transform.forward = angle;
        }

        private void UpdateHomingV2()
        {
            if (!targetEnemy.activeSelf)
            {
                isHomingProjectile = false;
                return;
            }

            var targetRight = targetEnemy.transform.position - transform.position;

            transform.right = Vector3.Lerp(transform.right, targetRight, Time.deltaTime * homingTurnSpeed);

            deltaTravel = Time.deltaTime * projectileSpeed * pTransform.right;
            distanceTraveled += Vector3.Distance(pTransform.position, pTransform.position + deltaTravel);
            pTransform.position += deltaTravel;
        }

        private void UpdateRegularMove()
        {
            deltaTravel = Time.deltaTime * projectileSpeed * pTransform.forward;
            distanceTraveled += Vector3.Distance(pTransform.position, pTransform.position + deltaTravel);
            pTransform.position += deltaTravel;

            if (transform.position.y > 2.5f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
            }

            if (destroyAfterTravelingMax && distanceTraveled >= maxTravel)
            {
                Debug.Log("Destroying because of distance traveled");
                Destroy(gameObject);
            }
        }

        private void SelectRandomEnemy()
        {
            var enemies = DictionaryHolder.Enemies.Keys.ToList();
            var rnd = Random.Range(0, enemies.Count);

            targetEnemy = enemies[rnd];
        }

        private void SelectClosestEnemy()
        {
        }

        private void StartDestroying()
        {
            projectileSpeed = 0;
            //_renderer.enabled = false;
            //_collider2D.enabled = false;
            //DOVirtual.DelayedCall(0.25f, () => { Destroy(gameObject); });
            Debug.Log("Destroying because of hit");
            Destroy(gameObject);
        }

        public void Explode()
        {
            if (!doesExplode) return;
            Debug.Log("Expliding!!!!");
            var explosion = BasicPool.instance.Get(explosionPoolKey);
            explosion.transform.position = transform.position;

            var expSc = DictionaryHolder.Explosions[explosion];
            expSc.SetSize(10);
            expSc.SetDamage(damage * explosionDamageMultiplier);
            expSc.Explode();
        }

        public void HitTarget(IDamageable enemy)
        {
            if (!isActive) return;

            if (enemy == ignoredEnemy)
                return;


            if (isHomingProjectile)
            {
                Explode();
                DOTween.Kill(gameObject.GetInstanceID() + "turn");
            }

            //damage = weapon.weaponStats.damage;
            damage = Shooter?.GetDamage() ?? 0;
            var damageReduction = 0f;

            if (weapon != null)
                damageReduction = damage * (weapon.weaponStats.damageReductionOnPierce * _piercedEnemyCount) / 100f;

            damage -= damageReduction;
            damage = damage < 1 ? 1 : damage;
            EventManager.Instance.PlaySoundOnce(hitSound, 1);

            if (hitParticleEffect != null)
            {
                var hit = Instantiate(hitParticleEffect);
                hit.transform.position = transform.position;
                hit.transform.rotation = transform.rotation;
            }

            //ResetTravelData();
            var isCrit = Random.Range(0, 1f) <= criticalHitChance;
            enemy.DealDamage((int)damage, isCrit, weapon, 1);
            
            _piercedEnemyCount++;
            
            //Apply Ailments!
            //todo redo this!!!
            //
            if (weapon != null && weapon.weaponStats.addBurn)
                enemy.AddElementalAilment(ElementModifiers.Fire, weapon.weaponStats.burnTime,
                    weapon.weaponStats.burnDamage, weapon.weaponStats.burnSpreadAmount);

            if (weapon != null && weapon.weaponStats.addFreeze)
                enemy.AddElementalAilment(ElementModifiers.Ice, weapon.weaponStats.freezeTime,
                    weapon.weaponStats.freezeEffect);

            if (weapon != null && weapon.weaponStats.addShock)
                enemy.AddElementalAilment(ElementModifiers.Lightning, weapon.weaponStats.shockTime,
                    weapon.weaponStats.shockEffect);

            if (modifiers == null)
                modifiers = new List<Modifier>();

            foreach (var modifier in modifiers)
            {
                modifier.ApplyEffect(gameObject, this, isCrit);
            }

            if (split)
            {
                CommandController.instance.SplitProjectile(enemy.Transform.position, splitAmount, splitProjectileKey,
                    enemy);
                //Destroy(gameObject);
            }

            if (doesExplode)
            {
                Explode();
                StartDestroying();
            }
            else if (bounceNum > 0)
            {
                Debug.Log("Bouncing!!");
                //isHomingProjectile = false;
                StopTravelDestroy();
                ResetTravelData();
                bounceNum--;
                isHomingProjectile = true;
                var newTarget = GetClosestEnemy(enemy.Transform.gameObject);
                if (newTarget == null)
                {
                    Destroy(gameObject);
                    Debug.Log("No target to bounce. Destroying");
                }
                else
                {
                    Debug.Log("Old target: " + targetEnemy + "  new: " + newTarget);
                    targetEnemy = newTarget;
                    transform.forward = DictionaryHolder.Enemies[newTarget].HitPoint.transform.position -
                                        transform.position;
                    initialRotation = transform.forward;
                }
                //Debug.LogError("Stop!!!");
            }
            else if (pierceNum > 0)
            {
                //isHomingProjectile = false;
                pierceNum--;
                StartTravelDestroy();
            }
            else
            {
                //Destroy(gameObject);
                StartDestroying();
            }
        }

        private GameObject GetClosestEnemy(GameObject currentEnemy)
        {
            Debug.Log("Current Target: " + targetEnemy);
            //todo do this in one loop!!!
            var closestEnemy = (GameObject)null;
            var closestDistance = 99999f;
            bool isInIgnoreList = false;
            foreach (var enemy in DictionaryHolder.Enemies)
            {
                Debug.Log(enemy.Key + "  " + lastHitEnemy);
                if (enemy.Key == lastHitEnemy)
                {
                    Debug.Log("Current enemy is this enemy!!!");
                    continue;
                }

                //Check for ignore list
                for (int i = 0; i < enemiesWasHit.Count; i++)
                {
                    if (enemy.Key == enemiesWasHit[i])
                    {
                        isInIgnoreList = true;
                        break;
                    }
                }

                if (isInIgnoreList)
                    continue;

                var distance = Vector3.Distance(gameObject.transform.position, enemy.Key.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.Key;
                }
            }
            Debug.Log("New enemy: " + closestEnemy);

            return closestEnemy;
        }

        private void OnTriggerEnter(Collider other)
        {
            var hitBefore = false;
            
            for (int i = 0; i < enemiesWasHit.Count; i++)
            {
                if (enemiesWasHit[i] == other.gameObject)
                    return;
            }

            var damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                lastHitEnemy = other.gameObject;
                HitTarget(damageable);
                enemiesWasHit.Add(other.gameObject);
            }
        }


        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            DictionaryHolder.Projectiles.Remove(gameObject);
        }

        public void OnGet()
        {
            distanceTraveled = 0;
            DictionaryHolder.Projectiles.Add(gameObject, this);
        }
    }
}