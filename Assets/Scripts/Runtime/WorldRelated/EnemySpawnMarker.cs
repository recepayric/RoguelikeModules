using System;
using Runtime.Configs;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.WorldRelated
{
    public class EnemySpawnMarker : MonoBehaviour, IPoolObject
    {
        public float timer;
        public float scale;
        public float scaleMult;
        public bool didSpawn = false;
        public PoolKeys enemySpawnKey;
        
        
        private void Update()
        {
            if (didSpawn) return;
            
            timer -= Time.deltaTime;

            scale = 1 - timer * scaleMult;
            transform.localScale = Vector3.one * scale;
            
            if(timer <= 0)
                FinishSpawn();
        }

        private void FinishSpawn()
        {
            var enemy = BasicPool.instance.Get(enemySpawnKey);
            enemy.transform.position = transform.position;
            BasicPool.instance.Return(gameObject);
        }

        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
            DictionaryHolder.EnemySpawnMarkers.Remove(gameObject);
        }

        public void OnGet()
        {
            didSpawn = false;
            timer = MapConfig.EnemySpawnTimer;
            DictionaryHolder.EnemySpawnMarkers.Add(gameObject, this);
            transform.localScale = Vector3.zero;

            scaleMult = 1f / timer;
        }
    }
}