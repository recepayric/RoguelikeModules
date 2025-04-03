using System.Collections.Generic;
using Runtime.Data.ValueObject;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Model
{
    public interface ILoaderModel
    {
        public Dictionary<GameObject, LoaderVO> MediatorsToRegister { get; }
        public void AddLoader(GameObject gameObject, Mediator mediator);
        public void RemoveLoader(GameObject gameObject, Mediator mediator);
        public LoaderVO GetLoader(GameObject gameObject);
    }
}