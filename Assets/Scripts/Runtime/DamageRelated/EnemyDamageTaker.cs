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
        public List<Renderer> renderersArmor;
        public List<Renderer> renderersWeapon;
        public MaterialPropertyBlock PropertyBlock;
        public MaterialPropertyBlock PropertyBlockArmor;
        public MaterialPropertyBlock PropertyBlockWeapon;
        public float colorChangeAmount;
        public bool tryAdd;
        public Texture texture;
        public Texture armorTexture;
        public Texture weaponTexture;

        public Animator animator;

        private void Start()
        {
            PropertyBlock = new MaterialPropertyBlock();
            PropertyBlockArmor = new MaterialPropertyBlock();
            PropertyBlockWeapon = new MaterialPropertyBlock();
            PropertyBlock.SetTexture("_MainTex", texture);
            
            if(armorTexture != null )
                PropertyBlockArmor.SetTexture("_MainTex", armorTexture);
            
            if(weaponTexture != null)
                PropertyBlockWeapon.SetTexture("_MainTex", weaponTexture);
            
            
            for (int i = 0; i < renderers.Count; i++)
            {
                Debug.Log("Set Property Block!!");
                renderers[i].SetPropertyBlock(PropertyBlock);
            }
            
            for (int i = 0; i < renderersArmor.Count; i++)
            {
                renderersArmor[i].SetPropertyBlock(PropertyBlockArmor);
            }
            
            for (int i = 0; i < renderersWeapon.Count; i++)
            {
                renderersWeapon[i].SetPropertyBlock(PropertyBlockWeapon);
            }
        }

        public void DamageTaken()
        {
            //animator.SetFloat("HitSpeed", 1/AnimationConfig.DamageTakenColorChangeTime);
            animator.SetTrigger("GetHit");
            
            var clip = FindAnimation(animator, "hit");
            var speed = clip.length / AnimationConfig.DamageTakenColorChangeTime*2;
            animator.SetFloat("HitSpeed", speed);
            
            colorChangeAmount = 1;
            for (int i = 0; i < renderers.Count; i++)
            {
                PropertyBlock.SetFloat("_HitValue", colorChangeAmount);
                renderers[i].SetPropertyBlock(PropertyBlock);
            }
            
            
            for (int i = 0; i < renderersArmor.Count; i++)
            {
                PropertyBlockArmor.SetFloat("_HitValue", colorChangeAmount);
                renderersArmor[i].SetPropertyBlock(PropertyBlockArmor);
            }
            
            for (int i = 0; i < renderersWeapon.Count; i++)
            {
                PropertyBlock.SetFloat("_HitValue", colorChangeAmount);
                renderersWeapon[i].SetPropertyBlock(PropertyBlock);
            }
        }

        private void Update()
        {
            if (colorChangeAmount <= 0) return;

            colorChangeAmount -= Time.deltaTime / AnimationConfig.DamageTakenColorChangeTime;
            
            if (colorChangeAmount < 0)
                colorChangeAmount = 0;
            
            PropertyBlock.SetFloat("_HitValue", colorChangeAmount);
            PropertyBlockArmor.SetFloat("_HitValue", colorChangeAmount);
            PropertyBlockWeapon.SetFloat("_HitValue", colorChangeAmount);
            if (tryAdd)
            {
                for (int i = 0; i < renderers.Count; i++)
                {
                    renderers[i].SetPropertyBlock(PropertyBlock);
                }
                
                for (int i = 0; i < renderersArmor.Count; i++)
                {
                    renderersArmor[i].SetPropertyBlock(PropertyBlockArmor);
                }
                
                for (int i = 0; i < renderersWeapon.Count; i++)
                {
                    renderersWeapon[i].SetPropertyBlock(PropertyBlockWeapon);
                }
            }
        }
        
        public AnimationClip FindAnimation (Animator animator, string name) 
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == name)
                {
                    return clip;
                }
            }

            return null;
        }
    }
}