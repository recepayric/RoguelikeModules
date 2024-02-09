using Runtime.Enums;

namespace Runtime.Modifiers.CharacterModifiers
{
    public class DealNoBurn : Modifier
    {
        public DealNoBurn()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            player.stats.SetStat(AllStats.DealNoBurn, 1);
        }
    }
}