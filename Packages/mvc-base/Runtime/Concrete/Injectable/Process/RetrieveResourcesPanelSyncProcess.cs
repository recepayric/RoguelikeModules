using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Injectable.Process;
using MVC.Base.Runtime.Signals;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Injectable.Process
{
   
    public class RetrieveResourcesPanelSyncProcess : ContinuousProcess
    {
        [Inject] public ScreenSignals ScreenSignals{get;set;}

        public IPanelVo PanelVo;
        protected override void DoStart()
        {
            var asset = Resources.Load<GameObject>("Screens/" + PanelVo.Key);
            if (asset == null)
            {
                Debug.LogError("LoadFromResources! Panel not found!! " + PanelVo.Key);
            }
            ScreenSignals.AfterRetrievedPanel.Dispatch(PanelVo,asset);
        }
    }
}