using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Model;
using Sirenix.OdinInspector;

namespace MVC.Base.Runtime.Concrete.Model
{
  
  
  public class ScreenModel : IScreenModel
  {
    [PostConstruct]
    public void OnPostConstruct()
    {
        CurrentPanels = new List<IPanelVo>();
        History = new List<IPanelVo>();
    }

    [ShowInInspector]
    [ListDrawerSettings(ShowIndexLabels = true,ListElementLabelName = "Name")]
    public List<IPanelVo> History { get; set; }

    [ShowInInspector]
    public List<IPanelVo> CurrentPanels { get; set; }

    public string GetHistoryData()
    {
      string data = string.Empty;
      foreach (IPanelVo panelVo in History)
      {
        data += panelVo.Name + ",";
      }

      return data;
    }
    
    public bool HasScreenOpened(string key)
    {
      for (int i = 0; i < CurrentPanels.Count; i++)
      {
        if (CurrentPanels[i].Key == key)
        {
          return true;
        }
      }

      return false;
    }
  }
}