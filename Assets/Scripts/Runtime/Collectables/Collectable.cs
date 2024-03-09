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
        private Coroutine _returnCoroutine;
        private float value = 1;
        private float speedMult = 1;
        private float backSpeedMult = -1;
        private float backSpeedMultTime = 0.1f;

        public void Collect(Transform target)
        {
            if (isBeingCollected) return;
            _target = target;
            isBeingCollected = true;
            backSpeedMult = -1;
            //_returnCoroutine = StartCoroutine(ReturnMoveLoop());
        }

        private void Update()
        {
            if (!isBeingCollected) return;
            UpdateBackspeed();

            var speedMlt = moveSpeed * speedMult * backSpeedMult;
            
            var goAngle = _target.transform.position - transform.position;
            var deltaTravel = Time.deltaTime * speedMlt * goAngle;
            transform.position += deltaTravel;
            
            //transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * speedMlt);
            var distance = Vector3.Distance(transform.position, _target.position);

            if (distance < 0.5f)
            {
                FinishedCollecting();
            }

            speedMult += Time.deltaTime * 10f;
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