using System;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Runtime.Data.Structs
{
    [Serializable]
    public struct ChargeAttackSettings
    {
        public Ease chargeTweenEase;
        
        public bool hasReadyAnimation;
        public bool rollWhileCharging;
        public bool rotateWhileCharging;
        public bool getUpAfterCharge;
        public bool stunAfterCharge;
        public bool hasStopAnimation;

        [PropertySpace]
        public float chargeDistance;
        public float chargePrepTime;

        [PropertySpace]
        [ShowIf("@hasReadyAnimation")] public float readyAnimationTime;
        [ShowIf("@hasReadyAnimation")] public float readyAnimationSwitchTime;
        
        
        [ShowIf("@rollWhileCharging")] public float rollSpeed;
        [ShowIf("@rotateWhileCharging")] public float rotateSpeed;
        
        [PropertySpace]
        public float chargeRunTransitionTime;
        public float chargeStopTransitionTime;
        
        [PropertySpace]
        public float chargeAnimationSpeed;
        public float chargeSpeedMultiplier;

        [PropertySpace] public float stopTime;

        [PropertySpace]
        [ShowIf("@getUpAfterCharge")] public float getUpAnimationTransitionTime;
        [ShowIf("@getUpAfterCharge")] public float getUpTime;

        [PropertySpace]
        [ShowIf("@stunAfterCharge")]
        public float afterChargeStunTime;
        [ShowIf("@stunAfterCharge")]
        public float afterChargeStunTransitionTime;
    }
}