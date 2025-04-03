using System.Collections.Generic;
using UnityEngine;

namespace MVC.Base.Runtime.Abstract.Model
{
    public interface IObjectPoolModel
    {
        /// <summary>
        /// Instansiate the pool objects
        /// </summary>
        /// <param name="key"> Unique key value to store on pool </param>
        /// <param name="prefab"> A Prefab to instansiate </param>
        /// <param name="count"> How many gameobject should be created </param>
        void Pool(string key, GameObject prefab, int count, bool isLevelPoolInitialization = false, bool isLevelPoolObject = false);

        /// <summary>
        /// Return a random gameobject from pool
        /// </summary>
        /// <param name="key"> Key of the desired pool object </param>
        /// <returns></returns>
        GameObject Get(string key, bool withParent = false, bool isLevelPoolObject = false);
        
        /// <summary>
        /// Return a random gameobject from pool
        /// </summary>
        /// <param name="key"> Key of the desired pool object </param>
        /// <returns></returns>
        GameObject Get(string key,Transform parent);

        /// <summary>
        /// Use to disable and return the gameobject to pool
        /// </summary>
        /// <param name="obj"> Gameobject to be disabled </param>
        void Return(GameObject obj);

        /// <summary>
        /// Return all pool objects
        /// </summary>
        void ReturnAll();

        /// <summary>
        /// Check if pool has the key
        /// </summary>
        /// <returns></returns>
        bool Has(string key);
        
        public List<GameObject> ActiveLevelObjects { get; }

    }
}