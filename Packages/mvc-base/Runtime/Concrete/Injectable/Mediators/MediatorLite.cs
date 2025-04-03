using strange.extensions.context.api;
using strange.extensions.mediation.api;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Injectable.Mediators
{
	[System.Serializable]
    public class MediatorLite : IMediator
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView{get;set;}

        public bool _isRegistered;
        public bool _isInited;
        
        public MediatorLite ()
        {
        }

        /**
		 * Fires directly after creation and before injection
		 */
        virtual public void PreRegister()
        {
        }

        /**
		 * Fires after all injections satisifed.
		 *
		 * Override and place your initialization code here.
		 */
        virtual public void OnRegister()
        {
	        _isRegistered = true;
	        _isInited = true;
        }

        /**
		 * Fires on removal of view.
		 *
		 * Override and place your cleanup code here
		 */
        virtual public void OnRemove()
        {
	        _isRegistered = false;
        }

        /**
		 * Fires on enabling of view.
		 */
        virtual public void OnEnabled()
        {
	        if(!_isRegistered && _isInited)
		        OnRegister();

        }

        /**
		 * Fires on disabling of view.
		 */
        virtual public void OnDisabled()
        {
	        if(_isRegistered && _isInited)
		        OnRemove();
        }
    }
}