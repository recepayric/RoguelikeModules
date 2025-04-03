using MVC.Base.Runtime.Abstract.Model;

namespace MVC.Base.Runtime.Abstract.View
{
    public class MVCPoolView : MVCView
    {
        public override bool isInjectable => true;

        protected override void Awake()
        {
            base.Awake();
        }

        [Inject] public IObjectPoolModel Pool { get; set; }
    }
}