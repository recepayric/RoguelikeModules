using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.UIRelated.RawCameraRelated
{
    public class CharacterMannqueen : MonoBehaviour, IPoolObject
    {
        private GameObject selecterWeapon;
        public GameObject weaponHolder;
    
        public void SetWeapon(PoolKeys poolKeys)
        {
            if(selecterWeapon != null)
                BasicPool.instance.Return(selecterWeapon);
        
            var weapon = BasicPool.instance.Get(poolKeys);
            weapon.transform.SetParent(weaponHolder.transform);

            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localScale = Vector3.one;
            weapon.transform.localRotation = Quaternion.identity;
            selecterWeapon = weapon;
        }

        private void RemoveWeapon()
        {
            if(selecterWeapon != null)
                BasicPool.instance.Return(selecterWeapon);
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            RemoveWeapon();
            //EventManager.Instance.WeaponSelectChangedEvent -= OnWeaponSelectChanged;
        }

        public void OnGet()
        {
            //EventManager.Instance.WeaponSelectChangedEvent += OnWeaponSelectChanged;
        }
    }
}