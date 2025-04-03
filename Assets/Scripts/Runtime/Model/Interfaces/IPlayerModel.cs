using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Model.Interfaces
{
    public interface IPlayerModel
    {
        public Dictionary<GameObject, PlayerVO> PlayerVos { get;  }
        public GameObject ActivePlayer { get; set; }
        bool RegisterPlayer(GameObject gameObject, PlayerVO playerVo);
        bool RemovePlayer(GameObject gameObject);
    }
}