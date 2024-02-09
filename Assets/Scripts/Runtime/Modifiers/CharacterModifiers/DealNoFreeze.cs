using Runtime.Enums;

namespace Runtime.Modifiers.CharacterModifiers
{
    public class DealNoFreeze : Modifier
    {
        
        
        public DealNoFreeze()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            player.stats.SetStat(AllStats.DealNoFreeze, 1);
        }
    }
}