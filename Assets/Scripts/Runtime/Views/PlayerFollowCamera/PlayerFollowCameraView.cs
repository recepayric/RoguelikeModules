using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.View;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Views.PlayerFollowCamera
{
    public class PlayerFollowCameraView : MVCView
    {
        //public float followSpeed;
        public Vector3 distanceFromPlayer;
        public bool isFollowingPlayer = false;
        public GameObject playerObject;
        
        public float distanceToPlayer;
        public float distanceYToPlayer;
        public float xAngleOffset;
        
        public float turnSpeed;
        public float followSpeed;
        public float angle;
        public float targetAngle;

        public float mouseDragMultiplier = 1;
        public bool isMouseDown = false;
        public Vector3 mousePos;
        public Vector3 mousePosLast;
        public Vector3 mouseDrag;
        
        public GameObject insideCamera;
        
        private Vector3 originalPosition;
        public float duration;
        public float magnitude;
        private bool isShaking = false;
        private float shakeElapsed = 0f;

        public float magnitudeCustom;
        public float durationCustom;
        public float cameraYRotation;

        public bool canRotateCamera;
        
        [Button]
        public void TriggerShake()
        {
            isShaking = true;
            shakeElapsed = 0f;
            
            magnitudeCustom = magnitude;
            durationCustom = duration;
        }

        public void TriggerShake(float shakeMagnitude, float time)
        {
            magnitudeCustom = shakeMagnitude;
            durationCustom = time;
            isShaking = true;
            shakeElapsed = 0f;
        }
        
        private void HandleShake()
        {
            if (isShaking)
            {
                if (shakeElapsed < durationCustom)
                {
                    float x = Random.Range(-1f, 1f) * magnitudeCustom;
                    float y = Random.Range(-1f, 1f) * magnitudeCustom;

                    insideCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0);
                    shakeElapsed += Time.deltaTime;
                }
                else
                {
                    isShaking = false;
                    insideCamera.transform.localPosition = originalPosition;
                }
            }
        }

        private void LateUpdate()
        {
            if (!isFollowingPlayer) return;
            
            HandleShake();
            CameraMove();
        }
        
        private void Update()
        {
            if (!canRotateCamera)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                if(canRotateCamera)
                    isMouseDown = true;
                else
                    isMouseDown = false;
                mousePosLast = Input.mousePosition;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
            }
            
            if (Input.GetMouseButton(0))
            {
                if (isMouseDown)
                {
                    var pos = Input.mousePosition;
                    mouseDrag = mousePosLast - pos;
                    targetAngle += mouseDrag.x*mouseDragMultiplier;
                    mousePosLast = pos;
                }
            }
        }
        
        private void CameraMove()
        {
            if (playerObject == null) return;
            //return;
            angle = Mathf.Lerp(angle, targetAngle, turnSpeed * Time.deltaTime);
            var tempAngle = angle - 90;
            var angleRad = Mathf.Deg2Rad * tempAngle;
 
            var posX = distanceToPlayer * Mathf.Cos(angleRad);
            var posZ = distanceToPlayer * Mathf.Sin(angleRad);
            
            var targetPos = playerObject.transform.position + new Vector3(posX, distanceYToPlayer, posZ);
            
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
            //transform.LookAt(playerObject.transform);
            //return;
            if(canRotateCamera)
                transform.LookAt(playerObject.transform);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(45+xAngleOffset, cameraYRotation, 0)), 3f*Time.deltaTime);
        }
        
        public void StartFollowingPlayer()
        {
            //playerObject = DictionaryHolder.Player.gameObject;
            //playerMovementScript = playerObject.GetComponent<PlayerMovement3D>();
            //playerScript = playerObject.GetComponent<Player>();
            isFollowingPlayer = true;
        }
        
        public void StopFollowingPlayer()
        {
            isFollowingPlayer = false;
        }

        private void OnActivateCameraRotation()
        {
            canRotateCamera = true;
        }

        private void OnDisableCameraRotation()
        {
            canRotateCamera = false;
            isMouseDown = false;
            targetAngle = -45;
        }
    }
}