using MVC.Base.Runtime.Concrete.Context;
using MVC.Base.Runtime.Extensions;
using Runtime.Model;
using Runtime.Model.Interfaces;
using Runtime.Views.Players.PlayerAfterImage;

namespace Runtime.Context
{
    public class PlayerContexts
    {
        public void BindModels(MVCContext context)
        {
            context.injectionBinder.BindCrossContextSingletonSafely<IPlayerModel, PlayerModel>();
        }
        
        public void BindViews(MVCContext context)
        {
            context.mediationBinder.Bind<PlayerAfterImageView>().To<PlayerAfterImageMediator>();
        }
    }
}