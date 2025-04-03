using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/String", order = 0)]
    public class RD_String : ScriptableObject
    {
        public string Value;
    }
}