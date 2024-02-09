using Runtime.Enums;

namespace Runtime.Modifiers.CharacterModifiers
{
    public class HealthModifierReduction : Modifier
    {
        public HealthModifierReduction()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            player.stats.SetStat(AllStats.HealthMultiplier, 0.5f);
            player.CalculateBaseStats();
            player.CalculateStats();
        }
    }
}