using DG.Tweening;
using MVC.Base.Runtime.Abstract.View;
using Runtime.Data.ValueObject;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Views.Global.Health
{
    public class HealthView : MVCView
    {
        private GameObject _cameraObject;
        public event UnityAction<float, float> UpdateHealthEvent;
        [ShowInInspector] private float healthRegenPerSecond;
        public HealthVO healthVo;
        public bool hasHealthBarUI = false;
        public bool updateUI = false;
        public bool isImmortal = false;
        public bool isBoss;

        public bool canRegenHealth = false;
        public float regenUpdateUITime = 0.25f; // Time interval for UI updates
        private float timer = 0f;

        public GameObject healthBarObject;
        public Renderer fillBarRenderer;
        private MaterialPropertyBlock _fillBarPropertyBlock;

        public Vector3 initialHealthBarScale;
        public float healthBarHitScaleAmount;
        public float healthBarHitScaleTime;

        private string _healthBarScaleID;
        private bool _isHealthBarNull = false;
        private bool _isHealthBarActivated = false;

        public void Init()
        {
            _fillBarPropertyBlock ??= new MaterialPropertyBlock();

            if (healthBarObject == null)
                return;

            if (_cameraObject == null)
                _cameraObject = Camera.main.gameObject;
            
            if(hasHealthBarUI)
                healthBarObject.SetActive(false);

            if (initialHealthBarScale == Vector3.zero)
                initialHealthBarScale = healthBarObject.transform.localScale;

            _healthBarScaleID = GetInstanceID() + "HealthBarScale";
        }

        public void DamageTaken()
        {
            if(hasHealthBarUI)
                healthBarObject.SetActive(true);
            
            if (healthBarObject == null) return;
            DOTween.Kill(_healthBarScaleID);

            var targetScale = initialHealthBarScale * healthBarHitScaleAmount;

            healthBarObject.transform.DOScale(targetScale, healthBarHitScaleTime/2).SetLoops(2, LoopType.Yoyo).SetId(_healthBarScaleID);

            var healthPercentage = healthVo.currentHealth / healthVo.maxHealth;
            
            _fillBarPropertyBlock.SetFloat("_FillAmount", healthPercentage);
            fillBarRenderer.SetPropertyBlock(_fillBarPropertyBlock);
            
            if(healthPercentage <= 0)
                healthBarObject.SetActive(false);
        }

        public void CalculateHpRegenPerSecond()
        {
            healthRegenPerSecond = 0.25f * Mathf.Pow(healthVo.healthRegen, 0.9f)/5;
            if (float.IsNaN(healthRegenPerSecond))
                healthRegenPerSecond = 0;
        }

        public void StartHealthRegen()
        {
            canRegenHealth = true;
            timer = 0;
        }

        public void StopHealthRegen()
        {
            canRegenHealth = false;
        }

        public void Heal(float healAmount)
        {
            healthVo.RegenHealth(healAmount);
            if (updateUI) UpdateHealthEvent?.Invoke(healthVo.currentHealth, healthVo.maxHealth);
        }

        [Button]
        public void DealDamage()
        {
            healthVo.DealDamage(1f);
            
            if (updateUI) UpdateHealthEvent?.Invoke(healthVo.currentHealth, healthVo.maxHealth);
        }

        void Update()
        {
            if (healthBarObject != null)
                healthBarObject.transform.forward = _cameraObject.transform.forward;
            
            if (canRegenHealth)
            {
                var healthRegen = healthRegenPerSecond * Time.deltaTime;
                healthVo.RegenHealth(healthRegen);
            }

            if (!updateUI) return;

            timer += Time.deltaTime;
            if (timer >= regenUpdateUITime)
            {
                timer = 0f;
                healthVo.HealthUpdated();
                UpdateHealthEvent?.Invoke(healthVo.currentHealth, healthVo.maxHealth);
            }
        }
    }
}