using System;
using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Abstract.View;
using MVC.Base.Runtime.Concrete.Views;
using MVC.Base.Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

//Currently multi-screen manager is not supported, only have one of this in your app.
namespace MVC.Base.Runtime.Concrete.Injectable.Mediators
{
    public class ScreenManagerMediator : Mediator
    {
        [Inject] public ScreenManager Manager { get; set; }

        [Inject] public IScreenModel ScreenModel { get; set; }

        [Inject] public ScreenSignals ScreenSignals{get;set;}

        private List<GameObject> _panels;
        /// <summary>
        /// Works after all bindings are completed. 
        /// Useful to attach listeners
        /// After Awake 
        /// Before Start. 
        /// </summary>
        public override void OnRegister()
        {
            ScreenSignals.AfterRetrievedPanel.AddListener(CreatePanel);
            ScreenSignals.DisplayPanel.AddListener(OnDisplayPanel);
            ScreenSignals.ClearLayerPanel.AddListener(OnClearLayer);
            ScreenSignals.GoBackScreen.AddListener(OnBack);
            ScreenSignals.CloseSplashLayer.AddListener(OnCloseSplashLayer);

            _panels = new List<GameObject>();
            foreach (Transform layer in Manager.Layers)
            {
                foreach (Transform panel in layer)
                {
                    _panels.Add(panel.gameObject);
                }
            }
        
            //Debug.Log(GetType() + " registered with context");
        }

        /// <summary>
        /// Remove the current page. Check the previous page and load it.
        /// </summary>
        private void OnBack()
        {
            if (ScreenModel.History.Count < 2)
                return;

            ScreenModel.History.RemoveAt(ScreenModel.History.Count - 1);
            IPanelVo prePanelVo = ScreenModel.History[ScreenModel.History.Count - 1];
            ScreenModel.History.RemoveAt(ScreenModel.History.Count - 1);

            //Creating signal argument
            ScreenSignals.DisplayPanel.Dispatch(prePanelVo);
        }

        /// <summary>
        /// Receives the display panel request
        /// </summary>
        private void OnDisplayPanel(IPanelVo panelVo)
        {
            if (panelVo.Key == null)
            {
                Debug.LogError("Panel is null");
                return;
            }

            RetrievePanel(panelVo);
        }

        /// <summary>
        /// Checks if the display panel request is valid and raises retrieve signal.
        /// </summary>
        private void RetrievePanel(IPanelVo panelVo)
        {
            if (panelVo.LayerIndex >= Manager.Layers.Length)
            {
                Debug.LogError("There is no layer " + panelVo.LayerIndex);
                return;
            }

            ScreenSignals.RetrievePanel.Dispatch(panelVo,Manager.SyncLoad);
        }

        /// <summary>
        /// Remove the last screen added by name
        /// </summary>
        /// <param name="panelVo"></param>
        private void RemoveFromHistoryByNameFromLast(string name)
        {
            for (int i = ScreenModel.History.Count - 1; i > 0; i--)
            {
                if (name == ScreenModel.History[i].Name)
                {
                    ScreenModel.History.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Create the panel and set the transform of gameobject
        /// </summary>
        /// <param name="vo"> PanelVo which is stored on View objects, if it is a screen </param>
        /// <param name="template"> Prefab to create </param>
        private void CreatePanel(IPanelVo panelVo, GameObject template)
        {
        
            if (panelVo.RemoveSamePanels)
                RemoveSamePanels(panelVo.Key,panelVo.LayerIndex);

            if (panelVo.RemoveLayer)
                RemoveLayer(panelVo.LayerIndex);

            if (panelVo.RemoveAll)
                RemoveAllPanels();
        
            //This can be a pool!
            GameObject newPanel = Instantiate(template,Manager.Layers[panelVo.LayerIndex]);
            IPanel panel = newPanel.GetComponent<IPanel>();
            MVCView view = panel as MVCView;
            if(view == null)
            {
                Debug.LogError("This is not a view!",newPanel);
                return;
            }
            panel.vo = panelVo;
            view.Initialize();
        
            newPanel.transform.SetParent(Manager.Layers[panelVo.LayerIndex], false);
            newPanel.transform.localScale = Vector3.one;

            _panels.Add(newPanel);

            if (!panelVo.IgnoreHistory)
                ScreenModel.History.Add(panel.vo);

            ScreenModel.CurrentPanels.Add(panel.vo);
            //Debug.Log("---------------------" + vo.Type);
        }

        /// <summary>
        /// Used to prevent having same panels on a layer
        /// </summary>
        private void RemoveSamePanels(string key, int layerIndex)
        {
            foreach (Transform child in Manager.Layers[layerIndex].transform)
            {
                ScreenModel.CurrentPanels.RemoveAll(vo=>vo.Key == key);
                //if (App.Status.value.HasFlag(child.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= child.GetComponent<IPanelView>().vo.Type;

                int index = child.name.IndexOf(key, StringComparison.Ordinal);
                if (index != -1)
                {
                    Destroy(child.gameObject);
                    RemoveFromHistoryByNameFromLast(key);
                }
            }
        }

        /// <summary>
        /// Clear all the gameobjecs on the given layer
        /// </summary>
        private void OnClearLayer(int layer)
        {
            RemoveLayer(layer);
        }

        /// <summary>
        /// Clear all gameobjects on layer. Called when loading a new screen
        /// </summary>
        private void RemoveLayer(int voLayerIndex)
        {
            foreach (Transform panel in Manager.Layers[voLayerIndex].transform)
            {
                ScreenModel.CurrentPanels.Remove(panel.GetComponent<IPanel>().vo);
                //if (App.Status.value.HasFlag(panel.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= panel.GetComponent<IPanelView>().vo.Type;

                Destroy(panel.gameObject);
                _panels.Remove(panel.gameObject);
            }
        }

        /// <summary>
        /// Clear all panels on all layers
        /// </summary>
        private void RemoveAllPanels()
        {
            foreach (var panel in _panels)
            {
                ScreenModel.CurrentPanels.Remove(panel.GetComponent<IPanel>().vo);
                //if (App.Status.value.HasFlag(panel.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= panel.GetComponent<IPanelView>().vo.Type;

                Destroy(panel);
            }

            _panels.Clear();
        }
        
        /// <summary>
        /// Closing splash layer that has not any IPanelView or related mediator.
        /// </summary>
        private void OnCloseSplashLayer()
        {
            if (Manager.CloseSplashScreen())
                ScreenSignals.CloseSplashLayer.RemoveListener(OnCloseSplashLayer);
        }
        /// <summary>
        /// Works when connected gameobject is destroyed. 
        /// Useful to remove listeners
        /// Before OnDestroy method
        /// </summary>
        public override void OnRemove()
        {
            ScreenSignals.AfterRetrievedPanel.RemoveListener(CreatePanel);
            ScreenSignals.DisplayPanel.RemoveListener(OnDisplayPanel);
            ScreenSignals.ClearLayerPanel.RemoveListener(OnClearLayer);
            ScreenSignals.GoBackScreen.RemoveListener(OnBack);
            ScreenSignals.CloseSplashLayer.RemoveListener(OnCloseSplashLayer);
        }
    }
}