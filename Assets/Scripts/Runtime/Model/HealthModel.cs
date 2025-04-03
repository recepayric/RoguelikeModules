using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Model.Interfaces;
using UnityEngine;

namespace Runtime.Model
{
    public class HealthModel : IHealthModel
    {
        private Dictionary<GameObject, HealthVO> _healthVos = new Dictionary<GameObject, HealthVO>();
        public Dictionary<GameObject, HealthVO> HealthVos => _healthVos;
        public void RegisterHealth(GameObject gameObject, HealthVO healthVo)
        {
            if(!_healthVos.ContainsKey(gameObject))
                _healthVos.Add(gameObject, healthVo);
        }

        public void RemoveHealth(GameObject gameObject)
        {
            if(_healthVos.ContainsKey(gameObject))
                _healthVos.Remove(gameObject);
        }

        public HealthVO GetHealth(GameObject gameObject)
        {
            if (_healthVos.ContainsKey(gameObject))
                return _healthVos[gameObject];

            return null;
        }
    }
}