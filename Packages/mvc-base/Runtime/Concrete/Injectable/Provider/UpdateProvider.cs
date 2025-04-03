using System.Collections;
using MVC.Base.Runtime.Abstract.Injectable.Provider;
using MVC.Base.Runtime.Concrete.Handler.UnityObject;
using strange.extensions.context.api;
using UnityEngine;
using UnityEngine.Events;

namespace MVC.Base.Runtime.Concrete.Injectable.Provider
{
    public class UpdateProvider : IUpdateProvider
    {
#pragma warning disable 67
        private event UnityAction onUpdate;
        private event UnityAction onLateUpdate;
        private event UnityAction onFixedUpdate;
#pragma warning restore 67

        private UpdateProviderEventHandler _updateProviderEventHandler;

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView{get;set;}

        [PostConstruct]
        public void PostConstruct()
        {
            GameObject newUpdater = new GameObject("Updater["+ contextView.name + "]");
            newUpdater.transform.SetParent(contextView.transform);
            _updateProviderEventHandler = newUpdater.AddComponent<UpdateProviderEventHandler>();
        }

        [Deconstruct]
        public void Deconstruct()
        {
            GameObject.Destroy(_updateProviderEventHandler.gameObject);
        }

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return _updateProviderEventHandler.StartCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            _updateProviderEventHandler.StopCoroutine(coroutine);
        }

        public void StopAllCoroutines()
        {
            _updateProviderEventHandler.StopAllCoroutines();
        }

        public void AddUpdate(UnityAction action)
        {
            _updateProviderEventHandler.onUpdate += action;
        }

        public void RemoveUpdate(UnityAction action)
        {
            _updateProviderEventHandler.onUpdate -= action;
        }

        public void AddFixedUpdate(UnityAction action)
        {
            _updateProviderEventHandler.onFixedUpdate += action;
        }

        public void RemoveFixedUpdate(UnityAction action)
        {
            _updateProviderEventHandler.onFixedUpdate -= action;
        }

        public void AddLateUpdate(UnityAction action)
        {
            _updateProviderEventHandler.onLateUpdate += action;
        }

        public void RemoveLateUpdate(UnityAction action)
        {
            _updateProviderEventHandler.onLateUpdate -= action;
        }
    }
}