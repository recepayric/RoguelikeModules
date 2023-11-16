namespace Runtime.Enums
{

    public enum ItemRarity
    {
        Normal,
        Magic,
        Rare,
        Epic,
        Unique
    }
    
    public enum WeaponTypes
    {
        Ranged, Melee
    }

    public enum Weapons
    {
        Gun,
        Sword
    }
    
    public enum AllStats
    {
        Strength,
        Dexterity,
        Intelligence,
        Magic,
        MaxHealth,
        HealthRegen,
        MeleeAttack,
        RangedAttack,
        MagicalAttack,
        Damage,
        Range,
        ProjectileNumber,
        CollectRange,
        MoveSpeed,
        AttackSpeed
    }

    public enum SpecialModifiers
    {
        BounceOnCriticalStrike,
        SplitOnHit,
    }

    public enum WeaponAllStats
    {
        Range,
        ProjectileNumber,
        PierceNumber,
        PierceDamagePercent,
        BounceNumber,
        BounceChance,
    }
}