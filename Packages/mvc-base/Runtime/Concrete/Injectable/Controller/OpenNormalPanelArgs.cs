namespace MVC.Base.Runtime.Signals
{
    [System.Serializable]
    public struct OpenNormalPanelArgs
    {
        public OpenNormalPanelArgs(string panelKey ,bool iIgnoreHistory = false)
        {
            PanelKey = panelKey;
            LayerIndex = 0;
            IgnoreHistory = iIgnoreHistory;
        }
        
        public OpenNormalPanelArgs(string panelKey,int layerIndex ,bool iIgnoreHistory = false)
        {
            PanelKey = panelKey;
            LayerIndex = layerIndex;
            IgnoreHistory = iIgnoreHistory;
        }
        
        
        public string PanelKey;
        public int LayerIndex;
        public bool IgnoreHistory;
    }
}