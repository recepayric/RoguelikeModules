using Runtime.Enums;
using UnityEngine;

namespace Runtime.Interfaces
{
    public interface ITargetable
    {
        void Targeted();
        bool CanBeTargeted();
        void ChangeTargetableStatus(bool status);
        GameObject GetGameObject();
        GameObject GetTargetPoint();
        EntityType GetTargetType();
    }
}