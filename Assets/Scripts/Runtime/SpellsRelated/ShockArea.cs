using System.Collections.Generic;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.SpellsRelated
{
    public class ShockArea : Spell
    {
        public Player playerScript;
        public List<GameObject> enemiesInRange;
        public float baseRange;
        public Vector3 baseScale;
        public Vector3 targetScale;

        public bool isActive = false;
        public float activateTime;
        public float deactivateTime;

        public override void Cast()
        {
            
        }

        public override void Activate()
        {
            isActive = true;
        }

        public override void DeActivate()
        {
            base.DeActivate();
            transform.DOScale(Vector3.zero, deactivateTime);

            isActive = false;
        }

        public override void Prepare()
        {
            UpdateSize();
            transform.DOScale(targetScale, activateTime);

            isActive = true;
        }

        public override void StartSpell()
        {
            base.StartSpell();
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

            if (!isActive) return;
            
            CheckEnemies();
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                DictionaryHolder.Damageables[enemiesInRange[i]].AddElementalAilment(ElementModifiers.Lightning, 1, 10);
            }
        }

        private void CheckEnemies()
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
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
            if (other.CompareTag("Enemy"))
            {
                enemiesInRange.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemiesInRange.Remove(other.gameObject);
            }
        }
    }
}