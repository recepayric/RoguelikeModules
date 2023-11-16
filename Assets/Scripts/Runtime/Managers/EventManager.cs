using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event UnityAction<bool> SetMonsterSpawning;
        public void SetMonsterSpawn(bool status) => SetMonsterSpawning?.Invoke(status);
        

        public event UnityAction UpdateResCountEvent;
        public void UpdateResCount() => UpdateResCountEvent?.Invoke();
        
        #region Static Events

        public static event UnityAction<float> EventSetDistanceBetweenEnemy;
        public void SetDistanceBetweenEnemy(float distance) => EventSetDistanceBetweenEnemy?.Invoke(distance);

        #endregion
    }
}