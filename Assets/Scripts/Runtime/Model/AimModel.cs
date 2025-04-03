using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Model
{
    public class AimModel : IAimModel
    {
        public Dictionary<GameObject, AimVO> _aimVos = new Dictionary<GameObject, AimVO>();
        public Dictionary<GameObject, AimVO> AimVos => _aimVos;
        
        public AimVO RegisterAimVo(GameObject gameObject, AimVO aimVo)
        {
            if (!_aimVos.ContainsKey(gameObject))
            {
                _aimVos.Add(gameObject, aimVo);
            }

            return _aimVos[gameObject];
        }

        public bool RemoveAimVo(GameObject gameObject)
        {
            if (_aimVos.ContainsKey(gameObject))
            {
                _aimVos.Remove(gameObject);
                return true;
            }
            
            return false;
        }

        public AimVO GetAimVo(GameObject gameObject)
        {
            if (_aimVos.ContainsKey(gameObject))
                return _aimVos[gameObject];

            return null;
        }
    }
}