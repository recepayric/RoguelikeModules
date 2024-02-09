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
        None,
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
        
        //Ailment
        BleedDamageIncrease,
        BleedDamageMultiplier,
        BleedDurationMultiplier,
        
        MaxFreezeStack,
        MaxBurnStack,
        MaxShockStack,
        MaxBleedStack,
        MaxStunStack,
        
        ExpGainMultiplier,
        BurnSpreadAmount,
        DealNoBurn,
        DealNoFreeze,
        DealNoShock,
        HealthMultiplier,
    }

    public enum ElementModifiers
    {
        Fire,
        Ice,
        Lightning,
        Bleed,
        Stun,
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