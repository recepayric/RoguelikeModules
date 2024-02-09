using System.Collections.Generic;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.SpellsRelated.Aura
{
    public class EnemyBuffSpell : Spell
    {
        public string spellScaleID;
        public Player playerScript;
        public List<GameObject> enemiesInRange;
        public Vector3 baseScale;
        public Vector3 targetScale;

        public bool isActive = false;
        public float activateTime;
        public float deactivateTime;
        public AllStats statToBuff;
        public float statIncreasePercentage;

        private AllStats[] _statsArray = new[]
            { AllStats.MaxHealth, AllStats.Damage, AllStats.Range, AllStats.AttackSpeed };

        public override void Cast()
        {
            
        }

        private void SetRandomStat()
        {
            statToBuff = _statsArray[Random.Range(0, _statsArray.Length)];
        }

        public override void Activate()
        {
            if (isActive)
                return;


            Prepare();
            isActive = true;
            SetRandomStat();
        }

        public override void DeActivate()
        {
            base.DeActivate();
            DOTween.Kill(spellScaleID);
            RemoveAllBuffs();
            transform.DOScale(Vector3.zero, deactivateTime).OnComplete(() =>
            {
                BasicPool.instance.Return(gameObject);
            }).SetId(spellScaleID);

            isActive = false;
        }

        public override void Prepare()
        {
            DOTween.Kill(spellScaleID);
            UpdateSize();
            transform.DOScale(targetScale, activateTime).SetId(spellScaleID);

            isActive = true;
        }

        public override void StartSpell()
        {
            base.StartSpell();
            spellScaleID = gameObject.GetInstanceID() + "SpellScale";
            
            //Todo based on type
            Activate();
        }

        public override void StopSpell()
        {
            base.StopSpell();
            DeActivate();
        }

        public override void SetPosition(Vector3 targetPosition)
        {
        }

        private void UpdateSize()
        {
            var radius = 0f;
            if (OwnerScript != null)
                radius = OwnerScript.GetRange();

            var radToScale = radius / GameConfig.RangeToRadius;
            var scaleForY = radToScale * (baseScale.y / baseScale.x);

            var newScale = new Vector3(baseScale.x + radToScale, baseScale.y + scaleForY, baseScale.z + radToScale);
            //transform.localScale = newScale;
            targetScale = newScale;
        }

        public void Update()
        {
            if (FollowsOwner)
            {
                transform.position = OwnerScript.GetSpellPosition();
            }

            CheckEnemies();
        }

        public void ApplyBuff(GameObject gameObject)
        {
            if (gameObject == OwnerScript.GetGameObject())
                return;

            DictionaryHolder.Enemies[gameObject]._stats.AddStatBuff(statToBuff, statIncreasePercentage);
        }

        public void RemoveBuff(GameObject gameObject)
        {
            if (gameObject == OwnerScript.GetGameObject())
                return;

            if (gameObject == null)
                return;
            if (!gameObject.activeSelf)
                return;
            
            DictionaryHolder.Enemies[gameObject]._stats.RemoveBuff(statToBuff);
        }

        public void RemoveAllBuffs()
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if(!DictionaryHolder.Enemies.ContainsKey(enemiesInRange[i]))
                    continue;
                DictionaryHolder.Enemies[enemiesInRange[i]]._stats.RemoveBuff(statToBuff);
            }
        }

        private void CheckEnemies()
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (!DictionaryHolder.Enemies.ContainsKey(enemiesInRange[i]))
                {
                    enemiesInRange.Remove(enemiesInRange[i]);
                    i--;
                    continue;
                }
                if (!DictionaryHolder.Enemies[enemiesInRange[i]].IsAvailable())
                {
                    enemiesInRange.Remove(enemiesInRange[i]);
                    i--;
                }
            }
        }

        [Button]
        private void SetSize(int radius)
        {
            var radToScale = radius / GameConfig.RangeToRadius;
            var scaleForY = radToScale * (baseScale.y / baseScale.x);

            var newScale = new Vector3(baseScale.x + radToScale, baseScale.y + scaleForY, baseScale.z + radToScale);
            transform.localScale = newScale;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!isActive)
                return;
            //Debug.Log(other.name);
            if (other.CompareTag("Enemy"))
            {
                if (!DictionaryHolder.Enemies[other.gameObject].IsAvailable()) return;
                enemiesInRange.Add(other.gameObject);
                ApplyBuff(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!isActive)
                return;
            //Debug.Log("Left: " + other.name);
            if (other.CompareTag("Enemy"))
            {
                enemiesInRange.Remove(other.gameObject);
                RemoveBuff(other.gameObject);
            }
        }
    }
}