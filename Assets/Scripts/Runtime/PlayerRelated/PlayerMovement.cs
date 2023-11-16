using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.PlayerRelated
{
    public class PlayerMovement : MonoBehaviour
    {
        private Player _playerScript;
        private Stats _stats;


        [SerializeField] private float _moveSpeed;
        public float baseMoveSpeed = 1;
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private float _moveX;
        [SerializeField] private float _moveY;
        [SerializeField] private float _deltaX;
        [SerializeField] private float _deltaY;

        public bool isTargetingEnemy = false;

        private void Start()
        {
            _playerScript = gameObject.GetComponent<Player>();
            _stats = _playerScript.stats;
            _targetPosition = Vector3.zero;
            UpdateStats();
        }

        [Button]
        private void UpdateStats()
        {
            _moveSpeed = baseMoveSpeed + baseMoveSpeed*(_stats.moveSpeed/100f);
        }

        private void Update()
        {
            _moveX = Input.GetAxisRaw("Horizontal");
            _moveY = Input.GetAxisRaw("Vertical");

            

            //todo if goes to both axis, reduce the speed

            _deltaX = Time.deltaTime * _moveSpeed * _moveX;
            _deltaY = Time.deltaTime * _moveSpeed * _moveY;

            _targetPosition.x = transform.position.x + _deltaX;
            _targetPosition.y = transform.position.y + _deltaY;

            transform.position = _targetPosition;

            if (_moveX > 0)
                FaceTowards(1);
            else
                FaceTowards(-1);

        }

        private void FaceTowards(int side)
        {
            if(isTargetingEnemy)
                return;
            //-1 left, stay original.
            //1 right, turn.

            var scaleX = transform.localScale.x;
            var scaleY= transform.localScale.y;
            var scaleZ = transform.localScale.z;

            if (side == -1)
                scaleX = scaleX < 0 ? -scaleX : scaleX;
            else
                scaleX = scaleX > 0 ? -scaleX : scaleX;

            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        }
    }
}