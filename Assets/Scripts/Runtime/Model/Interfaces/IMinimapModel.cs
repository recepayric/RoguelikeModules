using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine.Events;

namespace Runtime.Model.Interfaces
{
    public interface IMinimapModel
    {
        public event UnityAction EnemyUpdatedEvent;
        List<MinimapObjectVO> MinimapObjectVos { get;  }
        void AddMinimapObjectVo(MinimapObjectVO minimapObjectVo);
        void RemoveMinimapObjectVo(MinimapObjectVO minimapObjectVo);
    }
    
    public class MinimapModel : IMinimapModel
    {
        public event UnityAction EnemyUpdatedEvent;
        private List<MinimapObjectVO> _minimapObjectVos = new List<MinimapObjectVO>();
        public List<MinimapObjectVO> MinimapObjectVos => _minimapObjectVos;
        
        public void AddMinimapObjectVo(MinimapObjectVO minimapObjectVo)
        {
            if (!_minimapObjectVos.Contains(minimapObjectVo))
                _minimapObjectVos.Add(minimapObjectVo);
            
            if(minimapObjectVo.type == MinimapObjectType.Enemy)
                EnemyUpdatedEvent?.Invoke();
        }

        public void RemoveMinimapObjectVo(MinimapObjectVO minimapObjectVo)
        {
            if (_minimapObjectVos.Contains(minimapObjectVo))
                _minimapObjectVos.Remove(minimapObjectVo);
            
            if(minimapObjectVo.type == MinimapObjectType.Enemy)
                EnemyUpdatedEvent?.Invoke();
        }
    }
}