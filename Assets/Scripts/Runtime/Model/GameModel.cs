using System.Collections.Generic;
using Runtime.Data.UnityObject;
using UnityEngine;

namespace Runtime.Model
{
    public class GameModel : IGameModel
    {
        private RD_Pool _pool;
        private RD_SceneAsset _sceneAsset;
        private RD_BasePlayerStats _basePlayerStats;
        public RD_Player SelectedCharacter { get; set; }
        public bool IsMinionInitialized { get; set; }
        public bool IsConnectedToServer { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEscapeScreenOpen { get; set; }
        public bool IsGamePaused { get; set; }
        public bool ExitingGame { get; set; }
        public string ActiveScreen { get; set; }
        public List<GameObject> PlayerSpawnPositions { get; set; }


        [PostConstruct]
        public void OnPostConstruct()
        {
            PlayerSpawnPositions = new List<GameObject>();
            _pool = Resources.Load<RD_Pool>("Data/Pool");
            _sceneAsset = Resources.Load<RD_SceneAsset>("Data/SceneAsset");
            _basePlayerStats = Resources.Load<RD_BasePlayerStats>("Data/BaseCharacterData");
        }
        
        public RD_Pool Pool
        {
            get
            {
                if (_pool == null)
                    OnPostConstruct();
                return _pool;
            }
        }
        
        public RD_SceneAsset SceneAsset
        {
            get
            {
                if (_sceneAsset == null)
                    OnPostConstruct();
                return _sceneAsset;
            }
        }
        
        public RD_BasePlayerStats BasePlayerStats
        {
            get
            {
                if (_basePlayerStats == null)
                    OnPostConstruct();
                return _basePlayerStats;
            }
        }

    }
}