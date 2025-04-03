using MVC.Base.Runtime.Abstract.Key;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Key.UnityObject
{
    [CreateAssetMenu(fileName = "VariableKey", menuName = "MVC/Keys/Variable Key")]
    public class KEY_VariableKey : ScriptableObject,IVariableKey
    {
        [SerializeField]
        private string _id;
        public string ID
        {
            get => _id;
        }
    }
}