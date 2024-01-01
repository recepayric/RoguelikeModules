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
        Ranged, Melee, Magic, Trap
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
        AttackSpeed,
        PierceNumber,
        BounceNumber,
        PierceDamage,
        
        //Elemental
        BurnDamagePercentage,
        BurnDamage,
        ChanceToBurn,
        
        FreezingEffect,
        ChanceToFreeze,
        
        ShockEffect,
        ChanceToShock,
        Thorn,
        
        ExpGainMultiplier
    }

    public enum SpecialModifiers
    {
        BounceOnCriticalStrike,
        SplitOnHit,
        RotatingProjectiles,
        HomingProjectiles,
        
        //Elemental
        BurnOnHit,
        FreezeOnHit,
        ShockOnHit,
        
        //Damage
        LowHealthMoreAttackSpeed
    }

    public enum ElementModifiers
    {
        Fire,
        Ice,
        Lightning,
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