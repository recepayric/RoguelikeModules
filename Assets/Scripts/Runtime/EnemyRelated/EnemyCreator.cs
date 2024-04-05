using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.EnemyRelated
{
    public class EnemyCreator : MonoBehaviour
    {
        private Enemy _enemyScript;
        private Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;
        public Animator animator;
        public GameObject hitPoint;
        public GameObject projectilePoint;
        
        [Button]
        public void CreateEnemy()
        {
            AddRigidbodyAndCollider();

            SetupEnemy();
        }

        public void SetupEnemy()
        {
            _enemyScript = GetComponent<Enemy>();

            _enemyScript.boxCollider2D = _capsuleCollider;
            _enemyScript.animator = animator;
            _enemyScript._hitPoint = hitPoint;
            _enemyScript.projectilePoint = projectilePoint;

        }
        private void AddRigidbodyAndCollider()
        {
            var oldRigidBody = GetComponent<Rigidbody>();
            var oldCollider = GetComponent<CapsuleCollider>();

            if (oldRigidBody == null)
            {
                transform.AddComponent<Rigidbody>();
                _rigidbody = GetComponent<Rigidbody>();
            }
            
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            _rigidbody.constraints |= RigidbodyConstraints.FreezeRotationX;
            _rigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;
            _rigidbody.constraints |= RigidbodyConstraints.FreezeRotationZ;

            if (oldCollider == null)
            {
                transform.AddComponent<CapsuleCollider>();
                _capsuleCollider = GetComponent<CapsuleCollider>();
            }
        }
    }
}