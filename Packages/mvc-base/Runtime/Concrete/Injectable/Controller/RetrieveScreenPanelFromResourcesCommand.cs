using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Injectable.Provider;
using MVC.Base.Runtime.Concrete.Injectable.Process;
using strange.extensions.command.impl;

namespace MVC.Base.Runtime.Signals
{
    public class RetrieveScreenPanelFromResourcesCommand : Command
    {
        [Inject] public IPanelVo ParamPanelVo {get;set;} //Set by signal
        [Inject] public bool ParamSyncLoad {get;set;} //Set by signal
        [Inject] public IProcessProvider ProcessProvider{get;set;}

        public override void Execute()
        {
            if (!ParamSyncLoad)
            {
                RetrieveResourcesPanelProcess process = ProcessProvider.Get<RetrieveResourcesPanelProcess>();
                process.PanelVo = ParamPanelVo;
                process.AutoReturn = true;
                process.Start();
            }
            else
            {
                RetrieveResourcesPanelSyncProcess panelSyncProcess = ProcessProvider.Get<RetrieveResourcesPanelSyncProcess>();
                panelSyncProcess.PanelVo = ParamPanelVo;
                panelSyncProcess.Start();
            }
        }
    }
}