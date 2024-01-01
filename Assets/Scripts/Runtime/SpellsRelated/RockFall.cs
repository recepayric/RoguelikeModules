using System.Collections.Generic;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Modifiers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.SpellsRelated
{
    public class RockFall : MonoBehaviour, ICastable
    {
        public Vector3 targetPosition;

        public GameObject shadowObject;
        public GameObject rockObject;

        public float animationTime;
        public float rockMoveUpAmount;

        public Vector3 shadowInitialScale;
        public Vector3 rockInitialScale;

        public Ease ease;

        public PoolKeys projectilePoolKey;
        public List<Modifier> modifiers;


        public bool FollowsOwner { get; set; }

        [Button]
        public void Cast()
        {
            PlaceShadow();
            StartRockFalling();
        }

        public void Activate()
        {
            
        }

        public void Prepare()
        {
            
        }

        public void SetPosition(Vector3 targetPosition)
        {
        }

        public void SetOwner(Owners owner)
        {
        }

        [Button]
        public void Prepare(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
            transform.position = this.targetPosition;

            shadowObject.SetActive(true);
            rockObject.SetActive(true);
            
            shadowInitialScale = shadowObject.transform.localScale;
            rockInitialScale = rockObject.transform.localScale;

            rockObject.transform.localPosition = new Vector3(0, rockMoveUpAmount, 0);
        }

        private void PlaceShadow()
        {
            shadowObject.transform.position = targetPosition;
            shadowObject.transform.localScale = Vector3.zero;

            shadowObject.transform.DOScale(shadowInitialScale, animationTime).SetEase(ease);
        }

        private void StartRockFalling()
        {
            rockObject.transform.DOLocalMove(Vector3.zero, animationTime).SetEase(ease).OnComplete(() =>
            {
                CheckForCollision();
                ShatterRock();
                CreateRocks();
            });
        }

        private void CheckForCollision()
        {
            var explosion = BasicPool.instance.Get(PoolKeys.Explosion1);
            explosion.transform.position = rockObject.transform.position;
            ScriptDictionaryHolder.Explosions[explosion].SetSize(5);
            ScriptDictionaryHolder.Explosions[explosion].Explode();
        }

        private void CreateRocks()
        {
            for (int i = 0; i < 10; i++)
            {
                var projectile = BasicPool.instance.Get(projectilePoolKey);
                projectile.transform.position = rockObject.transform.position + rockObject.transform.up;
                projectile.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                //projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                var sc = ScriptDictionaryHolder.Projectiles[projectile];
                sc.SetMaxDistance(400f);
                sc.SetModifiers(modifiers);
            }
            
            //sc.ignoredEnemy = ignoredEnemy;
        }

        private void ShatterRock()
        {
            rockObject.SetActive(false);
            shadowObject.SetActive(false);
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }
    }
}