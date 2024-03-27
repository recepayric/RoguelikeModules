using Data;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Interfaces
{
    public interface IDamageable
    {
        Transform Transform { get; set; }
        GameObject HitPoint { get;}
        void DealDamage(float damage, bool isCriticalHit, Weapon weapon, float knockbackAmount = 0);
        void DealDamage(float damage, bool isCriticalHit, float knockbackAmount = 0);
        void AddElementalAilment(ElementModifiers ailment, float time, float effect, int spreadAmount = 0);
    }
}