using System;
using Runtime.UIRelated;
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
            Debug.Log("Event Manager is loaded!");
        }


        public event UnityAction<bool> SetMonsterSpawning;
        public void SetMonsterSpawn(bool status) => SetMonsterSpawning?.Invoke(status);


        public event UnityAction UpdateResCountEvent;
        public void UpdateResCount() => UpdateResCountEvent?.Invoke();

        #region Floor Related

        public event UnityAction FloorEndsEvent;
        public void FloorEnds() => FloorEndsEvent?.Invoke();

        public event UnityAction FloorStartsEvent;
        public void FloorStarts() => FloorStartsEvent?.Invoke();

        public event UnityAction FloorLoadEvent;
        public void FloorLoad() => FloorLoadEvent?.Invoke();

        public event UnityAction FloorExitEvent;
        public void FloorExit() => FloorExitEvent?.Invoke();

        #endregion


        #region UI Related

        public event UnityAction<int> UpdateFloorTimerEvent;
        public void UpdateFloorTimer(int remainingTime) => UpdateFloorTimerEvent?.Invoke(remainingTime);
        
        public event UnityAction<int> UpdateFloorNumberEvent;
        public void UpdateFloorNumber(int floorNumber) => UpdateFloorNumberEvent?.Invoke(floorNumber);
        
        public event UnityAction<Screens, bool> OnOpenScreen;
        public void OpenScreen(Screens sceneToOpen, bool closePreviousScreens) => OnOpenScreen?.Invoke(sceneToOpen, closePreviousScreens);
        
        public event UnityAction<Screens> OnCloseScreen;
        public void CloseScreen(Screens sceneToClose) => OnCloseScreen?.Invoke(sceneToClose);

        #endregion

        public event UnityAction PlayerDiesEvent;
        public void PlayerDies() => PlayerDiesEvent?.Invoke();

        #region Static Events

        public static event UnityAction<float> EventSetDistanceBetweenEnemy;
        public void SetDistanceBetweenEnemy(float distance) => EventSetDistanceBetweenEnemy?.Invoke(distance);

        #endregion
    }
}