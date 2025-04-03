using System.Collections.Generic;
using JetBrains.Annotations;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Runtime.Signals
{
    public class UISignals
    {
        public Signal<float, float> UpdateHealthUISignal = new Signal<float, float>();
        public Signal<float, float> UpdateEnemyHealthUISignal = new Signal<float, float>();
    }
}