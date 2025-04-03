using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Model;
using Runtime.Model.Interfaces;
using Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.PlayerMovement
{
    public class PlayerMovementMediator : Mediator
    {
        [Inject] public PlayerMovementView view { get; set; }
        [Inject] public IPlayerModel PlayerModel { get; set; }
        [Inject] public IStatModel StatModel { get; set; }
        [Inject] public IAimModel AimModel { get; set; }
        [Inject] public ILoaderModel LoaderModel { get; set; }
        [Inject] public EntitySignals EntitySignals { get; set; }
        
        private PlayerVO _playerVo;
        private AimVO _aimVo;
        private EntityStatsVO _statVo;
        private float _diffNormal;
        private float _diffInverse;
        public float angleDegree;
        private float _targetAngle;
        private float _angleOffset = 45;

        private Vector3 lastInputDirection;

        public Vector3 targetForward;

        private void Update()
        {
            if (lastInputDirection != Vector3.zero)
            {
                var angle2 = Mathf.Atan2(lastInputDirection.x, lastInputDirection.z);
                var _targetAngle2 = Mathf.Rad2Deg * angle2;
                if (_targetAngle2 < 0)
                    _targetAngle2 += 360;
            
                view.DirectionObject.transform.rotation = Quaternion.Euler(new Vector3(0, _targetAngle2 + _angleOffset, 0));
                view.targetForward = view.DirectionObject.transform.forward;
            }
        }

        public void OnPlayerMoved(Vector3 direction)
        {
            if (_playerVo == null)
                _playerVo = PlayerModel.PlayerVos[gameObject];
            
            if (_statVo == null)
                _statVo = StatModel.Stats[gameObject];
            
            if (_aimVo == null)
                _aimVo = AimModel.AimVos[gameObject];

            var speed = _statVo.GetStat(AllStats.BaseMoveSpeed);
            var speedIncrease = speed*_statVo.GetStat(AllStats.MoveSpeedIncrease)/100f;
            speed += speedIncrease;

            lastInputDirection = direction;

            if (direction != Vector3.zero)
            {
                var angle2 = Mathf.Atan2(direction.x, direction.z);
                var _targetAngle2 = Mathf.Rad2Deg * angle2;
                if (_targetAngle2 < 0)
                    _targetAngle2 += 360;
            
                view.DirectionObject.transform.rotation = Quaternion.Euler(new Vector3(0, _targetAngle2 + _angleOffset, 0));
                view.targetForward = view.DirectionObject.transform.forward;
            }
            
            
            if (view.isPlayerAttacking)
            {
                HandlePlayerStop(true);
                lastDirection = Vector3.zero;
                return;
            }
            
            if (direction == Vector3.zero)
            {
                HandlePlayerStop();
                return;
            }
            
            HandlePlayerStartMove();
            UpdateMovement(direction, speed);
        }



        private void OnPlayerDies()
        {
            view.PlayerDies();
        }

        private Vector3 lastDirection = Vector3.zero;
        private void UpdateMovement(Vector3 direction, float speed)
        {
            if (lastDirection != direction)
            {
                
                ChangeDirection(direction);
                lastDirection = direction;
            }
            
            var rad = Mathf.Deg2Rad * (view.DirectionObject.transform.localRotation.eulerAngles.y + 90);
            view.SetTargetAnimationSpeed(-Mathf.Cos(rad), Mathf.Sin(rad));

            var deltaMove = view.DirectionObject.transform.forward * speed * Time.deltaTime;
            transform.position += deltaMove;
        }

        private void ChangeDirection(Vector3 targetDirection)
        {
            var magnitude = (lastDirection - targetDirection).magnitude;
            var angle = Mathf.Atan2(targetDirection.x, targetDirection.z);
            _targetAngle = Mathf.Rad2Deg * angle;
            if (_targetAngle < 0)
                _targetAngle += 360;
            var targetAngle = new Vector3(0, _targetAngle + _angleOffset, 0);

            DOTween.Kill("RotatePlayer");

            var rotateTime = view.rotateTime*(magnitude / 2);
            transform.DORotate(targetAngle, rotateTime).SetEase(view.rotateEase).SetId("RotatePlayer");
        }

       
        private void HandlePlayerStop(bool goIdleMode = true)
        {
            view.SetTargetAnimationSpeed(0, 0);
            if (_playerVo.isMoving)
            {
                _playerVo.isMoving = false;
                view.StopMoving(goIdleMode);
                _playerVo.PlayerStoppedMoving();
            }
        }

        private void HandlePlayerStartMove()
        {
            if (!_playerVo.isMoving)
            {
                _playerVo.isMoving = true;
                view.StartedMoving();
                _playerVo.PlayerStartedToMove();
            }
        }
        
        private void OnPlayerStoppedWalking()
        {
            lastDirection = transform.forward;
        }

        private void OnPlayerStartedWalking()
        {
            
        }

        private void OnLeaveAfterImage(Vector3 position)
        {
        }
        
        private void OnPlayerDashed()
        {
            view.StartDash(_playerVo);
        }

        private void OnStopPlayerMovement()
        {
            view.canMove = false;
        }

        private void OnStartPlayerMovement()
        {
            view.canMove = true;
        }

        public void Initialize()
        {
            view.ResetPlayerDies();
        }

        public void Destroy()
        {
        }

        private void OnPlayerDashStarted()
        {
            EntitySignals.PlayerDashedSignal.Dispatch(gameObject);
        }
        
        public override void OnRegister()
        {
            base.OnRegister();
            LoaderModel.GetLoader(gameObject).InitializeMediatorsEvent += Initialize;
            LoaderModel.GetLoader(gameObject).DestroyMediatorsEvent += Destroy;
            view.PlayerMoved += OnPlayerMoved;
            view.PlayerDashed += OnPlayerDashed;
            view.LeaveAfterImageEvent += OnLeaveAfterImage;
            view.PlayerStartedMovingEvent += OnPlayerStartedWalking;
            view.PlayerStoppedMovingEvent += OnPlayerStoppedWalking;
            view.PlayerDashStartedEvent += OnPlayerDashStarted;
            
            EntitySignals.StartCharacterMovementSignal.AddListener(OnStartPlayerMovement);
            EntitySignals.StopCharacterMovementSignal.AddListener(OnStopPlayerMovement);
            EntitySignals.PlayerDiedSignal.AddListener(OnPlayerDies);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            LoaderModel.GetLoader(gameObject).InitializeMediatorsEvent -= Initialize;
            LoaderModel.GetLoader(gameObject).DestroyMediatorsEvent -= Destroy;
            view.PlayerMoved -= OnPlayerMoved;
            view.PlayerDashed -= OnPlayerDashed;
            view.LeaveAfterImageEvent -= OnLeaveAfterImage;
            view.PlayerStartedMovingEvent -= OnPlayerStartedWalking;
            view.PlayerStoppedMovingEvent -= OnPlayerStoppedWalking;
            view.PlayerDashStartedEvent -= OnPlayerDashStarted;

            EntitySignals.StartCharacterMovementSignal.RemoveListener(OnStartPlayerMovement);
            EntitySignals.StopCharacterMovementSignal.RemoveListener(OnStopPlayerMovement);
            EntitySignals.PlayerDiedSignal.RemoveListener(OnPlayerDies);

        }
    }
}
