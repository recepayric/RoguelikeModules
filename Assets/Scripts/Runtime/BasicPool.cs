using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasicPool : MonoBehaviour
{
    public static BasicPool instance;
    public bool isLoaded;

    private void Awake()
    {
        instance = this;
        poolObjects = new Dictionary<PoolKeys, PoolObjectHolder>();
        _allObjects = new Dictionary<GameObject, IPoolObject>();

        isLoaded = true;
    }

    public List<PoolKeys> keys;
    public List<GameObject> prefabs;

    private Dictionary<PoolKeys, PoolObjectHolder> poolObjects;
    private Dictionary<GameObject, IPoolObject> _allObjects;

    // Start is called before the first frame update
    void Start()
    {
    }

    public GameObject prefab;
    public PoolKeys key;

    [Button]
    public void AddToPool()
    {
        keys.Add(key);
        prefabs.Add(prefab);

        prefab = null;
    }

    public GameObject Get(PoolKeys poolObject)
    {
        if (!poolObjects.ContainsKey(poolObject))
        {
            var objHolder = new PoolObjectHolder();
            objHolder.key = poolObject;
            poolObjects.Add(poolObject, objHolder);
        }

        var obj = poolObjects[poolObject].GetAvailable();
        return obj;
    }

    public bool isEditing = false;

    [ShowIf("isEditing")] [OnValueChanged("GetPrefabForKey")]
    public PoolKeys poolKeyToEdit;

    [ShowIf("isEditing")] public GameObject objectForTheKey;

    public void GetPrefabForKey()
    {
        var index = keys.IndexOf(poolKeyToEdit);
        if (index >= 0)
            objectForTheKey = prefabs[index];
        else
            objectForTheKey = null;
    }

    [ShowIf("isEditing")]
    [Button]
    public void Change()
    {
        var index = keys.IndexOf(poolKeyToEdit);
        if (index >= 0)
        {
            prefabs[index] = objectForTheKey;
        }
    }

    public List<PoolKeys> missingKeys;

    [Button]
    public void CheckMissingKeys()
    {
        if (missingKeys == null) missingKeys = new List<PoolKeys>();
        missingKeys.Clear();

        var PieceTypeNames = System.Enum.GetValues(typeof(PoolKeys));

        for (int i = 0; i < PieceTypeNames.Length; i++)
        {
            var key = (PoolKeys) PieceTypeNames.GetValue(i);
            if (!keys.Contains(key))
            {
                missingKeys.Add(key);
            }
        }
    }

    public void Return(GameObject obj)
    {
        poolObjects[_allObjects[obj].PoolKeys].ReturnObject(obj);
    }

    private class PoolObjectHolder
    {
        public PoolKeys key;
        public Dictionary<GameObject, IPoolObject> PoolObjects = new Dictionary<GameObject, IPoolObject>();
        public Dictionary<GameObject, IPoolObject> ObjectsInUse = new Dictionary<GameObject, IPoolObject>();

        public GameObject GetAvailable()
        {
            if (PoolObjects.Count == 0)
            {
                var index = instance.keys.IndexOf(key);
                var prefab = instance.prefabs[index];
                var objCreated = Instantiate(prefab);
                var sc = objCreated.GetComponent<IPoolObject>();
                PoolObjects.Add(objCreated, sc);
                instance._allObjects.Add(objCreated, sc);
            }

            var objToReturn = PoolObjects.Keys.First();
            objToReturn.SetActive(true);
            PoolObjects[objToReturn].OnGet();
            PoolObjects[objToReturn].PoolKeys = key;

            ObjectsInUse.Add(objToReturn, PoolObjects[objToReturn]);
            PoolObjects.Remove(objToReturn);
            return objToReturn;
        }

        public void ReturnObject(GameObject gameObject)
        {
            if (!ObjectsInUse.ContainsKey(gameObject))
                return;

            ObjectsInUse[gameObject].OnReturn();
            gameObject.SetActive(false);
            gameObject.transform.parent = instance.transform;
            PoolObjects.Add(gameObject, ObjectsInUse[gameObject]);
            ObjectsInUse.Remove(gameObject);
        }
    }
}