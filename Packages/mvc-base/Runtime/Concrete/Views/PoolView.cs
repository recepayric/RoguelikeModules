using MVC.Base.Runtime.Abstract.Model;
using strange.extensions.mediation.impl;

namespace MVC.Base.Runtime.Concrete.Views
{
    /// <summary>
    /// If pool access needed in any ...View script. This view should be derived from this script.
    /// </summary>
    public class PoolView : EventView
    {
        /// <summary>
        /// Pool model
        /// </summary>
        [Inject] public IObjectPoolModel pool { get; set; }
    }
}

