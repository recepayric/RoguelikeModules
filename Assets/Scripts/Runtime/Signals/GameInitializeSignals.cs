using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Runtime.Signals
{
    /// <summary>
    /// Initialize Signals used for events like Setup,Tutorial,Localization,Save/Load ,SDK/API,
    /// </summary>
    public class GameInitializeSignals
    {
        public Signal InitializeGameSignal = new Signal();
        
        
        //Floor
        public Signal LoadFloorSignal = new Signal();
        
    }
}