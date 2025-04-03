using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Wizards
{
    [InitializeOnLoad]
    public class CreateTestRootContextListener
    {
        private static CodeGenerationSettings _settings;
        
        static CreateTestRootContextListener()
        {
            if (EditorPrefs.GetBool("CreateTestRootContextWizard"))
            {
                EditorPrefs.SetBool("CreateTestRootContextWizard", false);

                string name = EditorPrefs.GetString("Name");
                string testType = EditorPrefs.GetString("Type");
                EditorPrefs.DeleteKey("Name");
                EditorPrefs.DeleteKey("Type");
                
                _settings = AssetDatabase.LoadAssetAtPath<CodeGenerationSettings>(CodeGenPaths.SETTINGS_PATH);
                
                //Instantiate root GameObject.
                if(!TryInstantiateRootInstance(name,testType,out GameObject instanceRootGameObject))
                {
                    Debug.LogError("Could not instantiate root instance!");
                    return;
                }
                
                EditorSceneManager.MarkAllScenesDirty();
                EditorSceneManager.SaveOpenScenes();
            }
        }

        private static bool TryInstantiateRootInstance(string panelName, string testType,out GameObject rootInstance)
        {
            Object root = AssetDatabase.LoadAssetAtPath(_settings.TemplatePrefabPath + "/" + "ScreenRootTemplate.prefab", typeof(GameObject));
            rootInstance = PrefabUtility.InstantiatePrefab(root) as GameObject;
            
            if(rootInstance == null)
                return false;

            PrefabUtility.UnpackPrefabInstance(rootInstance, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            rootInstance.name = panelName + "Root";
            
            //Adding root component.
            Type type = GetTypeFromName(panelName + testType + "TestRoot");
            Component rootComponent = rootInstance.AddComponent(type);
            
            return true;
        }

        private static Type GetTypeFromName(string classNameWithNameSpace)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = a.GetTypes();
                foreach (Type type in types)
                {
                    if (type.AssemblyQualifiedName != null && type.AssemblyQualifiedName.Contains(classNameWithNameSpace))
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }
    
    public class CreateTestRootContextWizard : ScriptableWizard
    {
        private string _type = "Screens";
        
        public string Name = string.Empty;
        
        private string _testScriptsPath;

        private string _testControllersPath;

        private string _testRootPath;

        private string _keyPath;

        [MenuItem("Tools/MVC/Create Test Root And Context",false,50)]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Create Test Root And Context", typeof(CreateTestRootContextWizard), "Create");
        }

        private static CodeGenerationSettings _settings;
        private CodeGenerationOperationConfig _operationConfig;

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            if (string.IsNullOrEmpty(Name))
                return;

            _settings = AssetDatabase.LoadAssetAtPath<CodeGenerationSettings>(CodeGenPaths.SETTINGS_PATH);
            _operationConfig = _settings.TestRootAndContextCodeGenerationConfig;

            //Create necessary start args.
            Dictionary<Type, object> startArgs = new Dictionary<Type, object>()
            {
                {
                    typeof(TestRootContextGenerationOperation),
                    new TestRootContextGenerationOperation.StartArgs()
                    {
                        Name = this.Name,
                        Type = _type,
                        Empty = true
                    }
                },
                {
                    typeof(CreateSceneGenerationOperation),
                    new CreateSceneGenerationOperation.StartArgs()
                    {
                        Name = this.Name
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
            EditorPrefs.SetBool("CreateTestRootContextWizard", true);
            EditorPrefs.SetString("Name", Name);
            EditorPrefs.SetString("Type", _type);

            AssetDatabase.Refresh();
        }
    }
}