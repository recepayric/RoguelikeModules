using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.UIRelated.CharacterSelect
{
    public class CharacterMannqueen : MonoBehaviour, IPoolObject
    {
        private GameObject selecterWeapon;
        public GameObject weaponHolder;
    
        public void OnWeaponSelectChanged(PoolKeys poolKeys)
        {
            if(selecterWeapon != null)
                BasicPool.instance.Return(selecterWeapon);
        
            var weapon = BasicPool.instance.Get(poolKeys);
            weapon.transform.SetParent(weaponHolder.transform);

            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            selecterWeapon = weapon;
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            EventManager.Instance.WeaponSelectChangedEvent -= OnWeaponSelectChanged;
        }

        public void OnGet()
        {
            EventManager.Instance.WeaponSelectChangedEvent += OnWeaponSelectChanged;
        }
    }
}