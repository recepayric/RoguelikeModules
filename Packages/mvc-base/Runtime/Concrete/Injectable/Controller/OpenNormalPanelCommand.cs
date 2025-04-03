using MVC.Base.Runtime.Concrete.Data.ValueObject;
using strange.extensions.command.impl;
using UnityEngine;

namespace MVC.Base.Runtime.Signals
{
    public class OpenNormalPanelCommand : Command
    {
        [Inject]
        public ScreenSignals ScreenSignals { get; set; }
        
        [Inject]
        public OpenNormalPanelArgs ParamArgs { get; set; }
        
        public override void Execute()
        {
            Debug.Log("Opening Normal Panel " + ParamArgs.PanelKey);
            PanelVo panelVo = new PanelVo()
            {
                Key = ParamArgs.PanelKey,
                LayerIndex = ParamArgs.LayerIndex,
                Name = ParamArgs.PanelKey,
                IgnoreHistory = ParamArgs.IgnoreHistory
            };
            ScreenSignals.DisplayPanel.Dispatch(panelVo);
        }
    }
}