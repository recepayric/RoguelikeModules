using DG.Tweening;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.SpellsRelated.Cast
{
    public class MeteorFall : MonoBehaviour, IPoolObject
    {
        public GameObject meteorToSummon;
        public GameObject shadow;
        public Vector3 shadowTargetScale;
        public Vector3 meteorFallPosition;
        public Vector3 meteorPositionOffset;
        public float animationTotalTime;

        public void Activate()
        {
            Cast();
        }

        [Button]
        public void Cast()
        {
            SetMeteorsPosition();
            StartShadowScaleUp();
            StartMeteorFalling();
            SpellEnd();
        }

        public void SetPosition(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        private void StartShadowScaleUp()
        {
            shadow.transform.localScale = Vector3.zero;
            shadow.transform.DOScale(shadowTargetScale, animationTotalTime).SetEase(Ease.InQuart);
        }

        private void StartMeteorFalling()
        {
            meteorToSummon.transform.DOLocalMove(meteorFallPosition, animationTotalTime).SetEase(Ease.InQuart);
        }

        private void SetMeteorsPosition()
        {
            meteorToSummon.transform.localPosition = meteorFallPosition + meteorPositionOffset;
        }

        private void SpellEnd()
        {
            DOVirtual.DelayedCall(animationTotalTime, () =>
            {
                BasicPool.instance.Return(gameObject);
            });
        }

        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
            
        }

        public void OnGet()
        {
            SetMeteorsPosition();
            Cast();
        }
    }
}
