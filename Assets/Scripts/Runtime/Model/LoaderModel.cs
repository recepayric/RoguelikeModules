using System.Collections.Generic;
using Runtime.Data.ValueObject;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Model
{
    public class LoaderModel : ILoaderModel
    {
        private Dictionary<GameObject, LoaderVO> _mediatorsToRegister = new Dictionary<GameObject, LoaderVO>();
        public Dictionary<GameObject, LoaderVO> MediatorsToRegister => _mediatorsToRegister;

        public LoaderVO GetLoader(GameObject gameObject)
        {
            if (!_mediatorsToRegister.ContainsKey(gameObject))
            {
                _mediatorsToRegister.Add(gameObject, new LoaderVO());
            }

            return _mediatorsToRegister[gameObject];
        }
        
        public void AddLoader(GameObject gameObject, Mediator mediator)
        {
            if (_mediatorsToRegister.ContainsKey(gameObject))
            {
                //_mediatorsToRegister[gameObject].mediator.Add(mediator);
                //_mediatorsToRegister[gameObject].InitializeMediatorsEvent
            }
            else
            {
                _mediatorsToRegister.Add(gameObject, new LoaderVO());
                _mediatorsToRegister[gameObject].mediator.Add(mediator);
            }
        }

        public void RemoveLoader(GameObject gameObject, Mediator mediator)
        {
            if (_mediatorsToRegister.ContainsKey(gameObject))
            {
                _mediatorsToRegister[gameObject].mediator.Remove(mediator);
            }
            else
            {
                _mediatorsToRegister.Add(gameObject, new LoaderVO());
            }
        }
    }
}