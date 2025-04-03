using System;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Code.ContextList
{
  [Serializable]
  public class ContextVo
  {
    [SerializeField, AppContext] public int Context;

    [HideInInspector] public bool visible;
  }
}