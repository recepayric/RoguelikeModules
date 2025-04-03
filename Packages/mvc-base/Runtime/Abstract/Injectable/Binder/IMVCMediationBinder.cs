using strange.extensions.mediation.api;
using UnityEngine;

namespace MVC.Base.Runtime.Abstract.Injectable.Binder
{
    public interface IMVCMediationBinder : IMediationBinder
    {

        void ActivateRoot(GameObject root);
    }
}