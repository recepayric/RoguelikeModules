using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Code;
#endif
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Pool", order = 30)]
    public class RD_Pool : SerializedScriptableObject
    {
        public List<PoolVO> List;

#if UNITY_EDITOR
        private string infoBoxMessage = string.Empty;

        [InfoBox("$infoBoxMessage", InfoMessageType.Warning)]
        [BoxGroup("Add Pool Key")]
        [ShowInInspector]
        private string key;

        [ShowInInspector]
        [BoxGroup("Add Pool Key")]
        private int count;

        [ShowInInspector]
        [BoxGroup("Add Pool Key")]
        private GameObject prefab;

        [BoxGroup("Add Pool Key")]
        [Button(ButtonSizes.Large), GUIColor(0,1,0)]
        protected void CreatePoolKey()
        {
            if (string.IsNullOrEmpty(key))
            {
                ShowMessage("Key Cannot Be Empty");
                return;
            }

            string poolKeyPath = "Assets/Scripts/Runtime/Enums/PoolKey.cs";
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(poolKeyPath);

            if (obj == null)
            {
                ShowMessage("There is No PoolKey File");
                return;
            }

            string data = LoadFileOnPath(poolKeyPath);

            if (data.Contains(key))
            {
                ShowMessage("Enum already exists");
                return;
            }

            data = AddKeyToData(data, key);
            CodeUtilities.SaveFile(data, poolKeyPath);

            ShowMessage("Enum is Added");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1.0f)]
        [BoxGroup("Add Pool Key")]
        private void AddToList()
        {
            if (string.IsNullOrEmpty(key) || prefab == null)
            {
                ShowMessage("Prefab || Key Empty");
                return;
            }

            try
            {
                PoolVO vo = new PoolVO
                {
                    Count = count,
                    Key = (PoolKey)Enum.Parse(typeof(PoolKey), key),
                    Prefab = prefab
                };

                List ??= new List<PoolVO>();
                List.Add(vo);

                ResetInputFields();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                ShowMessage("Enum is not declared");
            }
        }

        public string baseName;
        public string categoryName;
        public List<string> projectileNames;

        public List<string> recentlyAddedPoolKeys;
        public List<GameObject> prefabsToAdd;
        
        [BoxGroup("Add Pool Key")]
        [Button(ButtonSizes.Large), GUIColor(0,1,0)]
        protected void CreateMultiplePoolKeys()
        {

            recentlyAddedPoolKeys.Clear();
            string poolKeyPath = "Assets/Scripts/Runtime/Enums/PoolKey.cs";
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(poolKeyPath);

            if (obj == null)
            {
                ShowMessage("There is No PoolKey File");
                return;
            }

            string data = LoadFileOnPath(poolKeyPath);

            for (int i = 0; i < projectileNames.Count; i++)
            {
                var stringBaseProjectile = baseName + categoryName;
                var specificName = stringBaseProjectile + projectileNames[i];
                Debug.Log(specificName);
                recentlyAddedPoolKeys.Add(specificName);
                data = AddKeyToData(data, specificName);
            }
            
            for (int i = 0; i < projectileNames.Count; i++)
            {
                var stringBaseMuzzle = baseName + "Muzzle" + categoryName;
                var specificName = stringBaseMuzzle + projectileNames[i];
                Debug.Log(specificName);
                recentlyAddedPoolKeys.Add(specificName);
                data = AddKeyToData(data, specificName);


            }
            
            for (int i = 0; i < projectileNames.Count; i++)
            {
                var stringBaseHit = baseName + "Hit" + categoryName;
                var specificName = stringBaseHit + projectileNames[i];
                Debug.Log(specificName);
                recentlyAddedPoolKeys.Add(specificName);
                data = AddKeyToData(data, specificName);

            }
            
            //data = AddKeyToData(data, key);
            CodeUtilities.SaveFile(data, poolKeyPath);

            ShowMessage("Enum is Added");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
        
        [BoxGroup("Add Pool Key")]
        [Button(ButtonSizes.Large), GUIColor(0,1,0)]
        protected void AddMultipleToPool()
        {
            for (int i = 0; i < recentlyAddedPoolKeys.Count; i++)
            {
               try
               {
                    PoolVO vo = new PoolVO
                    {
                        Count = 0,
                        Key = (PoolKey)Enum.Parse(typeof(PoolKey), recentlyAddedPoolKeys[i]),
                        Prefab = prefabsToAdd[i]
                    };

                    List ??= new List<PoolVO>();
                    List.Add(vo);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    ShowMessage("Enum is not declared");
                }
            }
           
        }

        public PoolKey keyToSelect;
        public PoolVO poolVo;


        [Button]
        public void GetOne()
        {
            poolVo = List.Find(t => t.Key == keyToSelect);
        }

        public PoolKey keyToRemove;
        public bool doesKeyExist;
        
        [Button]
        public void DoesKeyExist()
        {
            doesKeyExist = List.Find(t => t.Key == keyToRemove) != null;
        }
        
        [Button]
        public void RemoveFromPool()
        {
            var voToDelete = List.Find(t => t.Key == keyToRemove);
            List.Remove(voToDelete);
        }

        public List<PoolKey> unusedKeys;
        [Button]
        public void GetUnusedKeys()
        {
            unusedKeys.Clear();
            
            foreach(PoolKey pieceType in Enum.GetValues(typeof(PoolKey)))
            {
                var poolVo = List.Find(t => t.Key == pieceType);

                if (poolVo != null) continue;
                
                if(pieceType.ToString().Contains("Placeholder")) continue;
                if(pieceType.ToString().Contains("PlaceHolder")) continue;
                
                unusedKeys.Add(pieceType);
            }
        }

        private string AddKeyToData(string data, string key)
        {
            string addition = $"\r\t\t{key},//-\r\t\tADDPOINT";
            data = data.Replace("//*ADDITION*//", addition);
            data = data.Replace("ADDPOINT", "//*ADDITION*//");
            return data;
        }
        
        [ReadOnly]public string ImportantNote = "Don't forget to add the IPoolable interface.";
        
        private void ResetInputFields()
        {
            count = 0;
            key = string.Empty;
            prefab = null;
        }

        private void ShowMessage(string text)
        {
            infoBoxMessage = text;
        }

        private string LoadFileOnPath(string filePath)
        {
            try
            {
                using StreamReader reader = new StreamReader(filePath, Encoding.Default);
                return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return string.Empty;
            }
        }
        
#endif
    }
}
