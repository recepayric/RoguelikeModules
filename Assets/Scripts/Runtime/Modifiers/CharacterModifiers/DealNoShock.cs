using Runtime.Enums;

namespace Runtime.Modifiers.CharacterModifiers
{
    public class DealNoShock : Modifier
    {
        public DealNoShock()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            player.stats.SetStat(AllStats.DealNoShock, 1);
        }
    }
}