using System;
using System.Collections.Generic;
using Runtime.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.DamageRelated
{
    public class EnemyDamageTaker : MonoBehaviour
    {
        public List<Renderer> renderers;
        public MaterialPropertyBlock PropertyBlock;
        public float colorChangeAmount;
        public bool tryAdd;

        public Animator animator;

        private void Start()
        {
            PropertyBlock = new MaterialPropertyBlock();
        }

        public void DamageTaken()
        {
            animator.SetFloat("HitSpeed", 1/AnimationConfig.DamageTakenColorChangeTime);
            animator.SetTrigger("GetHit");
            colorChangeAmount = 1;
            for (int i = 0; i < renderers.Count; i++)
            {
                PropertyBlock.SetFloat("_HitValue", colorChangeAmount);
                renderers[i].SetPropertyBlock(PropertyBlock);
            }
        }

        private void Update()
        {
            if (colorChangeAmount <= 0) return;

            colorChangeAmount -= Time.deltaTime / AnimationConfig.DamageTakenColorChangeTime;
            
            if (colorChangeAmount < 0)
                colorChangeAmount = 0;
            
            PropertyBlock.SetFloat("_HitValue", colorChangeAmount);
            if (tryAdd)
            {
                for (int i = 0; i < renderers.Count; i++)
                {
                    renderers[i].SetPropertyBlock(PropertyBlock);
                }
            }
        }
    }
}