﻿using strange.extensions.context.api;

namespace MVC.Base.Runtime.Concrete.Handler.UnityObject
{
    public class ViewOperationHandler
    {
        private IContext _context;
        public static IContext FirstMVCContext
        {
            get => _instance._context;
        }
        
        public static void Init(IContext context)
        {
            if (_instance == null)
            {
                _instance = new ViewOperationHandler { _context = context};
            }
        }
        
        private static ViewOperationHandler _instance;
    }
}