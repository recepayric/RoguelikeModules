using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class Enemy : MonoBehaviour, IPoolObject
    {
        //Ailments
        [Header("Ailment Objects")] public GameObject burnAilmentObject;
        public GameObject freezeAilmentObject;
        public GameObject shockAilmentObject;

        [Header("Ailments")] public bool isBurning;
        public bool isFrozen;
        public bool isShocked;

        //Ailment Times
        [Header("Ailment Times")] public float burnTime;
        public float freezeTime;
        public float shockTime;

        //Ailment Effects
        [Header("Ailment Effects")] 
        public float burningDamagePerSecond;
        public float freezeEffect;
        public float shockEffect;

        public float burningTimer;
        
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

        public float moveSpeed = 5;
        // Update is called once per frame
        void Update()
        {
            UpdateAilments();
            
            //transform.position = Vector3.Lerp(transform.position, ScriptDictionaryHolder.Player.transform.position, Time.deltaTime*moveSpeed);
        }

        private void UpdateAilments()
        {
            if (burnTime > 0)
            {
                burningTimer += Time.deltaTime;
                if (burningTimer >= 1)
                {
                    burningTimer -= 1;
                    GetHit((int)burningDamagePerSecond, false);
                }
                burnTime -= Time.deltaTime;
                if (burnTime <= 0)
                {
                    isBurning = false;
                    FinishBurn();
                }
            }


            if (freezeTime > 0)
            {
                freezeTime -= Time.deltaTime;
                if (freezeTime <= 0)
                {
                    isFrozen = false;
                    FinishFreeze();
                }
            }


            if (shockTime > 0)
            {
                shockTime -= Time.deltaTime;
                if (shockTime <= 0)
                {
                    isShocked = false;
                    FinishShock();
                }
            }
        }

        #region Burn
        public void AddBurning(float burnTimeToAdd, float burningDamage)
        {
            burningDamagePerSecond = burningDamage;
            burnTime = burnTimeToAdd;
            burnAilmentObject.SetActive(true);
        }

        private void FinishBurn()
        {
            burningTimer = 1;
            burningDamagePerSecond = 0;
            burnAilmentObject.SetActive(false);
        }
        #endregion
        
        #region Freeze
        public void AddFreeze(float freezeTimeToAdd, float freezeEffect)
        {
            freezeTime = freezeTimeToAdd;
            this.freezeEffect = freezeEffect;
            freezeAilmentObject.SetActive(true);
        }
        private void FinishFreeze()
        {
            freezeEffect = 0;
            freezeAilmentObject.SetActive(false);
        }
        #endregion

        #region Shock

        public void AddShock(float shockTimeToAdd, float shockEffect)
        {
            shockTime = shockTimeToAdd;
            this.shockEffect = shockEffect;
            shockAilmentObject.SetActive(true);
        }
        private void FinishShock()
        {
            shockEffect = 0;
            shockAilmentObject.SetActive(false);
        }

        #endregion

        
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
            if (col.CompareTag("Projectile"))
            {
                col.GetComponent<Projectile>().HitTarget(this);
            }
        }

        public void GetHit(int damage, bool isCriticalHit)
        {
            UIController.instance.AddDamageText(gameObject, damage, isCriticalHit);

            if (!isImmortal)
            {
                //BasicPool.instance.Return(gameObject);
                Die();
            }
        }

        private void Die()
        {
            //ItemDropManager.instance.DropItemFromEnemy(this);
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