using strange.extensions.command.impl;

namespace MVC.Base.Runtime.Signals
{
    public class SROptionsModelRegisterCommand<TModel> : Command where TModel : class
    {
        [Inject]
        public TModel Model { get; set; }

        public override void Execute()
        {
            if (SRDebug.Instance.Settings.IsEnabled)
                SRDebug.Instance.AddOptionContainer(Model);
        }
    }
}