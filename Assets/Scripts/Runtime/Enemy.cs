using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class Enemy : MonoBehaviour, IPoolObject
    {
        public float xBound = 8.5f;
        public float yBound = 4.5f;

        public bool moveAround = true;
        public bool isImmortal = false;

        // Start is called before the first frame update
        void Start()
        {
            if (moveAround)
                StartRandomMoving();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void StartRandomMoving()
        {
            var randX = Random.Range(-xBound, xBound);
            var randY = Random.Range(-yBound, yBound);

            if (randX > transform.position.x)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                
                if (randX > transform.position.x)
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
            }

            transform.DOMove(new Vector3(randX, randY, 0), 3).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1, () => StartRandomMoving());
            });
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("Enter Area!!");
            if (col.CompareTag("Projectile"))
            {
                col.GetComponent<Projectile>().HitTarget(this);
            }
        }

        public void GetHit(int damage, bool isCriticalHit)
        {
            Debug.Log("Enemy Hit!");
            UIController.instance.AddDamageText(gameObject, 1, isCriticalHit);

            if (!isImmortal)
            {
                BasicPool.instance.Return(gameObject);
                Die();
            }
        }

        private void Die()
        {
            ItemDropManager.instance.DropItemFromEnemy(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            Debug.Log("Removing this enemy!");
            ScriptDictionaryHolder.Enemies.Remove(gameObject);
        }

        public void OnGet()
        {
            ScriptDictionaryHolder.Enemies.Add(gameObject, this);
        }
    }
}