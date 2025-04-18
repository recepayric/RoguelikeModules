﻿using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Test
{
    [CreateAssetMenu(fileName = "CreateSceneGenerationOperation", menuName = "MVC/Admin/Code Generation Operations/Scene Generation Operation")]
    public class CreateSceneGenerationOperation
        : CodeGenerationOperation<
            CreateSceneGenerationOperation,
            CreateSceneGenerationOperation.StartArgs,
            CreateSceneGenerationOperation.OperateArgs>
    {
        public struct StartArgs
        {
            public string Name;
            public string Type;
        }

        public struct OperateArgs
        {
            
        }

        private string _testRootPath;
        private string _name;
        private string _type;
        protected override void OnBegin(StartArgs arg)
        {
            base.OnBegin(arg);
            _name = arg.Name;
            _type = arg.Type;
            //Creating root folder for testing screens.
            _testRootPath = DirectoryHelpers.CreateOrGetFolderPath(_sharedSettings.ProjectTestRootPath + _type,_name);
        }

        protected override void OnOperate(OperateArgs arg)
        {
            CreateScene();
        }
        
        private void CreateScene()
        {
            //Create Scene
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, _testRootPath + "/" + _name + _type + "Test.unity");
            UnityEngine.Object sceneAsset =
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(_testRootPath + "/" + _name + _type + "Test.unity");
            AssetDatabase.SetLabels(sceneAsset, new string[] { "Exclude" });
        }
    }
}