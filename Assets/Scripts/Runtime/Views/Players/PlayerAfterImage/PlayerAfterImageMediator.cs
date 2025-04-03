using MVC.Base.Runtime.Abstract.Model;
using strange.extensions.mediation.impl;

namespace Runtime.Views.Players.PlayerAfterImage
{
    public class PlayerAfterImageMediator : Mediator
    {
        [Inject] public PlayerAfterImageView view { get; set; }
        [Inject] public IObjectPoolModel ObjectPoolModel { get; set; }

        private void OnDestroyAfterImage()
        {
	        ObjectPoolModel.Return(gameObject);
        }
        
		private void OnGetFromPool()
		{
			
		}
		private void OnReturnToPool()
		{
			
		}

        public override void OnRegister()
        {
            base.OnRegister();
			view.GetFromPool+=OnGetFromPool;
			view.ReturnToPool+=OnReturnToPool;
			view.DestroyAfterimageEvent += OnDestroyAfterImage;
        }

        public override void OnRemove()
        {
            base.OnRemove();
			view.GetFromPool-=OnGetFromPool;
			view.ReturnToPool-=OnReturnToPool;
			view.DestroyAfterimageEvent -= OnDestroyAfterImage;
        }
    }
}
