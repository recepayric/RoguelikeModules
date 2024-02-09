using System;
using Random = UnityEngine.Random;

namespace Runtime.TowerRelated
{
    [Serializable]
    public class TowerModifier
    {
        public TowerMonsterModifiers TowerMonsterModifiers;
        public TowerPlayerModifiers TowerPlayerModifiers;

        public string TextTemplate;
        public string TextToShow;
        public float MinEffect;
        public float MaxEffect;
        public int CurrentEffect;
        
        public TowerModifier(TowerMonsterModifiers towerMonsterModifiers, float min, float max)
        {
            TowerMonsterModifiers = towerMonsterModifiers;
            MinEffect = min;
            MaxEffect = max;
        }
        
        public TowerModifier(TowerPlayerModifiers towerPlayerModifiers, float min, float max)
        {
            TowerPlayerModifiers = towerPlayerModifiers;
            MinEffect = min;
            MaxEffect = max;
        }

        public TowerModifier SetText(string text)
        {
            TextTemplate = text;
            return this;
        }

        public void RandomizeValue(int tier)
        {
            float value = Random.Range(MinEffect, MaxEffect) * ( 1 + 0.1f*(tier - 1));
            CurrentEffect = (int)value;
            TextToShow = TextTemplate.Replace("[x]", CurrentEffect.ToString());
        }

        public void ApplyModifier(Enemy enemy)
        {
            switch (TowerMonsterModifiers)
            {
                case TowerMonsterModifiers.MonsterHealthIncrease:
                    break;
                case TowerMonsterModifiers.MonsterSpeedIncrease:
                    break;
                case TowerMonsterModifiers.MonsterDamageIncrease:
                    break;
                case TowerMonsterModifiers.MonsterRangeIncrease:
                    break;
                case TowerMonsterModifiers.MonsterDefenceIncrease:
                    break;
                case TowerMonsterModifiers.MonsterAttackSpeedIncrease:
                    break;
                case TowerMonsterModifiers.MonsterAlwaysDealCriticalHit:
                    break;
                case TowerMonsterModifiers.MonsterDontTakeCriticalHit:
                    break;
                case TowerMonsterModifiers.MonsterCriticalDamageIncrease:
                    break;
                case TowerMonsterModifiers.MonsterHitsShock:
                    break;
                case TowerMonsterModifiers.MonsterHitsFreeze:
                    break;
                case TowerMonsterModifiers.MonsterHitsBurn:
                    break;
                case TowerMonsterModifiers.MonsterCantBeFrozen:
                    break;
                case TowerMonsterModifiers.MonsterCantBeBurn:
                    break;
                case TowerMonsterModifiers.MonsterCantBeShocked:
                    break;
                case TowerMonsterModifiers.MonsterHealthRegen:
                    break;
                case TowerMonsterModifiers.MonsterCantTakeMagicalDamage:
                    break;
                case TowerMonsterModifiers.MonsterCantTakeMeleeDamage:
                    break;
                case TowerMonsterModifiers.MonsterCantTakeRangedDamage:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}