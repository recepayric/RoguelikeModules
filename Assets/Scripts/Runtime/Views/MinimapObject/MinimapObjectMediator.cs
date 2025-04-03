using Runtime.Model.Interfaces;
using strange.extensions.mediation.impl;

namespace Runtime.Views.MinimapObject
{
    public class MinimapObjectMediator : Mediator
    {
        [Inject] public MinimapObjectView view { get; set; }
        [Inject] public IMinimapModel MinimapModel { get; set; }

        

        public override void OnRegister()
        {
            base.OnRegister();
            MinimapModel.AddMinimapObjectVo(view.minimapObjectVo);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            MinimapModel.RemoveMinimapObjectVo(view.minimapObjectVo);
        }
    }
}
