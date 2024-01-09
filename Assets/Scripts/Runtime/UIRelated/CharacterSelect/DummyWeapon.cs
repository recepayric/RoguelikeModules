using Runtime.Enums;
using UnityEngine;

namespace Runtime.UIRelated.CharacterSelect
{
    public class DummyWeapon : MonoBehaviour, IPoolObject
    {
        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
            
        }

        public void OnGet()
        {
        }
    }
}