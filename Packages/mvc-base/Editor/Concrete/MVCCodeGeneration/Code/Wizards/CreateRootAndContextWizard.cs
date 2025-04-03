using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Code.Operations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants;
using UnityEditor;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Code.Wizards
{
    public class CreateRootAndContextWizard : ScriptableWizard
    {
        public string Name = string.Empty;
        
        private string _testScriptsPath;

        private string _testControllersPath;

        private string _testRootPath;

        private string _keyPath;

        [MenuItem("Tools/MVC/Create Root And Context",false,10)]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Create Root And Context", typeof(CreateRootAndContextWizard), "Create");
        }

        private static CodeGenerationSettings _settings;
        private CodeGenerationOperationConfig _operationConfig;

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            if (string.IsNullOrEmpty(Name))
                return;

            _settings = AssetDatabase.LoadAssetAtPath<CodeGenerationSettings>(CodeGenPaths.SETTINGS_PATH);
            _operationConfig = _settings.RootAndContextCodeGenerationConfig;

            //Create necessary start args.
            Dictionary<Type, object> startArgs = new Dictionary<Type, object>()
            {
                {
                    typeof(RootContextGenerationOperation),
                    new RootContextGenerationOperation.StartArgs()
                    {
                        Name = this.Name,
                    }
                }
            };
            
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
            EditorPrefs.SetBool("CreateRootAndContextWizard", true);
            EditorPrefs.SetString("Name", Name);

            AssetDatabase.Refresh();
        }
    }
}