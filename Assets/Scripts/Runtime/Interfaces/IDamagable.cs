using Data;
using UnityEngine;

namespace Runtime.Interfaces
{
    public interface IDamageable
    {
        Transform Transform { get; set; }
        void DealDamage(float damage, bool isCriticalHit);
    }
}