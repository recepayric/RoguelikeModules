namespace Runtime.Configs
{
    public class MapConfig
    {
        public static readonly int[] ModifiersPerTier = new[]
            { 2, 2, 4, 4, 6, 6, 8, 8, 10, 10, 12, 12, 14, 14, 16, 16, 18, 18, 20, 20 };

        public static readonly float ModifyToDropRateMultiplier = 0.25f;
        public static readonly float ModifyToExperienceRateMultiplier = 0.5f;
    }
}