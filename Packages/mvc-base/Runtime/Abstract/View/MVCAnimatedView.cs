using System.Diagnostics;
using DG.Tweening;
using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MVC.Base.Runtime.Abstract.View
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MVCAnimatedView : MVCView, IPanel
    {
        public IPanelVo vo { get; set; }

        public ScreenTransitionType TransitionType = ScreenTransitionType.Disolve;

        public bool UseIndependentTime = true;
        
        [SerializeField]
        private float _openAnimationDuration = .5f;
        //[SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField][ShowIf("TransitionType",ScreenTransitionType.Scale)]
        private Transform _scaleObject;
        
        private Tweener _tween;
        
        [DisableInEditorMode]
        [Button(ButtonSizes.Large,Name="Test Transition Animation")]
        [GUIColor(0,.7f,.2f)]
        public void Open()
        {
            switch (TransitionType)
            {
                case ScreenTransitionType.None:
                    
                    break;
                case ScreenTransitionType.Disolve:
                    OpenDisolve();
                    break;

                case ScreenTransitionType.Scale:
                    OpenScale();
                    break;
                        
                default:
                    OpenDisolve();
                    break;
            }
        }

        public void OpenDisolve()
        {
            _tween = DOVirtual.Float(0f, 1f, _openAnimationDuration, (float value) => { _canvasGroup.alpha = value; })
                .SetEase(Ease.OutQuad).SetUpdate(UseIndependentTime);
        }
        public void OpenScale()
        {
            if (_scaleObject == null)
                _scaleObject = transform.GetChild(0);

            if (_scaleObject == null)
                _scaleObject = transform;
            
            _scaleObject.localScale = Vector3.one*.7f;
            _scaleObject.DOScale(Vector3.one, _openAnimationDuration).SetEase(Ease.OutBack).SetUpdate(UseIndependentTime);
        }

        protected override void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            base.Awake();
            Open();
        }
        protected override void OnDestroy()
        {
            _tween.Kill();
            base.OnDestroy();
        }
    }
}