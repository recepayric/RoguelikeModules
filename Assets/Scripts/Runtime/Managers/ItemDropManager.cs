using System;
using Runtime.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Managers
{
    public class ItemDropManager : MonoBehaviour
    {
        #region instance

        public static ItemDropManager instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion


        public float towerFragmentDropChance;
        public float regularItemDropChance;
        
        //Player drop data..
        //Calculate Chances

        public void DropItemFromEnemy(Enemy enemy)
        {
            //Check which kind of enemy it is, and drop based on that first
            //todo later

           DropCurrency(Random.Range(0, 1f), enemy);
           DropTowerFragment(Random.Range(0, 1f), enemy);
        }

        private void DropTowerFragment(float rnd, Enemy enemy)
        {
            if (rnd > towerFragmentDropChance) return;
            var dropPos = enemy.gameObject.transform.position;

            var orb = BasicPool.instance.Get(PoolKeys.OrbTowerFragment);
            var randX = Random.Range(-0.5f, 0.5f);
            var randY = Random.Range(-0.5f, 0.5f);
            orb.transform.position = dropPos + new Vector3(randX, randY);

        }

        private void DropCurrency(float rnd, Enemy enemy)
        {
            if (rnd > regularItemDropChance) return;
            
            var dropPos = enemy.gameObject.transform.position;

            var orb = BasicPool.instance.Get(PoolKeys.OrbPurple1);
            var randX = Random.Range(-0.5f, 0.5f);
            var randY = Random.Range(-0.5f, 0.5f);
            orb.transform.position = dropPos + new Vector3(randX, randY);
        }
    }
}