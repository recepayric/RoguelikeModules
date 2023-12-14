using System;
using Data;
using Runtime.Collectables;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Managers
{
    public class CollectableManager : MonoBehaviour
    {
        #region instance

        public static CollectableManager instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion
        
        
        [SerializeField] private CurrencyDataSo _currencyDataSo;

        void Start()
        {
            _currencyDataSo = Resources.Load<CurrencyDataSo>("CollectableData");
        }

        private Collectable _collected;

        public void OnCollected(Collectable collectable)
        {
            _collected = collectable;

            switch (_collected.collectableType)
            {
                case CollectableTypes.Orb:
                    CollectOrb();
                    break;
                case CollectableTypes.Food:
                    CollectFood();
                    break;
                case CollectableTypes.Chest:
                    CollectChest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CollectOrb()
        {
            //todo amount change depends on if we have remaining ones from previous floor.
            _currencyDataSo.AddCollectable(CollectableTypes.Orb, 1);
            EventManager.Instance.UpdateResCount();
        }

        private void CollectChest()
        {
        }

        private void CollectFood()
        {
        }
    }
}