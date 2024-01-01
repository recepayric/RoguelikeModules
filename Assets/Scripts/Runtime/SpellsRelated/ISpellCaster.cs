
using Runtime.Enums;
using UnityEngine;

namespace Runtime.SpellsRelated
{
    public interface ISpellCaster
    {
        GameObject GetGameObject();
        float GetRange();
        Vector3 GetSpellPosition();
        void AddGambleStat(AllStats stat, float increase);
    }
}