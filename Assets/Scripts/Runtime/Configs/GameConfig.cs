using Runtime.Enums;
using Runtime.UIRelated;

namespace Runtime.Configs
{
    public class GameConfig
    {
        public static Screens FirstScreenToOpen = Screens.CharacterSelect;
        public static float RangeToRadius = 20;
        public static float MinRangeForExplosions = 5;

        public static float ResourceDropChance = 1;

        public static int FloorDuration = 60;

        public static float FireSpreadDistance = 5f;

        public static AllStats[] randomCurses = new[]
        {
            AllStats.AttackSpeed, AllStats.MoveSpeed, AllStats.Damage
        };
    }
}