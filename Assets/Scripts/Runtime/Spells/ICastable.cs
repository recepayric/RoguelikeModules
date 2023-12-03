using UnityEngine;

namespace Runtime.Spells
{
    public interface ICastable
    {
        void Cast();
        void Prepare(Vector3 targetPosition);
    }
}