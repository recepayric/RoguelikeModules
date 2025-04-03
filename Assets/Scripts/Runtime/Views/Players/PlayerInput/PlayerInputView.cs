using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MVC.Base.Runtime.Abstract.View;
using Runtime.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Views.Players.PlayerInput
{
    public class PlayerInputView : MVCView
    {
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource _cancellationTokenChargeUlti;
        
        public event UnityAction UsePotionEvent;
        public event UnityAction<Vector3, int> CastSpellEvent;
        public event UnityAction<int> TryToCastSpellEvent;
        
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int AttackSpeedMelee = Animator.StringToHash("AttackSpeedMelee");
        private static readonly int AttackWait = Animator.StringToHash("AttackWait");
        private static readonly int IsSpellReady = Animator.StringToHash("IsSpellReady");
        private static readonly int IsSpellCast = Animator.StringToHash("IsSpellCast");
        private static readonly int CastSpeed = Animator.StringToHash("CastSpeed");
        private static readonly int Spell = Animator.StringToHash("CastSpell");

        [FoldoutGroup("Events")] public UnityEvent<Vector3, bool> OnMoveInput;
        [FoldoutGroup("Events")] public UnityEvent OnDashInput;
        [FoldoutGroup("Events")] public UnityEvent OnPlayerStartedAttacking;
        [FoldoutGroup("Events")] public UnityEvent OnPlayerStoppedAttacking;
        
        [FoldoutGroup("Attack Times")] public float attackTime1, attackWaitTime1, attackTime1CrossFadeTime;
        [FoldoutGroup("Attack Times")] public float attackTime2, attackWaitTime2, attackTime2CrossFadeTime;
        [FoldoutGroup("Attack Times")] public float attackTime3, attackWaitTime3, attackTime3CrossFadeTime;
        [FoldoutGroup("Attack Times")] public float attackTimeUlti, attackTimeUltiCrossFadeTime;

        public bool isContinuousAttack;
        public Animator animator;

        [FoldoutGroup("Clips")] public AnimationClip attackClip1, attackClip1Wait;
        [FoldoutGroup("Clips")] public AnimationClip attackClip2, attackClip2Wait;
        [FoldoutGroup("Clips")] public AnimationClip attackClip3, attackClip3Wait;

        [FoldoutGroup("Clips")] public AnimationClip attackClipUlti;
        [FoldoutGroup("Clips")] public AnimationClip attackClipUltiReady;

        public bool isAttacking;

        public float attackCooldown;
        public float attackCooldownTimer;

        public GameObject scytheObject;
        public GameObject scytheLeaveParent;

        public GameObject scytheInitialAngle;
        public GameObject scytheInitialParent;
        public GameObject scytheSecondAngle;
        public GameObject scytheSecondParent;
        public GameObject scytheThirdAngle;
        public GameObject scytheThirdParent;

        public float scytheGoMainHandTime;
        public float scytheGoMainHandDelay;

        public Ease scytheFirstMoveEase;
        public Ease scytheFirstRotateEase;

        public Ease scytheFirstMoveEase2;
        public Ease scytheFirstRotateEase2;

        public float attackSpeed;
        public float sample = 24;
        public float sample2 = 24;

        public int grabKeyframe = 15;
        public int grabKeyframe2 = 15;

        public ParticleSystem slash1;
        public ParticleSystem slash2;
        public ParticleSystem slash3;
        public ParticleSystem slashUlti;

        public string scytheTweenIds;
        public string turnTargetTweenId;

        public Vector3 nextAttackTargetPos;

        public LayerMask layerMask;
        private Camera _cameraObject;

        public Ease lookAtEase;
        public float lookAtDelay;
        public float lookAtTime;

        public GameObject spellAreaIndicator;

        public bool isPreparingSpell;
        public bool isCastingSpell;

        public bool isPressingAttack;
        public bool isReadyToUlti;

        public bool isChargingAttack;

        public float ultiTriggerTime;
        public float ultiTriggerTimer;

        public float ultiChargeAnimationTime;
        public float ultiChargeTime;
        public float ultiChargeTimer;

        public GameObject ultiChargeOut;

        public float ultiEnterThreshold;
        public float waitTimeGracePeriod;
        public float ultiCooldownTimer;
        public float ultiCooldownTime;
        public bool isInGracePeriod;

        public int attackCount = 0;
        public bool canAttackContinue;
        public bool isAttackCanclled;
        public bool isUsingUlti;

        public float turnSpeed;
        
        public AnimationClip castSpellAnimationClip;
        public float spellCastSpeed;
        public float spellCooldown;
        public float spellCooldownTimer;
        public float scytheGoMainHandTimeSpell;
        public Ease scytheFirstMoveEaseSpell;

        public List<ParticleSystem> spellReadyParticles;

        public int skillNumber;
        public int castingSkillIndex;
        public KeyCode[] skillKeycodes;

        protected override void Start()
        {
            base.Start();
            _cameraObject = Camera.main;
            scytheTweenIds = "scythe" + GetInstanceID();
            turnTargetTweenId = "playerTurn" + GetInstanceID();

            skillKeycodes = new[]
            {
                KeyCode.Alpha1,
                KeyCode.Alpha2,
                KeyCode.Alpha3
            };
        }

        public void LeaveScytheFirst()
        {
            if (isAttackCanclled) return;
            var timeToSwitch = grabKeyframe / sample;
            timeToSwitch /= attackSpeed;

            scytheObject.transform.SetParent(scytheLeaveParent.transform);
            scytheObject.transform.DOLocalMove(scytheSecondAngle.transform.localPosition, timeToSwitch)
                .SetEase(scytheFirstMoveEase).SetId(scytheTweenIds);
            scytheObject.transform.DOLocalRotate(scytheSecondAngle.transform.localEulerAngles, timeToSwitch)
                .SetEase(scytheFirstRotateEase).SetId(scytheTweenIds);
        }

        public void GrabScytheFirst()
        {
            if (isAttackCanclled) return;
            scytheObject.transform.SetParent(scytheSecondParent.transform);
        }

        public void LeaveScytheSecond()
        {
            if (isAttackCanclled) return;
            
            var timeToSwitch = grabKeyframe2 / sample2;
            timeToSwitch /= attackSpeed;

            scytheObject.transform.SetParent(scytheLeaveParent.transform);
            scytheObject.transform.DOLocalMove(scytheThirdAngle.transform.localPosition, timeToSwitch)
                .SetEase(scytheFirstMoveEase2).SetId(scytheTweenIds);
            scytheObject.transform.DOLocalRotate(scytheThirdAngle.transform.localEulerAngles, timeToSwitch)
                .SetEase(scytheFirstRotateEase2).SetId(scytheTweenIds);
        }

        public void GrabScytheSecond()
        {
            if (isAttackCanclled) return;
            scytheObject.transform.SetParent(scytheThirdParent.transform);
        }

        private void ChangeScytheHand()
        {
            scytheObject.transform.SetParent(scytheInitialParent.transform);
            scytheObject.transform.DOLocalMove(scytheInitialAngle.transform.localPosition, scytheGoMainHandTime)
                .SetEase(scytheFirstMoveEase2).SetDelay(scytheGoMainHandDelay);
            scytheObject.transform.DOLocalRotate(scytheInitialAngle.transform.localEulerAngles, scytheGoMainHandTime)
                .SetEase(scytheFirstRotateEase2).SetDelay(scytheGoMainHandDelay);
        }

        public void Slash1()
        {
            if (isAttackCanclled) return;
            
            slash1.gameObject.SetActive(true);
            slash1.Play();
        }

        public void Slash2()
        {
            if (isAttackCanclled) return;

            slash2.gameObject.SetActive(true);
            slash2.Play();
        }

        public void Slash3()
        {
            if (isAttackCanclled) return;

            slash3.gameObject.SetActive(true);
            slash3.Play();
        }

        private void StartNewAttack()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }

            CastRayAttackTarget();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenChargeUlti = new CancellationTokenSource();
            Attack(_cancellationTokenSource.Token).Forget();
        }

        private void CancelAttack()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                isAttackCanclled = true;
            }
        }
        
        private bool CaptureAttack()
        {
            if (isCastingSpell) return false;
            if (isPreparingSpell) return false;
            
            

            if (Input.GetMouseButtonUp(0))
            {
                isContinuousAttack = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                ultiChargeTimer = 0;
                ultiTriggerTimer = 0;
                isPressingAttack = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                isPressingAttack = false;

                if (isReadyToUlti)
                {
                    SlashUlti().Forget();
                    isChargingAttack = false;
                }
                else
                {
                    CancelUltimateCharge();
                }
            }
            
            if (Input.GetMouseButton(0))
            {
                isContinuousAttack = true;
                if (isChargingAttack) return true;
                if (attackCooldownTimer > 0) return false;

                if (isUsingUlti)
                    return false;

                if (isAttacking || isInGracePeriod)
                {
                    if (!canAttackContinue) return true;

                    StartNewAttack();
                    return true;
                }
                
                OnMoveInput?.Invoke(Vector3.zero, false);
                StartNewAttack();
                return true;
            }
            
            return false;
        }

        private void UpdateUltimateAttack()
        {
            if (!isPressingAttack) return;
            if (ultiCooldownTimer > 0) return;

            ultiTriggerTimer += Time.deltaTime;
            if (isAttacking) return;

            if (ultiTriggerTimer >= ultiTriggerTime)
            {
                if (!isChargingAttack)
                {
                    _cancellationTokenChargeUlti = new CancellationTokenSource();
                    StartUltiCharge(_cancellationTokenChargeUlti.Token).Forget();
                }
            }
        }

        private void CancelUltimateCharge()
        {
            isReadyToUlti = false;

            if (!isChargingAttack) return;

            isChargingAttack = false;

            ultiChargeOut.SetActive(false);

            _cancellationTokenChargeUlti.Cancel();
            _cancellationTokenChargeUlti.Dispose();
            animator.SetBool(IsAttacking, false);

            ultiCooldownTimer = ultiCooldownTime;
            AttackStopped(true);
            SetAttackLayerWeight(0.2f, 1, 0);
        }

        private async UniTaskVoid StartUltiCharge(CancellationToken cancellationToken)
        {
            SetAttackLayerWeight(ultiChargeAnimationTime, 0, 1, ultiEnterThreshold);
            isAttacking = true;

            animator.SetBool(IsAttacking, true);

            EndGracePeriod();
            ultiChargeOut.SetActive(true);
            isChargingAttack = true;


            Debug.Log("Ulti Charge Time: " + ultiChargeTimer);

            //await UniTask.WaitForSeconds(ultiEnterThreshold, cancellationToken: cancellationToken, cancelImmediately: true);

            ArvaveUtility.SwitchAnimation(animator, "UltiReady", ultiChargeAnimationTime, 2);
            ultiChargeTimer = 0;
            OnPlayerStartedAttacking?.Invoke();


            while (ultiChargeTimer < ultiChargeTime)
            {
                var percentage = ultiChargeTimer / ultiChargeTime;

                ultiChargeTimer += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cancellationToken,
                    cancelImmediately: true);
            }

            Debug.Log("Ulti Charge Done: ");

            ultiChargeTimer = ultiChargeTime;

            isReadyToUlti = true;
        }

        private void AttackStopped(bool resetAttackCount)
        {
            Debug.Log("Attack Stopped!");
            isAttacking = false;
            OnPlayerStoppedAttacking?.Invoke();
            attackCount = resetAttackCount ? 0 : attackCount;
        }
        
        public async UniTaskVoid Attack(CancellationToken cancellationToken)
        {
            SetAttackLayerWeight(0, 0, 1);
            OnPlayerStartedAttacking?.Invoke();
            isAttacking = true;
            isInGracePeriod = false;
            canAttackContinue = false;
            isAttackCanclled = false;


            var attackTime = attackCount switch
            {
                0 => attackTime1,
                1 => attackTime2,
                2 => attackTime3
            };

            var attackTimeWait = attackCount switch
            {
                0 => attackWaitTime1,
                1 => attackWaitTime2,
                2 => attackWaitTime3
            };

            var attackCrossFadeTime = attackCount switch
            {
                0 => attackTime1CrossFadeTime,
                1 => attackTime2CrossFadeTime,
                2 => attackTime3CrossFadeTime
            };

            var attackClip = attackCount switch
            {
                0 => attackClip1,
                1 => attackClip2,
                2 => attackClip3
            };

            var attackClipWait = attackCount switch
            {
                0 => attackClip1Wait,
                1 => attackClip2Wait,
                2 => attackClip3Wait
            };

            var attackAnimatorName = attackCount switch
            {
                0 => "Attack1",
                1 => "Attack2",
                2 => "Attack3"
            };

            attackCount++;
            if (attackCount >= 3)
                attackCount = 0;

            //Turn to target
            TurnToHitTarget();


            //Set states to inital form
            animator.SetBool(IsAttacking, true);

            //Calculate animation speeds based on attack times!
            var attackTimeSpeedMult = ArvaveUtility.GetAnimationSpeed(attackClip, attackTime);
            var attackWaitTimeSpeed = ArvaveUtility.GetAnimationSpeed(attackClipWait, attackTimeWait);

            attackSpeed = attackTimeSpeedMult;

            //Set speeds
            animator.SetFloat(AttackSpeedMelee, attackTimeSpeedMult);
            animator.SetFloat(AttackWait, attackWaitTimeSpeed);

            //Switch animation to Attack!
            ArvaveUtility.SwitchAnimation(animator, attackAnimatorName, attackCrossFadeTime, 2);

            //Wait for half the attack time to end before capturing continuous attacks
            await UniTask.WaitForSeconds(attackTime / 3, cancellationToken: cancellationToken, cancelImmediately: true);
            
            //Wait for remaining attack time to finish to prevent super fast attacks
            await UniTask.WaitForSeconds(attackTime / 3 * 2, cancellationToken: cancellationToken,
                cancelImmediately: true);

            if(!isContinuousAttack)
                canAttackContinue = true;

            if (attackCount == 0)
                attackCooldownTimer = attackCooldown;

            await UniTask.WaitForSeconds(attackTimeWait, cancellationToken: cancellationToken, cancelImmediately: true);

            canAttackContinue = true;

            animator.SetBool(IsAttacking, false);

            if (attackCount == 2)
                ChangeScytheHand();

            AttackStopped(false);

            if (attackCount == 0)
                return;

            isInGracePeriod = true;

            await UniTask.WaitForSeconds(waitTimeGracePeriod, cancellationToken: cancellationToken,
                cancelImmediately: true);

            EndGracePeriod();
        }
        
        private void Update()
        {
            attackCooldownTimer -= Time.deltaTime;
            ultiCooldownTimer -= Time.deltaTime;
            spellCooldownTimer -= Time.deltaTime;
            if (CaptureAttack())
            {
                return;
            }

            UpdateUltimateAttack();
            UpdateSpellCasting();
            CaptureSpell();
            CaptureMovementInputs();
            CaptureDashInputs();
            CapturePotionUse();
        }

        public List<ParticleSystem> rageParticles;
        public bool isRaged;
        public float rageTime;
        public float rageTimer;
        private void UpdateSpellCasting()
        {
            if (!isPreparingSpell) return;
            if (isCastingSpell) return;
            
            CastRayForSpellTarget();

            if (isRaged) return;
            rageTimer += Time.deltaTime;

            if (rageTimer >= rageTime)
            {
                isRaged = true;
                ArvaveUtility.SetParticleEmission(rageParticles, true);
            }
        }

        public void StartSpellCasting(int index)
        {
            castingSkillIndex = index;
            rageTimer = 0;
            ArvaveUtility.SetParticleEmission(spellReadyParticles, true);
            //succubusDissolve.PlayChainParticle();
            SetAttackLayerWeight(0, 0, 1);
            animator.SetBool(IsSpellReady, true);

            spellAreaIndicator.SetActive(true);
            isPreparingSpell = true;
            CastRayForSpellTarget();
            OnPlayerStartedAttacking?.Invoke();
        }
        
        private async UniTaskVoid CastSpell()
        {
            isRaged = false;
            ArvaveUtility.SetParticleEmission(spellReadyParticles, false);
            ArvaveUtility.SetParticleEmission(rageParticles, false);
            //succubusDissolve.StopChainParticles();
            isPreparingSpell = false;
            isCastingSpell = true;
            var castPosition = spellAreaIndicator.transform.position;
            var speedMulti = ArvaveUtility.GetAnimationSpeed(castSpellAnimationClip, spellCastSpeed);
            animator.SetFloat(CastSpeed, speedMulti);
            animator.SetTrigger(Spell);
            animator.SetBool(IsSpellReady, false);
            animator.SetBool(IsSpellCast, true);

            await UniTask.WaitForSeconds(spellCastSpeed);


            CastSpellEvent?.Invoke(castPosition, castingSkillIndex);
            isPreparingSpell = false;
            OnPlayerStoppedAttacking?.Invoke();
            SetAttackLayerWeight(.5f, 1, 0);
            spellCooldownTimer = spellCooldown;
            isCastingSpell = false;
            animator.SetBool(IsSpellCast, false);
        }
        
        [Button]
        public void ChangeScytheHandAfterSpell()
        {
            var percentage = 0f;

            var initialPos = scytheObject.transform.position;
            var initialRotation = scytheObject.transform.rotation;
            DOTween.To(() => percentage, x => percentage = x, 1f, scytheGoMainHandTimeSpell)
                .OnUpdate(() =>
                {
                    scytheObject.transform.position = Vector3.Lerp(initialPos,scytheInitialAngle.transform.position, percentage );
                    scytheObject.transform.rotation = Quaternion.Lerp(initialRotation,scytheInitialAngle.transform.rotation, percentage );
                }).SetEase(scytheFirstMoveEaseSpell).OnComplete(() =>
                {
                    scytheObject.transform.SetParent(scytheInitialAngle.transform);
                });
        }

        private void StopSpellCasting()
        {
            spellAreaIndicator.SetActive(false);
            CastSpell().Forget();

            //ArvaveUtility.SwitchAnimation(animator, "None", spellReadyCrossTime);
        }

        private void CaptureSpell()
        {
            if (isAttacking) return;
            
            for (int i = 0; i < skillNumber; i++)
            {
                if (Input.GetKey(skillKeycodes[i]))
                {
                    if (spellCooldownTimer > 0) return;
                    if (isPreparingSpell) return;
                    if (isCastingSpell) return;
                    TryToCastSpellEvent?.Invoke(i);
                    //StartSpellCasting();
                }

                if (Input.GetKeyUp(skillKeycodes[i]))
                {
                    if (isPreparingSpell && castingSkillIndex == i)
                        StopSpellCasting();
                }
            }

            
        }

        private async UniTaskVoid SlashUlti()
        {
            isUsingUlti = true;
            ultiChargeOut.SetActive(false);
            Debug.Log("Ulti Start!");
            isReadyToUlti = false;

            slashUlti.Play();

            var attackTimeSpeedMult = ArvaveUtility.GetAnimationSpeed(attackClipUlti, attackTimeUlti);
            animator.SetFloat(AttackSpeedMelee, attackTimeSpeedMult);

            ArvaveUtility.SwitchAnimation(animator, "Ulti", attackTimeUltiCrossFadeTime, 2);

            await UniTask.WaitForSeconds(attackTimeUlti);

            animator.SetBool(IsAttacking, false);
            AttackStopped(true);
            //attackCooldownTimer = attackCooldown;
            ultiCooldownTimer = ultiCooldownTime;
            isUsingUlti = false;
        }
        
        public void PlayerDashed()
        {
            Debug.Log("Player Dashed!!!");

            CancelUltimateCharge();
            isPressingAttack = false;
            if (isAttacking)
            {
                CancelAttack();
                DOTween.Kill(scytheTweenIds);
                DOTween.Kill(turnTargetTweenId);
                AttackStopped(true);
                ChangeScytheHand();

                slash1.Stop();
                slash1.Clear();
                slash2.Stop();
                slash2.Clear();
                slash3.Stop();
                slash3.Clear();
            }

            animator.SetLayerWeight(2, 0);
        }

        private void CapturePotionUse()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                UsePotionEvent?.Invoke();
        }

        private void CaptureMovementInputs()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 moveInput = new Vector3(horizontal, 0, vertical);
            OnMoveInput?.Invoke(moveInput, true);

            if (moveInput != Vector3.zero && isInGracePeriod)
            {
                EndGracePeriod();
            }
        }

        private void CaptureDashInputs()
        {
            if (isUsingUlti) return;
            if (Input.GetKeyDown(KeyCode.Space))
                OnDashInput?.Invoke();
        }

        private void SetAttackLayerWeight(float changeTime, float init, float target, float delay = 0f)
        {
            DOTween.Kill("LayerWeight" + GetInstanceID());
            var layerWeight = animator.GetLayerWeight(2);

            DOTween.To(() => layerWeight, x => layerWeight = x, target, changeTime)
                .OnUpdate(() => { animator.SetLayerWeight(2, layerWeight); }).SetEase(Ease.InSine)
                .SetId("LayerWeight" + GetInstanceID()).SetDelay(delay);
        }
        
        private void EndGracePeriod()
        {
            isInGracePeriod = false;
            attackCount = 0;
        }
        
        private void TurnToHitTarget()
        {
            transform.DOLookAt(nextAttackTargetPos, lookAtTime).SetEase(lookAtEase).SetDelay(lookAtDelay)
                .SetId(turnTargetTweenId);
        }
        
        private void CastRayAttackTarget()
        {
            Debug.Log("Sending Ray ");

            Ray ray = _cameraObject.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Hit: " + hit.transform.gameObject);
                nextAttackTargetPos = hit.point;
                nextAttackTargetPos.y = transform.position.y;
            }
        }
        
        private void CastRayForSpellTarget()
        {
            Ray ray = _cameraObject.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
            {
                var pos = hit.point;
                pos.y = transform.position.y;

                var targetForward = spellAreaIndicator.transform.position - transform.position;
                transform.forward = Vector3.Lerp(transform.forward, targetForward, Time.deltaTime * turnSpeed);

                spellAreaIndicator.transform.position = pos;
            }
        }
    }
}