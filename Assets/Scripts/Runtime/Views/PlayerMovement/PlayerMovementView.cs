using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Events;
using MVC.Base.Runtime.Abstract.View;
using MVC.Base.Runtime.Concrete.Model;
using MVC.Base.Runtime.Extensions;
using Runtime.Constants;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Views.PlayerMovement
{
    public class PlayerMovementView : MVCView
    {

        public event UnityAction<Vector3> PlayerMoved;
        public event UnityAction PlayerStartedMovingEvent;
        public event UnityAction PlayerStoppedMovingEvent;
        public event UnityAction PlayerDashed;
        public event UnityAction PlayerDashStartedEvent;
        public event UnityAction<Vector3> LeaveAfterImageEvent;

        public ParticleSystem dashParticle;

        [SerializeField] private GameObject _directionObject;
        [SerializeField] public Rigidbody _rigidBody;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _animationSpeedChangeSpeed;

        public bool canMove;
        public GameObject DirectionObject => _directionObject;
        public Rigidbody Rigidbody => _rigidBody;

        public float TurnSpeed => _turnSpeed;
        public Vector3 targetForward;
        public float baseSpeed;
        private Vector3 _direction;
        
        public float _speedX;
        public float _speedZ;

        private float _targetX, _targetZ;

        public PoolKey afterImageKey;
        public float dashCooldown = 3;
        public float dashCooldownTimer = 0;

        public bool isSpawned;

        public AnimationClip pickUpClip;
        public GameObject crystalHoldParent;

        public bool isStoppedWalking = false;
        public bool isPlayerStartedWalking = false;
        public bool hasStopAnimation = false;

        public bool isPlayerAttacking;

        public ParticleSystem leftFootParticle;
        public ParticleSystem rightFootParticle;


        public void PlayLeftFootParticle()
        {
            leftFootParticle.Play();
        }
        
        public void PlayRightFootParticle()
        {
            rightFootParticle.Play();
        }

        public void OnPlayerMove(Vector3 direction, bool hasStopAnimation)
        {
            _direction = direction;
            this.hasStopAnimation = hasStopAnimation;
        }

        public void PlayerStartedToAttack()
        {
            isPlayerAttacking = true;
        }

        public void PlayerAttackEnded()
        {
            isPlayerAttacking = false;
        }

        public void OnPlayerDashed()
        {
            PlayerDashed?.Invoke();
        }

        private void Update()
        {
            _rigidBody.linearVelocity = Vector3.zero;
            //base.FixedUpdate();
            UpdateAnimatorParameters();
            if (!canMove)
            {
                PlayerMoved?.Invoke(Vector3.zero);
                return;
            }

            PlayerMoved?.Invoke(_direction);
            
            UpdateAnimationSpeed();
            dashCooldownTimer -= Time.deltaTime;
        }
        

        public float rotateTime;
        public float rotateTimeWhileMoving;
        public Ease rotateEase;
        public float timeToStartMove;
        public float timeToStopMove;
        public float dashSwitchInTime;
        public float dashSwitchOutTime;

        public void StopMoving(bool goIdleMode)
        {
            //Debug.LogError("Stopped Moving");
            if(goIdleMode)
                ArvaveUtility.SwitchAnimation(_animator, "Idle", timeToStopMove);
            isPlayerStartedWalking = false;
            isStoppedWalking = true;
            _animator.SetBool(Running, false);
        }

        public void StartedMoving()
        {
            //Debug.LogError("Started Moving");
            ArvaveUtility.SwitchAnimation(_animator, "Run", timeToStartMove);
            isPlayerStartedWalking = true;
            isStoppedWalking = false;
            _animator.SetBool(Running, true);
        }

        
        private void UpdateAnimatorParameters()
        {
            if (!isSpawned)
            {
                _animator.SetFloat("x", _speedX);
                _animator.SetFloat("z", _speedZ);
                return;
            }
            
            _animator.SetFloat("x", _speedX);
            _animator.SetFloat("z", _speedZ);
        }

        private void UpdateAnimationSpeed()
        {
            if (_targetX == 0 && _targetZ == 0)
            {
                //_speedX = 0;
                //_speedZ = 0;
                //return;
            }
            _speedX = Mathf.MoveTowards(_speedX, _targetX, _animationSpeedChangeSpeed * Time.deltaTime);
            _speedZ = Mathf.MoveTowards(_speedZ, _targetZ, _animationSpeedChangeSpeed * Time.deltaTime);
            
        }

        public void SetTargetAnimationSpeed(float x, float z)
        {
            _targetX = x;
            _targetZ = z;
        }
        

        public void StartHoldingCrystal()
        {
            _animator.SetBool("IsHoldingCrystal", true);
            crystalHoldParent.SetActive(true);
        }

        public void StopHoldingCrystal()
        {
            _animator.SetBool("IsHoldingCrystal", false);
            crystalHoldParent.SetActive(false);
        }

        private void SetDashLayer(float changeTime, float init, float target)
        {
            var layerWeight = init;

            DOTween.To(() => layerWeight, x => layerWeight = x, target, changeTime)
                .OnUpdate(() =>
                {
                    _animator.SetLayerWeight(1, layerWeight); 
                }).SetEase(Ease.Linear);
        }
        
        private Vector3 _dashStartPos;
        private Vector3 _dashDirection;
        private float _dashLength;
        private float _dashLengthFinal;
        private float _dashedLength;
        private EntityVO _entityVo;
        public LayerMask dashLayer;
        private static readonly int PickupCrystal = Animator.StringToHash("PickupCrystal");
        private static readonly int PickUpSpeed = Animator.StringToHash("PickUpSpeed");
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Dashing = Animator.StringToHash("Dashing");

        public void StartDash(EntityVO entityVo)
        {
            if (!canMove) return;

            if (dashCooldownTimer > 0)
                return;

            DOTween.Kill("RotatePlayer");
            
            //ArvaveUtility.SwitchAnimation(_animator, "Dash", dashSwitchInTime);
            //_animator.Play("Dash");
            
            SetDashLayer(dashSwitchInTime, 0, 1);

            _animator.SetBool(Dashing, true);

            PlayerDashStartedEvent?.Invoke();

            dashCooldownTimer = dashCooldown;
            _entityVo = entityVo;
            //Check if I can dash or not!

            transform.forward = targetForward;
            _dashedLength = 0;
            _dashStartPos = _rigidBody.transform.position;
            _dashLength = _entityVo.DashVo.dashLength;
            _dashLengthFinal = _dashLength;
            _dashDirection = targetForward;
            dashParticle.transform.forward = _dashDirection;
            dashParticle.Play();

            _entityVo.DashVo.isDashing = true;
            PlayerStoppedMovingEvent?.Invoke();
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position.AddY(1), _directionObject.transform.forward, out hit, _dashLength,
                    dashLayer))
            {
                var length = Vector3.Magnitude(hit.point - _dashStartPos);
                _dashLengthFinal = length;
            }
            
            DashAsync().Forget();
        }

        private async UniTask DashAsync()
        {
            int dashFrame = 0;
            int afterImageNumber = 0;
            float dashTime = _entityVo.DashVo.dashTime;
            float dashTimePassed = 0;
            float afterImageTotalDashedLength = 0;
            float afterImagePerLength = 1;


            // Create afterimages during the dash
            while (dashTimePassed < dashTime)
            {
                if (dashFrame == 0)
                {
                    //LeaveAfterImageEvent?.Invoke(Rigidbody.position);
                    afterImageNumber++;
                }

                var dashTarget = _dashStartPos + _dashDirection * _dashLength;
                var movePerFrame = (dashTarget - _dashStartPos) * (Time.deltaTime / dashTime);
                _dashedLength += movePerFrame.magnitude;
                afterImageTotalDashedLength += movePerFrame.magnitude;
                if (_dashLengthFinal != _dashLength && _dashedLength > _dashLengthFinal)
                {
                    var tempMove = _dashLengthFinal - _dashedLength;
                    var perc = tempMove / movePerFrame.magnitude;
                    movePerFrame = (dashTarget - _dashStartPos) * (Time.deltaTime / dashTime) * perc;
                    //_rigidBody.MovePosition(_rigidBody.position + movePerFrame);
                    transform.position += movePerFrame;
                    Debug.Log(_animator.IsInTransition(0));
                    
                    _animator.SetBool(Dashing, false);
                    SetDashLayer(dashSwitchOutTime, 1, 0);

                    
                    _entityVo.DashVo.isDashing = false;
                    //CreateAfterImage();
                    //LeaveAfterImageEvent?.Invoke(Rigidbody.position);
                    return;
                }

                //var deltaMove = targetAngleObject.transform.forward * moveSpeed * Time.fixedDeltaTime;
                //_rigidBody.MovePosition(_rigidBody.position + movePerFrame);
                transform.position += movePerFrame;

                if (afterImageTotalDashedLength >= afterImagePerLength)
                {
                    afterImageTotalDashedLength -= afterImagePerLength;
                    var afterImageTarget = _dashStartPos + _dashDirection * (afterImagePerLength * afterImageNumber);
                    //CreateAfterImage(afterImageTarget);
                    //LeaveAfterImageEvent?.Invoke(afterImageTarget);
                    afterImageNumber++;
                }

                dashFrame++;

                dashTimePassed += Time.deltaTime;
                if (dashTimePassed >= dashTime)
                {

                   
                    _animator.SetBool(Dashing, false);
                    SetDashLayer(dashSwitchOutTime, 1, 0);


                    _entityVo.DashVo.isDashing = false;
                    //CreateAfterImage();
                    //LeaveAfterImageEvent?.Invoke(Rigidbody.position);
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
            
            //Debug.Log("Dash Finished!");
        }

        public void PlayerDies()
        {
            ArvaveUtility.SwitchAnimation(_animator, "Death", 0.1f);
            
        }
        
        public void ResetPlayerDies()
        {
            _animator.SetBool("IsDead", false);
        }
    }
}
