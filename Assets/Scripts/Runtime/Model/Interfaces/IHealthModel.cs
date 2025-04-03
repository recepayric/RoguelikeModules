using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Model.Interfaces
{
    public interface IHealthModel
    {
        public Dictionary<GameObject, HealthVO> HealthVos { get;  }
        public void RegisterHealth(GameObject gameObject, HealthVO healthVo);
        public void RemoveHealth(GameObject gameObject);
        public HealthVO GetHealth(GameObject gameObject);
    }
}