using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Structs
{
    [Serializable]
    public struct CurseToAdd
    {
        public GameObject GameObject;
        public float remainingTime;
        public float curseAmount;
        public AllStats statToAffect;

        public CurseToAdd(GameObject gameObject, AllStats stat, float amount, float time)
        {
            GameObject = gameObject;
            remainingTime = time;
            curseAmount = amount;
            statToAffect = stat;
        }
    }
}