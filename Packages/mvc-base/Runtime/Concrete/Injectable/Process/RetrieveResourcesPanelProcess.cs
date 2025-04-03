using System.Collections;
using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Injectable.Process;
using MVC.Base.Runtime.Signals;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Injectable.Process
{
    public class RetrieveResourcesPanelProcess : CoroutineProcess<RetrieveResourcesPanelProcess>
    {
        [Inject] public ScreenSignals ScreenSignals{get;set;}
    
        public IPanelVo PanelVo;
    
        protected override IEnumerator Routine()
        {
            ResourceRequest request = Resources.LoadAsync("Screens/" + PanelVo.Key, typeof(GameObject));
            yield return request;
            if (request.asset == null)
            {
                Debug.LogError("LoadFromResources! Panel not found!! " + PanelVo.Key);
                yield break;
            }
            ScreenSignals.AfterRetrievedPanel.Dispatch(PanelVo,request.asset as GameObject);
        }
    }
}