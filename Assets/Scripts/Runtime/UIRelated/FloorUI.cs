using System;
using Runtime.Managers;
using TMPro;
using UnityEngine;

namespace Runtime.UIRelated
{
    public class FloorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textFloorNum;
        [SerializeField] private TextMeshProUGUI textTimer;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnUpdateFloorNumber(int floorNum)
        {
            textFloorNum.text = "Floor " + floorNum.ToString();
        }

        private void OnUpdateRemainingTime(int remainingTime)
        {
            textTimer.text = remainingTime.ToString();
        }

        private void OnEnable()
        {
            EventManager.Instance.UpdateFloorNumberEvent += OnUpdateFloorNumber;
            EventManager.Instance.UpdateFloorTimerEvent += OnUpdateRemainingTime;
        }

        private void OnDisable()
        {
            EventManager.Instance.UpdateFloorNumberEvent -= OnUpdateFloorNumber;
            EventManager.Instance.UpdateFloorTimerEvent -= OnUpdateRemainingTime;
        }
    }
}