using MVC.Base.Runtime.Concrete.Context;

namespace MVC.Base.Runtime.Concrete.Root
{
    public class DefaultUICanvasRoot : MVCContextRoot
    {
        protected override void InitializeContext()
        {
            context = new BaseUIContext(gameObject);
            context.Start();
            context.Launch();
        }
    }
}