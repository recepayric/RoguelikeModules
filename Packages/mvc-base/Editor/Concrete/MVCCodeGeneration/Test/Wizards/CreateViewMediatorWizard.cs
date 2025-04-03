using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants;
using UnityEditor;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Wizards
{
    public enum CreateViMaType
    {
        View,
        Manager
    }
    public class CreateViewMediatorWizard : ScriptableWizard
    {
        public CreateViMaType ViewType = CreateViMaType.View;

        public string Name = string.Empty;

        public string[] EventList;
        
        
        private string _scriptsPath;

        private string _controllersPath;

        private string _viewPath;
        
        private string _contextPath;
        
        [MenuItem("Tools/MVC/Create View Mediator",false,11)]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Create View Mediator", typeof(CreateViewMediatorWizard), "Create");
        }
        
        private static CodeGenerationSettings _settings;
        private CodeGenerationOperationConfig _operationConfig;
        
        [UsedImplicitly]
        private void OnWizardCreate()
        {
            if (string.IsNullOrEmpty(Name))
                return;

            _settings = AssetDatabase.LoadAssetAtPath<CodeGenerationSettings>(CodeGenPaths.SETTINGS_PATH);
            _operationConfig = _settings.ViewManagerCodeGenerationConfig;

            //Create necessary start args.
            Dictionary<Type, object> startArgs = new Dictionary<Type, object>();

    
            startArgs.Add(typeof(ViewManagerMediatorGenerationOperation), new ViewManagerMediatorGenerationOperation.StartArgs()
                {
                    Name = this.Name,
                    Type = this.ViewType,
                    ViewEventList = this.EventList
                }
            );
            
            foreach (CodeGenerationOperation operation in _operationConfig.Operations)
            {
                operation.Begin(startArgs);
            }
            
            //Create necessary operate args.
            Dictionary<Type, object> operateArgs = new Dictionary<Type, object>()
            {
                
            };
            
            foreach (CodeGenerationOperation operation in _operationConfig.Operations)
            {
                operation.Operate(operateArgs);
            }

            //This is for InitializeOnLoad code above.
            EditorPrefs.SetBool("CreateViewMediatorWizard", true);
            EditorPrefs.SetString("Name", Name);
            EditorPrefs.SetString("ViewType", ViewType.ToString());

            AssetDatabase.Refresh();
        }
    }
}