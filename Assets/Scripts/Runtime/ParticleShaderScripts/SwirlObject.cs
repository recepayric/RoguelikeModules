using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.ParticleShaderScripts
{
    public class SwirlObject : MonoBehaviour
    {
        public float spawnTime;
        public float spawnTimeNormalised;
        public float activeTime;
        public float activeTimeNormalised;
        public float despawnTime;
        public float despawnTimeNormalised;

        public float animationSpeed;
        
        public bool isSpawning;
        public bool isActive;
        public bool isDeSpawning;

        public float timer;

        public float swirlValue;

        public Material material;
        private static readonly int SwidlValue = Shader.PropertyToID("_SwidlValue");

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if(isSpawning)
                Spawn();
            else if(isActive)
                Active();
            else if(isDeSpawning)
                Despawn();
        }

        [Button]
        private void Normalise()
        {
            var total = activeTime + despawnTime + spawnTime;

            activeTimeNormalised = activeTime / total * animationSpeed;
            spawnTimeNormalised = spawnTime / total * animationSpeed;
            despawnTimeNormalised = despawnTime / total * animationSpeed;
        }

        public void StartSwirling(float swirlTime)
        {
            swirlValue = 0;
            timer = 0;
            spawnTimeNormalised = swirlTime;
            isSpawning = true;
        }

        public void StopSwirling()
        {
            isSpawning = false;
            isDeSpawning = true;
            despawnTimeNormalised = spawnTimeNormalised;
        }

        [Button]
        public void StartAnimation()
        {
            Normalise();
            timer = 0;
            isSpawning = true;
        }

        public void Spawn()
        {
            swirlValue += Time.deltaTime/spawnTimeNormalised;

            if (swirlValue >= 1)
            {
                swirlValue = 1;
                isSpawning = false;
                //isActive = true;
            }
                
            material.SetFloat(SwidlValue, swirlValue);
        }
        
        public void Active()
        {
            if (timer >= spawnTimeNormalised + activeTimeNormalised)
            {
                isActive = false;
                isDeSpawning = true;
            }
        }
        
        public void Despawn()
        {
            swirlValue -= Time.deltaTime/despawnTimeNormalised;

            if (swirlValue <= 0)
            {
                swirlValue = 0;
                isDeSpawning = false;
            }
                
            material.SetFloat(SwidlValue, swirlValue);
        }
    }
}
