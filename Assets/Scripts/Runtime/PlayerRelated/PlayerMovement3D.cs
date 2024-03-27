using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.PlayerRelated
{
    public class PlayerMovement3D : MonoBehaviour
    {
        public Animator playerAnimator;
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        private float _moveX;
        private float _moveZ;
        public float moveSpeed = 5;
        public float angleThreshold = 0.1f;
        public float angleOffset = -45;
        public float angleDegree;
        public float targetAngle;
        public float turnSpeed = 180;

        private void Start()
        {
            angleDegree = transform.rotation.eulerAngles.y;
        }

        private void FixedUpdate()
        {
            _moveX = Input.GetAxisRaw("Horizontal");
            _moveZ = Input.GetAxisRaw("Vertical");

            if (_moveX == 0 && _moveZ == 0)
            {
                Stop();
                return;
            }
            var angle = Mathf.Atan2(_moveX, _moveZ);
            targetAngle = Mathf.Rad2Deg * angle;
            if (targetAngle < 0)
                targetAngle += 360;
            
            
            Move();
        }

        private void Move()
        {
            var deltaMove = transform.forward * moveSpeed * Time.deltaTime;
            transform.position += deltaMove;
            Rotate();
        }

        private float diffNormal;
        private float diffInverse;
        private void Rotate()
        {
           var turn = angleDegree < targetAngle ? 1 : -1;
            var targetAngle2 = 360 - (angleDegree - targetAngle);

            diffNormal = Math.Abs(angleDegree - targetAngle);
            diffInverse = 360-diffNormal;

            if (diffInverse < diffNormal)
                turn = turn == 1 ? -1 : 1;

            if (diffNormal <= angleThreshold)
                return;
            if (diffInverse <= angleThreshold)
                return;
            
            if (turn == 1)
            {
                angleDegree += turnSpeed * Time.deltaTime;
            }
            else if (turn == -1)
            {
                angleDegree -= turnSpeed * Time.deltaTime;
            }

            if (angleDegree > 360)
                angleDegree -= 360;
            if (angleDegree < 0)
                angleDegree += 360;

            transform.rotation = Quaternion.Euler(new Vector3(0, angleDegree + angleOffset, 0));
        }

        private void Stop()
        {
        }
    }
}