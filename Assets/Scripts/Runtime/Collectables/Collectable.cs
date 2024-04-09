using System;
using System.Collections;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Collectables
{
    public class Collectable : MonoBehaviour, IPoolObject
    {
        public CollectableTypes collectableType;
        public float moveSpeed;
        public bool isBeingCollected = false;
        private Transform _target;
        private Vector3 _targetTemp;
        private Coroutine _returnCoroutine;
        private float value = 1;
        private float speedMult = 1;
        public float speedMultMax = 1;
        private float backSpeedMult = -1;
        private float backSpeedMultTime = 0.1f;

        private float collectPercentage;
        private float timer;
        public float collectTime;

        public void Collect(Transform target)
        {
            if (isBeingCollected) return;
            _target = target;
            _targetTemp = _target.transform.position;
            _targetTemp.y = transform.position.y;
            isBeingCollected = true;
            backSpeedMult = -2;
            //_returnCoroutine = StartCoroutine(ReturnMoveLoop());
            timer = 0;

        }


        private void Update()
        {
            if (!isBeingCollected) return;
            UpdateBackspeed();

            var speedMlt = moveSpeed * speedMult * backSpeedMult;
            var goAngle = _target.transform.position - transform.position;
            
            if (speedMlt < 0)
            {
                goAngle = _targetTemp - transform.position;

                timer += Time.deltaTime;
                collectPercentage = timer / collectTime;
            }
            
            var deltaTravel = Time.deltaTime * speedMlt * goAngle;
            transform.position += deltaTravel;
            
            //transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * speedMlt);
            var distance = Vector3.Distance(transform.position, _target.position);

            

            speedMult += Time.deltaTime * 20f;

            if (speedMult > speedMultMax)
            {
                speedMult = speedMultMax;
                Debug.Log("Speed multi is at max: " + speedMult);
            }
        }

        private void UpdateBackspeed()
        {
            backSpeedMult += Time.deltaTime / backSpeedMultTime;
            if (backSpeedMult > 1)
                backSpeedMult = 1;
        }

        private void FinishedCollecting()
        {
            CollectableManager.instance.OnCollected(this);
            EventManager.Instance.OrbCollected(value);
            BasicPool.instance.Return(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            FinishedCollecting();
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            isBeingCollected = false;
            speedMult = 1f;
            DictionaryHolder.Collectables.Remove(gameObject);
        }

        public void OnGet()
        {
            DictionaryHolder.Collectables.Add(gameObject, this);
        }
    }
}