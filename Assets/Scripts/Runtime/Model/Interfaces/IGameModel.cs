using System.Collections.Generic;
using Runtime.Data.UnityObject;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Model
{
    /// <summary>
    /// game related datas
    /// </summary>
    public interface IGameModel
    {
        RD_Pool Pool { get; }
        RD_SceneAsset SceneAsset { get; }
        RD_BasePlayerStats BasePlayerStats { get; }
        RD_Player SelectedCharacter { get; set; }
        bool IsMinionInitialized { get; set; }
        bool IsConnectedToServer { get; set; }
        bool IsAdmin { get; set; }
        bool IsEscapeScreenOpen { get; set; }
        bool IsGamePaused { get; set; }
        bool ExitingGame { get; set; }
        public string ActiveScreen { get; set; }

        List<GameObject> PlayerSpawnPositions { get; set; }
    }


    
}