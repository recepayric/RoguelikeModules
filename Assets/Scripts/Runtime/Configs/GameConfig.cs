using Runtime.UIRelated;

namespace Runtime.Configs
{
    public class GameConfig
    {
        public static Screens FirstScreenToOpen = Screens.MapSelect;
        public static float RangeToRadius = 20;
        public static float MinRangeForExplosions = 5;

        public static float ResourceDropChance = 1;

        public static int FloorDuration = 60;

        public static float FireSpreadDistance = 5f;
    }
}