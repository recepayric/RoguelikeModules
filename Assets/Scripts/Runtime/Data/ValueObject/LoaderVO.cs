using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class LoaderVO
    {
        public GameObject gameObject;
        public List<Mediator> mediator;
        public event UnityAction InitializeMediatorsEvent;
        public event UnityAction DestroyMediatorsEvent;

        public void Initialize()
        {
            InitializeMediatorsEvent?.Invoke();
        }

        public void Destroy()
        {
            DestroyMediatorsEvent?.Invoke();
        }
    }
}