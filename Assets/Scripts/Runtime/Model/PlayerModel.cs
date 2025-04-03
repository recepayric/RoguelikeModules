using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Model.Interfaces;
using UnityEngine;

namespace Runtime.Model
{
    public class PlayerModel : IPlayerModel
    {
        private Dictionary<GameObject, PlayerVO> _playerVos = new Dictionary<GameObject, PlayerVO>();
        public Dictionary<GameObject, PlayerVO> PlayerVos => _playerVos;



        public GameObject ActivePlayer { get; set; }
        public float CollectedExperience { get; set; }


        /// <summary>
        /// Try to register player to the dictionary
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="playerVo"></param>
        /// <returns>True - if player successfully registered, False - if player already registered</returns>
        public bool RegisterPlayer(GameObject gameObject, PlayerVO playerVo)
        {
            if (!_playerVos.ContainsKey(gameObject))
            {
                _playerVos.Add(gameObject, playerVo);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try to remove player from the dictonary
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>True - if player successfully removed, False - if player was already removed</returns>
        public bool RemovePlayer(GameObject gameObject)
        {
            if (_playerVos.ContainsKey(gameObject))
            {
                _playerVos.Remove(gameObject);
                return true;
            }
            
            return false;
        }
    }
}