using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Model
{
    public interface IAimModel
    {
        public Dictionary<GameObject, AimVO> AimVos { get;  }
        AimVO RegisterAimVo(GameObject gameObject, AimVO aimVo);
        bool RemoveAimVo(GameObject gameObject);
        AimVO GetAimVo(GameObject gameObject);
    }
}