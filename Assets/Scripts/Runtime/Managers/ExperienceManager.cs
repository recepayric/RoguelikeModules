using System;
using UnityEngine;

namespace Runtime.Managers
{
    public class ExperienceManager : MonoBehaviour
    {
        #region InstanceReg

        public static ExperienceManager instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        //private float _collectedOrbAmount;
    }
}