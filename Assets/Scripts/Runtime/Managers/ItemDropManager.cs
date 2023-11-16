using System;
using Runtime.Enums;
using UnityEngine;

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


        //Player drop data..
        //Calculate Chances

        public void DropItemFromEnemy(Enemy enemy)
        {
            //Check which kind of enemy it is, and drop based on that first
            //todo later

            var dropPos = enemy.gameObject.transform.position;

            var orb = BasicPool.instance.Get(PoolKeys.OrbPurple1);
            orb.transform.position = dropPos;
        }
    }
}