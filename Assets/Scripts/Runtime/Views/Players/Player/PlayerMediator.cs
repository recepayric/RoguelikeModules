using MVC.Base.Runtime.Abstract.Model;
using Runtime.Enums;
using Runtime.Model;
using Runtime.Model.Interfaces;
using Runtime.Signals;
using Sirenix.OdinInspector;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.Players.Player
{
    public class PlayerMediator : Mediator
    {
        [Inject] public PlayerView view { get; set; }
        [Inject] public EntitySignals EntitySignals { get; set; }
        [Inject] public IGameModel GameModel { get; set; }
        [Inject] public IPlayerModel PlayerModel { get; set; }
        [Inject] public IStatModel StatModel { get; set; }
        [Inject] public IAimModel AimModel { get; set; }
        [Inject] public ILoaderModel LoaderModel { get; set; }

        private void Start()
        {
            Initialize();
        }

        private void OnDealDamageToTarget(GameObject targetObject, PoolKey poolKey, Vector3 hitPoint)
        {
            
        }

        [Button]
        public void ReturnToPool()
        {
        }

        private void OnStartAttack()
        {
            view.playerVo.StartAttack(AimModel.AimVos[gameObject].targetObject);
        }
        
        private void OnStopAttack()
        {
            view.playerVo.StopAttack();
        }
        
        private void OnSetPos(Vector3 pos)
        {
            transform.position = pos;
        }

        private void Initialize()
        {
            //initialize stats
            SetModel();
            view.playerVo.isDead = false;
            view.stats.initialize();
            view.stats.AddStats(GameModel.BasePlayerStats.playerBaseStats);
            view.stats.AddStats(view.playerData.playerExtraStats);
            view.playerVo.playerExperienceVo.Reset();
            EntitySignals.CalculateStatsSignal.Dispatch(gameObject);
            
            LoaderModel.GetLoader(gameObject).Initialize();
        }

        private void SetModel()
        {
            PlayerModel.ActivePlayer = gameObject;
            EntitySignals.InitializePlayerCameraSignal.Dispatch();
        }

        private void OnGetFromPool()
        {
        }

        private void OnReturnToPoolEvent()
        {
            if (PlayerModel.ActivePlayer == gameObject)
                PlayerModel.ActivePlayer = null;

            LoaderModel.GetLoader(gameObject).Destroy();
            
        }

        private void OnCheckForCollectables()
        {
            var baseCollectRange = view.stats.GetStat(AllStats.BaseCollectRange);
            var collectRangeIncrease = view.stats.GetStat(AllStats.CollectRangeIncrease);
            var collectRange = baseCollectRange + baseCollectRange * collectRangeIncrease / 100f;
            EntitySignals.CheckForCollectablesSignal.Dispatch(gameObject, collectRange);
        }

        public override void OnRegister()
        {
            base.OnRegister();
            PlayerModel.RegisterPlayer(gameObject, view.playerVo);
            StatModel.RegisterStats(gameObject, view.stats);
            
            view.aimVo = AimModel.RegisterAimVo(gameObject, view.aimVo);
            
            view.OnGetFromPoolEvent += OnGetFromPool;
            view.OnReturnToPoolEvent += OnReturnToPoolEvent;
            view.StartAttackEvent += OnStartAttack;
            view.StopAttackEvent += OnStopAttack;
            view.CheckForCollectablesEvent += OnCheckForCollectables;
            view.SpawnedEvent += OnGetFromPool;
            view.DealDamageEvent += OnDealDamageToTarget;

            view.playerVo.SetMoveEvent += OnSetPos;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            PlayerModel.RemovePlayer(gameObject);
            StatModel.RemoveStats(gameObject);

            view.OnGetFromPoolEvent -= OnGetFromPool;
            view.OnReturnToPoolEvent -= OnReturnToPoolEvent;
            view.StartAttackEvent -= OnStartAttack;
            view.StopAttackEvent -= OnStopAttack;
            view.CheckForCollectablesEvent -= OnCheckForCollectables;
            view.SpawnedEvent -= OnGetFromPool;
            view.playerVo.SetMoveEvent -= OnSetPos;
            view.DealDamageEvent -= OnDealDamageToTarget;

        }
    }
}