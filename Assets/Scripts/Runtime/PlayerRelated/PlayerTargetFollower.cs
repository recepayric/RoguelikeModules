using System;
using UnityEngine;

namespace Runtime.PlayerRelated
{
    public class PlayerTargetFollower : MonoBehaviour
    {
        public PlayerMovement PlayerMovement;
        public GameObject BoneToRotate;

        public bool isTargeting;
        public GameObject targetEnemy;

        public float angle_rad;
        public float angle_deg;
        public float angle_try;

        public float y1, y2;
        public float x1, x2;

        private void Update()
        {
            if (!isTargeting || targetEnemy == null || !targetEnemy.activeSelf)
                return;


            y1 = BoneToRotate.transform.position.y;
            x1 = BoneToRotate.transform.position.x;

            y2 = targetEnemy.transform.position.y;
            x2 = targetEnemy.transform.position.x;

            angle_try = Vector2.Angle(targetEnemy.transform.position - BoneToRotate.transform.position, Vector2.right);

            if (x1 < x2)
            {
                FaceTowards(1);
                angle_rad = Mathf.Atan2(y1 - y2, x2 - x1);

            }
            else
            {
                FaceTowards(-1);
                angle_rad = Mathf.Atan2(y1 - y2, x1 - x2);

            }
            
            angle_deg = Mathf.Rad2Deg * angle_rad + 90;

            BoneToRotate.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle_deg));

          
        }

        private void FaceTowards(int side)
        {
            //-1 left, stay original.
            //1 right, turn.

            var scaleX = transform.localScale.x;
            var scaleY = transform.localScale.y;
            var scaleZ = transform.localScale.z;

            if (side == -1)
                scaleX = scaleX < 0 ? -scaleX : scaleX;
            else
                scaleX = scaleX > 0 ? -scaleX : scaleX;

            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }

        public void SetTargeting(bool isTargeting)
        {
            this.isTargeting = isTargeting;
            PlayerMovement.isTargetingEnemy = isTargeting;
        }

        public void SetTarget(GameObject targetEnemy)
        {
            this.targetEnemy = targetEnemy;
        }
    }
}