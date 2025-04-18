﻿using MVC.Base.Runtime.Abstract.Data.ValueObject;
using strange.extensions.signal.impl;
using UnityEngine;

namespace MVC.Base.Runtime.Signals
{
    public class ScreenSignals
    {
        //Future Idea: To support multi RichScreenManagers we should also send a key, and screen managers can be assigned to that key and Panel would only open on that specific Canvas.
        //Should be bound to command that is going to configure PanelVo.
        public Signal<OpenNormalPanelArgs> OpenPanel = new Signal<OpenNormalPanelArgs>();
        //Listened by RichScreenManagerMediator
        public Signal<IPanelVo> DisplayPanel = new Signal<IPanelVo>();
        //Binded to a command that starts the process of recieving the Panel.
        public Signal<IPanelVo,bool> RetrievePanel = new Signal<IPanelVo,bool>();
        //Listened by RichScreenManagerMediator
        public Signal<int> ClearLayerPanel = new Signal<int>();
        //Sent by Process to retriever of the panel. Listened by RichScreenManagerMediator, After this, panel should be displayed
        public Signal<IPanelVo,GameObject> AfterRetrievedPanel = new Signal<IPanelVo, GameObject>();
        //Listened by RichScreenManagerMediator
        public Signal GoBackScreen = new Signal();
        //Close splash layer
        public Signal CloseSplashLayer = new Signal();
    }
}