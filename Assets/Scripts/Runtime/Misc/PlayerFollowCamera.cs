using System;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Misc
{
    public class PlayerFollowCamera : MonoBehaviour
    {
        public Vector3 distanceFromPlayer;
        public bool isFollowingPlayer = false;
        public GameObject playerObject;

        

        private void LateUpdate()
        {
            if (!isFollowingPlayer) return;

            transform.position = playerObject.transform.position + distanceFromPlayer;
        }

        private void StartFollowingPlayer()
        {
            playerObject = DictionaryHolder.Player.gameObject;
            isFollowingPlayer = true;
        }

        private void StopFollowingPlayer(int floorNum)
        {
            isFollowingPlayer = false;
        }
        
        
        private void Start()
        {
            EventManager.Instance.FloorStartsEvent += StartFollowingPlayer;
            EventManager.Instance.FloorEndsEvent += StopFollowingPlayer;
        }

        private void OnDestroy()
        {
            EventManager.Instance.FloorStartsEvent -= StartFollowingPlayer;
            EventManager.Instance.FloorEndsEvent -= StopFollowingPlayer;
        }

    }
}