using MVC.Base.Runtime.Abstract.Function;
using UnityEngine;

namespace MVC.Base.Runtime.Utility
{
    public class BasePoolObject : MonoBehaviour ,IPoolable
    {
        public void OnGetFromPool()
        {
        }

        public void OnReturnToPool()
        {
        }

        public string PoolKey { get; set; }
    }
}
