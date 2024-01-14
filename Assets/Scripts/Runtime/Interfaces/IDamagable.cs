using Data;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Interfaces
{
    public interface IDamageable
    {
        Transform Transform { get; set; }
        void DealDamage(float damage, bool isCriticalHit);
        void AddElementalAilment(ElementModifiers element, float time, float effect, int spreadAmount = 0);
    }
}