using System;
using UnityEngine;

namespace Test_Runtime
{
    public class ExplodeParticle : MonoBehaviour
    {
        public float DestroyTimer;

        private void Update()
        {
            DestroyTimer -= Time.deltaTime;
            if(DestroyTimer <= 0)
                Destroy(gameObject);
        }
        
        
    }
}