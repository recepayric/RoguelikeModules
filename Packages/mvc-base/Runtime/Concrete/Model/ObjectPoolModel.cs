using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Data.ValueObject;
using strange.extensions.context.api;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Model
{
    public class ObjectPoolModel : IObjectPoolModel
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        private Dictionary<string, ObjectPoolVo> _poolVos;
        private Dictionary<string, Queue<GameObject>> _objectQueues;
        private Dictionary<GameObject, string> _levelObjectsPoolKeys;
        private List<GameObject> _activeLevelObjects = new List<GameObject>();
        public List<GameObject> ActiveLevelObjects => _activeLevelObjects;
        private GameObject container;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _poolVos = new Dictionary<string, ObjectPoolVo>();
            _objectQueues = new Dictionary<string, Queue<GameObject>>();
            _levelObjectsPoolKeys = new Dictionary<GameObject, string>();
            container = new GameObject("PoolObjects");
            container.transform.SetParent(contextView.transform);
        }

        private void AddToQueue(ObjectPoolVo vo, Queue<GameObject> queue, bool isLevelPoolObject)
        {
            for (var i = 0; i < vo.Count; i++)
            {
                var newObj = GameObject.Instantiate(vo.Prefab, container.transform);
                
                if (isLevelPoolObject)
                    _levelObjectsPoolKeys.Add(newObj, vo.Key);
                
                if (newObj.GetComponent<IPoolable>() is IPoolable poolable)
                    poolable.PoolKey = vo.Key;
                
                newObj.name = vo.Prefab.name;
                newObj.SetActive(false);
                queue.Enqueue(newObj);
            }
        }

        public void Pool(string key, GameObject prefab, int count, bool isLevelPoolInitialization = false, bool isLevelPoolObject = false)
        {
            if (isLevelPoolInitialization && _poolVos.ContainsKey(key))
                return;
            
            key = prefab.GetComponent<IPoolable>() == null ? prefab.transform.GetInstanceID().ToString() : key;

            var vo = new ObjectPoolVo { Key = key, Count = count, Prefab = prefab, IsLevelPoolObject = isLevelPoolObject };
            if (!_poolVos.ContainsKey(vo.Key))
                _poolVos.Add(vo.Key, vo);

            if (!_objectQueues.ContainsKey(vo.Key))
                _objectQueues.Add(vo.Key, new Queue<GameObject>());

            AddToQueue(vo, _objectQueues[vo.Key], isLevelPoolObject);
        }

        public void ReturnAll()
        {
            foreach (var pair in _objectQueues)
                foreach (var gameObject in pair.Value)
                    Return(gameObject);
        }

        public GameObject Get(string key, Transform parent)
        {
            var item = Get(key, true);
            if (item == null)
                return null;
            
            item.transform.SetParent(parent, false);
            item.GetComponent<IPoolable>()?.OnGetFromPool();
            
            return item;
        }

        public GameObject Get(string key, bool withParent = false, bool isLevelPoolObject = false)
        {
            if (!_objectQueues.TryGetValue(key, out var queue) || queue.Count == 0)
            {
                if (!_poolVos.TryGetValue(key, out var vo))
                {
                    Debug.LogWarning($"No object in pool with key {key}");
                    return null;
                }
                Pool(vo.Key, vo.Prefab, 1, false, vo.IsLevelPoolObject);
            }

            var newObj = _objectQueues[key].Dequeue();
            newObj.SetActive(true);
            
            if (isLevelPoolObject)
                _activeLevelObjects.Add(newObj);
            
            if (!withParent) 
                newObj.GetComponent<IPoolable>()?.OnGetFromPool();
            
            return newObj;
        }

        public void Return(GameObject obj)
        {
            if (!obj.activeInHierarchy)
                return;
            
            obj.GetComponent<IPoolable>()?.OnReturnToPool();
            
            if (_levelObjectsPoolKeys.TryGetValue(obj, out var key) && _poolVos[key].IsLevelPoolObject)
                _activeLevelObjects.Remove(obj);

            obj.transform.SetParent(container.transform);
            obj.SetActive(false);
            
            if (obj.GetComponent<IPoolable>() is IPoolable poolable)
                _objectQueues[poolable.PoolKey].Enqueue(obj);
            else if (_levelObjectsPoolKeys.ContainsKey(obj))
                _objectQueues[_levelObjectsPoolKeys[obj]].Enqueue(obj);
        }

        public bool Has(string key)
        {
            return _poolVos.ContainsKey(key);
        }
    }
}
