using System;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Misc
{
    public class PlayerFollowCamera3D : MonoBehaviour
    {
        public Vector3 distanceFromPlayer;
        public bool isFollowingPlayer = false;
        public float followSpeed;
        public GameObject playerObject;
        
        private void FixedUpdate()
        {
            if (!isFollowingPlayer)
            {
                FollowPlayer();
                return;
            }

            transform.position = Vector3.Lerp(transform.position, playerObject.transform.position + distanceFromPlayer,
                followSpeed * Time.deltaTime);
        }

        private void FollowPlayer()
        {
            transform.position = Vector3.Lerp(transform.position, playerObject.transform.position + distanceFromPlayer,
                followSpeed * Time.deltaTime);
        }
    }
}