using System.Collections.Generic;
using UnityEngine.Events;
using MVC.Base.Runtime.Abstract.View;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Views.Minimap
{
    public class MinimapView : MVCView
    {
        public RectTransform minimapContainer;
        public List<RectTransform> baseObjects;
        public Image playerIcon;
        public List<Image> enemyIcons;
        
    }
}
