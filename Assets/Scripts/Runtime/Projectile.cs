using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Interfaces;
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
        [Header("Properties")] public bool doesSpin;
        public float spinSpeed;
        public Vector3 spinAxis;

        [Header("General")] public GameObject projectileObject;

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

        public bool isStickingToEnemy = false;
        public bool doesExplode = false;
        public float explosionTimer;

        public TargetType targetType;
        public IShooter Shooter;
        public IDamageable Damagable;

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

            _renderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<CircleCollider2D>();

            if (_renderer == null)
                _renderer = projectileObject.GetComponent<SpriteRenderer>();
            if (_collider2D == null)
                _collider2D = projectileObject.GetComponent<CircleCollider2D>();

            _renderer.enabled = true;
            _collider2D.enabled = true;
        }

        private void Update()
        {
            if (isRotating)
                UpdateRotatingMove();
            else if (isHomingProjectile)
                UpdateHomingV2();
            else
                UpdateRegularMove();

            if (doesSpin)
                Spin();
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

        private void UpdateHoming()
        {
            var perc = moveTimer / timeToMove;
            var diff = targetEnemy.transform.position - initialPos;

            transform.right = targetEnemy.transform.position - initialPos;

            var top = transform.up * _projectileCurve.curveY.Evaluate(perc);
            var targetPoss = initialPos + diff * (_projectileCurve.curveX.Evaluate(perc)) + top * direction;

            //transform.right = targetPos.transform.position - initialPos.transform.position;
            transform.right = targetPoss - transform.position;
            transform.position = targetPoss;

            moveTimer += Time.deltaTime;
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
            distanceTraveled += Vector2.Distance(pTransform.position, pTransform.position + deltaTravel);
            pTransform.position += deltaTravel;
        }

        private void UpdateRegularMove()
        {
            deltaTravel = Time.deltaTime * projectileSpeed * pTransform.right;
            distanceTraveled += Vector2.Distance(pTransform.position, pTransform.position + deltaTravel);
            pTransform.position += deltaTravel;

            if (destroyAfterTravelingMax && distanceTraveled >= maxTravel)
            {
                //Debug.Log("Travelled: " + distanceTraveled + "    " + maxTravel);
                Destroy(gameObject);
            }
        }

        private void UpdateRotatingMove()
        {
            rotationAngle += Time.deltaTime * projectileSpeed * 10;
            if (rotationAngle >= 360f) rotationAngle -= 360f;
            var targetAngle = rotationAngle + Weapon.RotationGlobal;

            var posX = rotatingDistance * Mathf.Cos(Mathf.Deg2Rad * targetAngle);
            var posY = rotatingDistance * Mathf.Sin(Mathf.Deg2Rad * targetAngle);

            var currentPos = pTransform.position;
            var targetPos = ScriptDictionaryHolder.Player.transform.position + new Vector3(posX, posY, 0);

            pTransform.right = targetPos - currentPos;

            deltaTravel = Time.deltaTime * projectileSpeed * pTransform.right;
            distanceTraveled += Vector2.Distance(currentPos, currentPos + deltaTravel);
            currentPos += deltaTravel;
            pTransform.position = currentPos;

            if (destroyAfterTravelingMax && distanceTraveled >= maxTravel * 2)
            {
                //Debug.Log("Travelled: " + distanceTraveled + "    " + maxTravel);
                //Destroy(gameObject);
                if (isHomingProjectile)
                {
                    SelectRandomEnemy();
                    isHomingProjectile = true;
                    isRotating = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            rotationTimer -= Time.deltaTime;

            if (rotationTimer <= Random.Range(0, -1f))
            {
                //Debug.Log("Travelled: " + distanceTraveled + "    " + maxTravel);
                //Destroy(gameObject);
            }
        }

        private void SelectRandomEnemy()
        {
            var enemies = ScriptDictionaryHolder.Enemies.Keys.ToList();
            var rnd = Random.Range(0, enemies.Count);

            targetEnemy = enemies[rnd];
        }

        private void SelectClosestEnemy()
        {
        }

        private void StartDestroying()
        {
            projectileSpeed = 0;
            _renderer.enabled = false;
            _collider2D.enabled = false;
            DOVirtual.DelayedCall(0.25f, () => { Destroy(gameObject); });
        }

        public void HitTarget(IDamageable enemy)
        {
            if (enemy == ignoredEnemy)
                return;

            if (isHomingProjectile)
            {
                DOTween.Kill(gameObject.GetInstanceID() + "turn");
            }

            //damage = weapon.weaponStats.damage;
            damage = Shooter?.GetDamage() ?? 0;
            //ResetTravelData();
            var isCrit = Random.Range(0, 1f) <= criticalHitChance;

            enemy.DealDamage((int)damage, isCrit);

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

            if (bounceNum > 0)
            {
                //isHomingProjectile = false;
                StopTravelDestroy();
                ResetTravelData();
                bounceNum--;
                var newTarget = GetClosestEnemy(enemy.Transform.gameObject);
                if (newTarget == null)
                    Destroy(gameObject);
                else
                    transform.right = newTarget.transform.position - transform.position;
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
            Physics2D.OverlapCircleNonAlloc(transform.position, 500,
                collider2Ds); //layermask to filter the varius useless colliders

            foreach (var collidedEnemy in collider2Ds)
            {
                if (collidedEnemy == null) break;

                if (collidedEnemy.CompareTag("Enemy") && collidedEnemy.gameObject != currentEnemy)
                {
                    if(ScriptDictionaryHolder.Enemies[collidedEnemy.gameObject].IsAvailable())
                        return collidedEnemy.gameObject;
                }
            }


            return null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Projectile Hit!!");
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
                HitTarget(damageable);
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            ScriptDictionaryHolder.Projectiles.Remove(gameObject);
        }

        public void OnGet()
        {
            distanceTraveled = 0;
            ScriptDictionaryHolder.Projectiles.Add(gameObject, this);
        }
    }
}