using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.SpellsRelated.Cast
{
    public class MeteorFall : Spell
    {
        public GameObject meteorToSummon;
        public GameObject shadow;
        public Vector3 shadowTargetScale;
        public Vector3 meteorFallPosition;
        public Vector3 meteorPositionOffset;
        public float animationTotalTime;

        public override void Activate()
        {
            base.Activate();
            Cast();
        }

        [Button]
        public override void Cast()
        {
            SetMeteorsPosition();
            StartShadowScaleUp();
            StartMeteorFalling();
            SpellEnd();
        }

        public override void SetPosition(Vector3 targetPosition)
        {
            base.SetPosition(targetPosition);
            transform.position = targetPosition;
            //meteorFallPosition = targetPosition;
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
    }
}
