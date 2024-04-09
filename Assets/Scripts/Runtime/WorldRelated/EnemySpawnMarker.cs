using System;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

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
            didSpawn = true;
            
            var enemy = BasicPool.instance.Get(enemySpawnKey);
            enemy.transform.position = transform.position-Vector3.up*3;
            DictionaryHolder.Enemies[enemy].Spawn(transform.position);
            enemy.name = "Enemy " + Random.Range(0, 100000);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
                {
                    BasicPool.instance.Return(gameObject);
                });
            });
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