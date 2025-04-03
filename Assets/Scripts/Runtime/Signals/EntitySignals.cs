using Runtime.Data.Structs;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Runtime.Signals
{
    public class EntitySignals
    {
        public Signal InitializePlayerCameraSignal = new Signal();
        public Signal<GameObject> CalculateStatsSignal = new Signal<GameObject>();
        public Signal StatsUpdatedSignal = new Signal();
        public Signal<GameObject, float> CheckForCollectablesSignal = new Signal<GameObject, float>();

        public Signal PlayerDiedSignal = new Signal();

        public Signal<GameObject> PlayerDashedSignal = new Signal<GameObject>();
        public Signal StopCharacterMovementSignal = new Signal();
        public Signal StartCharacterMovementSignal = new Signal();
        public Signal<GameObject, float> HealPlayerSignal = new Signal<GameObject, float>();


        public Signal<GameObject> UseHealthPotionSignal = new Signal<GameObject>();

        //Particle
        public Signal<GameObject, float> StartParticleWithDuration = new Signal<GameObject, float>();

        public Signal<GameObject> InitializeHealth = new Signal<GameObject>();
        
    }
}