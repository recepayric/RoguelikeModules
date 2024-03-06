using System;
using UnityEngine;

namespace Test_Runtime
{
    public class TestProjectile : MonoBehaviour
    {
        public float speed;
        public float lifetime;
        public GameObject hitParticlePrefab;
        
        
        public void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
            
            
            var deltaTravel = Time.deltaTime * speed * transform.right;
            transform.position += deltaTravel;
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