using System;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.WorldRelated
{
    [Serializable]
    public class FloorInitializer
    {
        private const string FloorTimerID = "floor_timer";

        [SerializeField] private int currentFloor;
        [SerializeField] private int currentTimeOnFloor;

        public void Initialise()
        {
//            Debug.Log("Loading Floor Initializer...");
            currentFloor = 0;

            EventManager.Instance.FloorLoadEvent += OnFloorLoad;
            EventManager.Instance.PlayerDiesEvent += OnPlayerDies;
            EventManager.Instance.FloorExitEvent += OnFloorExit;
            EventManager.Instance.FloorEndsEvent += OnFloorEnds;

            //Debug.Log("Floor Initializer loaded!");
        }

        private void StartFloor()
        {
            EventManager.Instance.FloorStarts();
            EventManager.Instance.UpdateFloorNumber(currentFloor + 1);
            EventManager.Instance.UpdateFloorTimer(GameConfig.FloorDuration - currentTimeOnFloor);

            DOVirtual.DelayedCall(1f, () =>
            {
                currentTimeOnFloor += 1;
                EventManager.Instance.UpdateFloorTimer(GameConfig.FloorDuration - currentTimeOnFloor);
            }).SetLoops(GameConfig.FloorDuration).SetId(FloorTimerID).OnComplete(CompleteFloor);

            Debug.Log("Floor Started!");
        }

        private void CompleteFloor()
        {
            EventManager.Instance.FloorEnds(currentFloor);
            currentFloor++;
        }

        private void OnFloorLoad()
        {
            Debug.Log("Loading Floor");
            currentTimeOnFloor = 0;
            StartFloor();
        }

        private void OnPlayerDies()
        {
        }

        private void OnFloorExit()
        {
        }

        private void OnFloorEnds(int floorNum)
        {
            
        }

        public void Destroy()
        {
            EventManager.Instance.FloorLoadEvent -= OnFloorLoad;
            EventManager.Instance.PlayerDiesEvent -= OnPlayerDies;
            EventManager.Instance.FloorExitEvent -= OnFloorExit;
            EventManager.Instance.FloorEndsEvent -= OnFloorEnds;
        }
    }
}