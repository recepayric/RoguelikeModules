using Data.WeaponDataRelated;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UIRelated.CharacterSelect
{
    public class WeaponFrameUI : MonoBehaviour, IPoolObject
    {
        private const string IdForScale = "scaleUpWeapon";
        public GameObject weaponImage;
        public TextMeshProUGUI weaponName;
        private CharacterSelectUI _characterSelectUI;

        private WeaponDataSo _weaponDataSo;


        public void SelectWeapon()
        {
            _characterSelectUI.SetWeaponDetails(_weaponDataSo);
        }

        public void SetWeaponObject(WeaponDataSo data)
        {
            _weaponDataSo = data;
            weaponName.text = data.WeaponName;
        }
        
        public void SetCharacterSelectUI(CharacterSelectUI ui)
        {
            _characterSelectUI = ui;
        }
        
        #region Highlight

        public void OnPointerEnter(PointerEventData eventData)
        {
            Highlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopHighlight();
        }
        
        private void Highlight()
        {
            DOTween.Complete(IdForScale);
            weaponImage.transform.DOScale(Vector3.one * UIConfig.WeaponFrameScaleUp, 0.25f).SetId(IdForScale);
        }

        private void StopHighlight()
        {
            DOTween.Kill(IdForScale);
            weaponImage.transform.DOScale(Vector3.one, 0.25f).SetId(IdForScale);;
        }

        #endregion

        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
        }

        public void OnGet()
        {
        }
    }
}