using Runtime.Enums;
using UnityEngine;

namespace Runtime.SpellsRelated
{
    public interface ICastable
    {
        bool FollowsOwner { get; set; }
        void Cast();
        void Activate();
        void Prepare();
        void SetPosition(Vector3 targetPosition);
        void SetOwner(Owners owner);
    }
}