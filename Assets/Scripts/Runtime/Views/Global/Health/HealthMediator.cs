using Runtime.Enums;
using Runtime.Model;
using Runtime.Model.Interfaces;
using Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.Global.Health
{
    public class HealthMediator : Mediator
    {
        [Inject] public HealthView view { get; set; }
        [Inject] public IStatModel StatModel { get; set; }
        [Inject] public ILoaderModel LoaderModel { get; set; }
        [Inject] public IHealthModel HealthModel { get; set; }
        [Inject] public UISignals UISignals { get; set; }
        [Inject] public EntitySignals EntitySignals { get; set; }

        public void OnStatsChanged()
        {
            if (StatModel.GetStats(gameObject) != null)
                SetStats();
        }

        public void Initialize()
        {
            if (StatModel.GetStats(gameObject) != null)
            {
                SetStats(true);
            }

            
        }

        private void SetStats(bool isInitial = false)
        {
            if (isInitial)
                view.healthVo.damageTaken = 0;

            var previousMaxHealth = view.healthVo.maxHealth;
            view.healthVo.maxHealth = StatModel.Stats[gameObject].GetStat(AllStats.MaxHealth);

            if (view.healthVo.maxHealth < previousMaxHealth)
            {
                var difference = previousMaxHealth -  view.healthVo.maxHealth;
                view.healthVo.damageTaken -= difference;
                if (view.healthVo.damageTaken < 0)
                    view.healthVo.damageTaken = 0;
            }
            
            view.healthVo.currentHealth = view.healthVo.maxHealth - view.healthVo.damageTaken;
            
            view.healthVo.healthRegen = StatModel.Stats[gameObject].GetStat(AllStats.HealthRegen);
            
            if(isInitial)
                StatModel.Stats[gameObject].StatsChangedEvent += OnStatsChanged;

            if (view.isImmortal)
            {
                view.healthVo.maxHealth = 999999;
                view.healthVo.currentHealth = 999999;
            }
            
            view.CalculateHpRegenPerSecond();
        }

        public void Destroy()
        {
            if (StatModel.GetStats(gameObject) != null)
                StatModel.Stats[gameObject].StatsChangedEvent -= OnStatsChanged;
        }

        private void OnHealthUpdated()
        {
            //view.currentHealth = view.healthVo.currentHealth;
        }

        private void OnStartHealthRegen()
        {
            view.StartHealthRegen();
        }

        private void OnStopHealthRegen()
        {
            view.StopHealthRegen();
        }

        private void OnUpdateHealth(float currentHealth, float maxHealth)
        {
            if(view.isBoss)
                UISignals.UpdateEnemyHealthUISignal.Dispatch(currentHealth, maxHealth);
        }

        private void OnInitializeHealth(GameObject entityObject)
        {
            if (gameObject != entityObject) return;
            Initialize();
        }

        private void OnHealPlayer(GameObject entityObject, float healAmount)
        {
            if (gameObject != entityObject) return;
            view.Heal(healAmount);
        }

        private void OnDamageTaken()
        {
            view.DamageTaken();
        }
        
        public override void OnRegister()
        {
            base.OnRegister();
            LoaderModel.GetLoader(gameObject).InitializeMediatorsEvent += Initialize;
            LoaderModel.GetLoader(gameObject).DestroyMediatorsEvent += Destroy;

            view.healthVo.HealthUpdatedEvent += OnHealthUpdated;
            view.healthVo.StartHealthRegenEvent += OnStartHealthRegen;
            view.healthVo.StopHealthRegenEvent += OnStopHealthRegen;
            view.healthVo.DamageTakenEvent += OnDamageTaken;

            view.UpdateHealthEvent += OnUpdateHealth;
            
            HealthModel.RegisterHealth(gameObject, view.healthVo);
            EntitySignals.InitializeHealth.AddListener(OnInitializeHealth);
            EntitySignals.HealPlayerSignal.AddListener(OnHealPlayer);
            
            view.Init();
        }

        public override void OnRemove()
        {
            base.OnRemove();
            LoaderModel.GetLoader(gameObject).InitializeMediatorsEvent -= Initialize;
            LoaderModel.GetLoader(gameObject).DestroyMediatorsEvent -= Destroy;
            
            view.healthVo.HealthUpdatedEvent -= OnHealthUpdated;
            view.healthVo.StartHealthRegenEvent -= OnStartHealthRegen;
            view.healthVo.StopHealthRegenEvent -= OnStopHealthRegen;
            view.healthVo.DamageTakenEvent -= OnDamageTaken;

            
            view.UpdateHealthEvent -= OnUpdateHealth;
            
            HealthModel.RemoveHealth(gameObject);
            EntitySignals.InitializeHealth.RemoveListener(OnInitializeHealth);
            EntitySignals.HealPlayerSignal.RemoveListener(OnHealPlayer);

        }
    }
}