using System;
using Runtime.EnemyRelated;

namespace Runtime.TowerRelated
{
    public class TowerStatApplier
    {
        public static void ApplyStatToEnemy(EnemyStats enemyStats, TowerModifier enemyModifier)
        {
            switch (enemyModifier.TowerMonsterModifiers)
            {
                case TowerMonsterModifiers.MonsterHealthIncrease:
                    enemyStats.currentMaxHealth *= (1 + enemyModifier.CurrentEffect / 100f);
                    break;
                case TowerMonsterModifiers.MonsterSpeedIncrease:
                    enemyStats.currentSpeed *= (1 + enemyModifier.CurrentEffect / 100f);
                    break;
                case TowerMonsterModifiers.MonsterDamageIncrease:
                    enemyStats.currentDamage *= (1 + enemyModifier.CurrentEffect / 100f);
                    break;
                case TowerMonsterModifiers.MonsterRangeIncrease:
                    enemyStats.currentAttackRange *= (1 + enemyModifier.CurrentEffect / 100f);
                    enemyStats.currentMaxAttackRange *= (1 + enemyModifier.CurrentEffect / 100f);
                    break;
                case TowerMonsterModifiers.MonsterDefenceIncrease:
                    enemyStats.currentDefence *= (1 + enemyModifier.CurrentEffect / 100f);
                    break;
                case TowerMonsterModifiers.MonsterEvasionIncrease:
                    enemyStats.currentEvasion *= (1 + enemyModifier.CurrentEffect / 100f);
                    break;
                case TowerMonsterModifiers.MonsterAttackSpeedIncrease:
                    var multiplier = (1 + enemyModifier.CurrentEffect / 100f);
                    if (multiplier == 0)
                        multiplier = 0.01f;
                    enemyStats.currentAttackSpeed /= multiplier;
                    break;
                case TowerMonsterModifiers.MonsterAlwaysDealCriticalHit:
                    enemyStats.alwaysDealCriticalHit = true;
                    break;
                case TowerMonsterModifiers.MonsterDontTakeCriticalHit:
                    enemyStats.isImmuneToCriticalHits = true;
                    break;
                case TowerMonsterModifiers.MonsterCriticalDamageIncrease:
                    enemyStats.criticalDamageIncrease += (1 + enemyModifier.CurrentEffect / 100f);
                    break;
                case TowerMonsterModifiers.MonsterHitsShock:
                    enemyStats.ElementalStatus.hitsShock = true;
                    break;
                case TowerMonsterModifiers.MonsterHitsFreeze:
                    enemyStats.ElementalStatus.hitsFreeze = true;
                    break;
                case TowerMonsterModifiers.MonsterHitsBurn:
                    enemyStats.ElementalStatus.hitsBurn = true;
                    break;
                case TowerMonsterModifiers.MonsterCantBeFrozen:
                    enemyStats.ElementalStatus.immuneToFreeze = true;
                    break;
                case TowerMonsterModifiers.MonsterCantBeBurn:
                    enemyStats.ElementalStatus.immuneToBurn = true;
                    break;
                case TowerMonsterModifiers.MonsterCantBeShocked:
                    enemyStats.ElementalStatus.immuneToShock = true;
                    break;
                case TowerMonsterModifiers.MonsterHealthRegen:
                    enemyStats.currentDefence += enemyStats.currentHealthRegen;
                    break;
                case TowerMonsterModifiers.MonsterCantTakeMagicalDamage:
                    enemyStats.isImmuneToMagicDamage = true;
                    break;
                case TowerMonsterModifiers.MonsterCantTakeMeleeDamage:
                    enemyStats.isImmuneToMeleeDamage = true;
                    break;
                case TowerMonsterModifiers.MonsterCantTakeRangedDamage:
                    enemyStats.isImmuneToRangedDamage = true;
                    break;
            }
        }
    }
}