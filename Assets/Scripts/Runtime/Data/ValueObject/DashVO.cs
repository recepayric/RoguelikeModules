using System;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class DashVO
    {
        public bool isDashing;
        public int maxDashAmount;
        public int remainingDashAmount;
        public float dashTime;
        public float dashCooldown;
        public float dashTimer;
        public float dashLength;
    }
}