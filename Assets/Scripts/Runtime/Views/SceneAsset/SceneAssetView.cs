using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.View;
using Runtime.Utility;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Views.SceneAsset
{
    public class SceneAssetView : MVCView
    {
        public Camera MainCamera;
        public GameObject GameRoot;
        public AudioSource AudioSourceMusic;
        public AudioSource AudioSourceSoundEffects;
        public Camera GridCamera;
        public Tilemap Tilemap;
        public GameObject pointerObject;
        public GameObject playerSpawnPlace;
    }
}
