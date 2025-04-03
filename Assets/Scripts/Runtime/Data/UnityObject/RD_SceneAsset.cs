using System.Collections.Generic;
using Runtime.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/SceneAsset", order = 11)]
    public class RD_SceneAsset : SerializedScriptableObject
    {
        public Camera MainCamera;
        public Camera GridCamera;
        public Transform GameRoot;
        public Tilemap Tilemap;
        public GameObject pointerObject;
    }
}