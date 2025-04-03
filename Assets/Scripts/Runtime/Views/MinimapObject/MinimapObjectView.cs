using UnityEngine.Events;
using MVC.Base.Runtime.Abstract.View;
using Runtime.Data.ValueObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Views.MinimapObject
{
    public class MinimapObjectView : MVCView
    {
        public MinimapObjectVO minimapObjectVo;


        [Button]
        public void Setup()
        {
            var boxCollider = GetComponent<BoxCollider>();

            if (boxCollider != null)
                minimapObjectVo.boxCollider = boxCollider;
            
            minimapObjectVo.minimapObject = gameObject;
        }
    }
}
