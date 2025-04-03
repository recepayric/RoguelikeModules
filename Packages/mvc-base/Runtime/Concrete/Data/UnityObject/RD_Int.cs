using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Integer", order = 1)]
    public class RD_Int : ScriptableObject
    {
        public int Value;
    }
}