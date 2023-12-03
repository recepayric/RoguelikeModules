using System;
using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Runtime.Managers;
using TMPro;
using UnityEngine;

namespace Runtime.UIRelated
{
    public class DemoDebugUI : MonoBehaviour
    {
        [SerializeField] private CurrencyDataSo _currencyDataSo;

        public int lastXFrameCalculate;
        private List<float> fpss = new List<float>();

        private void Start()
        {
            Debug.Log("Loading");
            _currencyDataSo = Resources.Load<CurrencyDataSo>("CollectableData");
        }

        public TextMeshProUGUI monsterSpawnStatus;
        public TextMeshProUGUI closestMonsterDistance;
        public TextMeshProUGUI textCollectedItem;
        public TextMeshProUGUI textFPS;

        private void Update()
        {
            textFPS.text = CalculateFPS();
        }

        public void StartFloor()
        {
            EventManager.Instance.FloorLoad();
        }
        
        
        private string CalculateFPS()
        {
            var fps = 1f / Time.unscaledDeltaTime;
            if (fpss.Count > lastXFrameCalculate)
            {
                fpss.RemoveAt(0);
            }

            fpss.Add(fps);


            var total = 0f;
            for (int i = 0; i < fpss.Count; i++)
            {
                total += fpss[i];
            }

            return "" + (int)(total / fpss.Count);
        }

        private void OnUpdateItemCount()
        {
            textCollectedItem.text = "Resource: " + _currencyDataSo.collectables[CollectableTypes.Orb];
        }

        public void OnSetClosestMonsterDistance(float distance)
        {
            closestMonsterDistance.text = "distance: " + distance;
        }

        private void OnSetMonsterSpawning(bool status)
        {
            monsterSpawnStatus.text = "is spawning: " + status;
        }

        private void OnEnable()
        {
            EventManager.Instance.SetMonsterSpawning += OnSetMonsterSpawning;
            EventManager.Instance.UpdateResCountEvent += OnUpdateItemCount;
            EventManager.EventSetDistanceBetweenEnemy += OnSetClosestMonsterDistance;
        }

        private void OnDisable()
        {
            EventManager.Instance.SetMonsterSpawning -= OnSetMonsterSpawning;
            EventManager.Instance.UpdateResCountEvent -= OnUpdateItemCount;
            EventManager.EventSetDistanceBetweenEnemy -= OnSetClosestMonsterDistance;
        }
    }
}