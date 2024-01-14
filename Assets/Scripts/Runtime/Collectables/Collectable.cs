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

        public void Collect(Transform target)
        {
            if (isBeingCollected) return;
            _target = target;
            isBeingCollected = true;
            //_returnCoroutine = StartCoroutine(ReturnMoveLoop());
        }

        private void Update()
        {
            if (!isBeingCollected) return;
            
            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * moveSpeed*speedMult);
            var distance = Vector3.Distance(transform.position, _target.position);

            if (distance < 0.5f)
            {
                FinishedCollecting();
            }

            speedMult += Time.deltaTime * 3f;
        }

        private IEnumerator ReturnMoveLoop()
        {
            var isCloseToTarget = false;
            do
            {
                transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * moveSpeed);
                var distance = Vector3.Distance(transform.position, _target.position);

                if (distance < 0.5f)
                    isCloseToTarget = true;


                yield return null;
            } while (!isCloseToTarget);

            FinishedCollecting();
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