using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Key;
using MVC.Base.Runtime.Abstract.Data.ValueObject;

namespace MVC.Base.Runtime.Abstract.Model
{
    public interface IScreenModel
    {
        /// <summary>
        /// Panels list on history
        /// </summary>
        List<IPanelVo> History { get; set; }
        
        List<IPanelVo> CurrentPanels {get;set;}
        bool HasScreenOpened(string key);
    }
}