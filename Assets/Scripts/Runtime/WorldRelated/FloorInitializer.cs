using System;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Managers;
using Runtime.UIRelated;
using UnityEngine;

namespace Runtime.WorldRelated
{
    [Serializable]
    public class FloorInitializer
    {
        private const string FloorTimerID = "floor_timer";

        [SerializeField] public int currentFloor;
        [SerializeField] private int currentTimeOnFloor;
        public bool isPlayerDead = false;
        private int starterFloor = 0;

        public void Reset()
        {
            currentFloor = starterFloor;
            isPlayerDead = false;
            DOTween.Kill(FloorTimerID);
        }
        
        public void Initialise()
        {
            //Debug.Log("Loading Floor Initializer...");
            currentFloor = starterFloor;

            //EventManager.Instance.FloorLoadEvent += OnFloorLoad;
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
            currentFloor++;
            EventManager.Instance.FloorEnds(currentFloor);
        }

        public void OnFloorLoad()
        {
            Debug.Log("Loading Floor");
            currentTimeOnFloor = 0;
            StartFloor();
        }

        private void OnPlayerDies()
        {
            isPlayerDead = true;
            EventManager.Instance.FloorEnds(currentFloor);
            EventManager.Instance.GameEnd(false);
        }

        private void OnFloorExit()
        {
            EventManager.Instance.FloorEnds(currentFloor);
        }

        private void OnFloorEnds(int floorNum)
        {
           
        }

        private void CheckForGameEnd(int round)
        {
            
        }

        public void Destroy()
        {
            //EventManager.Instance.FloorLoadEvent -= OnFloorLoad;
            EventManager.Instance.PlayerDiesEvent -= OnPlayerDies;
            EventManager.Instance.FloorExitEvent -= OnFloorExit;
            EventManager.Instance.FloorEndsEvent -= OnFloorEnds;
        }
    }
}