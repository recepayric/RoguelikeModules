using UnityEngine;
using UnityEngine.Events;

namespace MVC.Base.Runtime.Concrete.Handler.UnityObject
{
    [AddComponentMenu("")]
    public class UpdateProviderEventHandler : MonoBehaviour
    {
        public event UnityAction onUpdate;
        public event UnityAction onLateUpdate;
        public event UnityAction onFixedUpdate;

        private void Update()
        {
            onUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            onFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            onLateUpdate?.Invoke();
        }
    }
}