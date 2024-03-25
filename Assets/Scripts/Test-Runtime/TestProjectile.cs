using System;
using UnityEngine;

namespace Test_Runtime
{
    public class TestProjectile : MonoBehaviour
    {
        public bool isShot =false;
        public bool isHoming =false;
        public float speed;
        public float turnTime;
        public float turningTimer;
        public float lifetime;
        public GameObject hitParticlePrefab;
        public GameObject targetPoint;

        public Vector3 initialRotation;
        public Vector3 targetRotation;

        private void Start()
        {
            
        }

        public void SetRotation(float angleToAdd)
        {
            transform.Rotate(new Vector3(0, 1, 0), angleToAdd);
            initialRotation = transform.forward;
        }

        public void SetTargetPoint(GameObject _targetPoint)
        {
            if (_targetPoint == null) return;

            targetPoint = _targetPoint;
            targetRotation = targetPoint.transform.position - transform.position;
        }

        public void Update()
        {
            if (!isShot)
                return;
            
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
            
            
            var deltaTravel = Time.deltaTime * speed * transform.forward;
            transform.position += deltaTravel;
            
            if(isHoming)
                UpdateHoming();
        }

        private float turnMultiplier = 0f;
        private void UpdateHoming()
        {
            if (targetPoint == null) return;
            turningTimer += Time.deltaTime/turnTime*turnMultiplier;

            turnMultiplier += Time.deltaTime/0.125f;
            
            if (turningTimer > 1)
                turningTimer = 1;
            
            targetRotation = targetPoint.transform.position - transform.position;

            var angle = Vector3.Lerp(initialRotation, targetRotation, turningTimer);
            transform.forward = angle;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hitParticlePrefab != null)
            {
                var hitParticle = Instantiate(hitParticlePrefab);
                hitParticle.transform.position = transform.position;
            }
            else
            {
                Debug.Log("Projectile has no Explode Prefab");
            }
            Destroy(gameObject);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (hitParticlePrefab != null)
            {
                var hitParticle = Instantiate(hitParticlePrefab);
                hitParticle.transform.position = transform.position;
            }
            else
            {
                Debug.Log("Projectile has no Explode Prefab");
            }
            Destroy(gameObject);
        }
    }
}