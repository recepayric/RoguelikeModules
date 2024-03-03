using System;
using System.Collections.Generic;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.SpellsRelated;
using Runtime.SpellsRelated.SpellsBoss;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.EnemyRelated
{
    public class EnemyBossBehaviour : MonoBehaviour
    {
        public Enemy enemy;
        public List<SpellV2> spells;
        public List<BossSpells> spellsToAdd;
        public List<BossAttackPatterns> bossAttackPatterns;
        private EnemyStats _enemyStats;
        public Bosses bossNumber;
        public float attackTimer;
        public bool canAttack = false;
        public GameObject projectilePrefab;
        
        
        public void SetUp(Enemy pEnemy)
        {
            enemy = pEnemy;
            _enemyStats = enemy._stats;
            canAttack = true;
            CreateSpells();
            attackTimer = 1;
        }

        public void Stop()
        {
            canAttack = false;
            spells.Clear();
        }

        private void CreateSpells()
        {
            for (int i = 0; i < spellsToAdd.Count; i++)
            {
                switch (spellsToAdd[i])
                {
                    case BossSpells.SpiralExplosions:
                        var spellSpiral = new ExplosionSpiral();
                        spellSpiral.source = gameObject;
                        spells.Add(spellSpiral);
                        break;
                    case BossSpells.ExplosionUnderPlayer:
                        break;
                    case BossSpells.RandomExplosionsAroundPlayer:
                        var spell = new ExplosionRandomAroundPlayer();
                        spells.Add(spell);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void FixedUpdate()
        {
            if (!canAttack) return;
            
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0)
            {
                Debug.Log("Boss is attacking!");
                Attack();
                attackTimer = 1;
            }
        }

        private void Attack()
        {
            var attackType = Random.Range(0, 2);
            
            if(attackType == 0)
                CastSpell();
            else if(attackType == 1)
                CastProjectile();
        }

        private void CastSpell()
        {
            var randSpellIndex = Random.Range(0, spells.Count);
            spells[randSpellIndex].CastSpell();
        }

        private void CastProjectile()
        {
            var randAttackIndex = Random.Range(0, bossAttackPatterns.Count);
            var pattern = bossAttackPatterns[randAttackIndex];
            
            if(pattern == BossAttackPatterns.CircleProjectile)
                AttackCircle();
        }

        private void AttackCircle()
        {
            var projectileCount = 10;
            var minRadius = 1;
            var angleBetween = Mathf.PI * 2 / projectileCount;

            for (int i = 0; i < projectileCount; i++)
            {
                var angle = angleBetween * i;
                var projectile = Instantiate(projectilePrefab);

                var x = minRadius * Mathf.Cos(angle);
                var y = minRadius * Mathf.Sin(angle);

                var pos = transform.position + new Vector3(x, y);
                var posNormalised = pos.normalized;
                projectile.transform.position = pos;
                projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle*Mathf.Rad2Deg));
                //projectile.transform.right = posNormalised - projectile.transform.position;
                
                var sc = projectile.GetComponent<Projectile>();
                sc.pierceNum = 0;
                sc.criticalHitChance = _enemyStats.currentCriticalHitChance / 100f;
                sc.criticalHitDamage = _enemyStats.criticalDamageIncrease;
                //sc.SetModifiers(modifiers);
                sc.SetMaxDistance(_enemyStats.currentAttackRange * GameConfig.RangeToRadius * 2*5);
                sc.SetShooter(enemy);
                sc.isActive = true;
            }
            
        }
    }
    
}