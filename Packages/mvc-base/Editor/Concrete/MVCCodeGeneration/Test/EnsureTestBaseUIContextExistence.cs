using System;
using System.IO;
using System.Text;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Code;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Test
{
    [CreateAssetMenu(fileName = "EnsureTestBaseUIContextExistence", menuName = "MVC/Admin/Code Generation Operations/EnsureTestBaseUIContextExistence")]
    public class EnsureTestBaseUIContextExistence : CodeGenerationOperation
    <EnsureTestBaseUIContextExistence,
        EnsureTestBaseUIContextExistence.StartArgs,
        EnsureTestBaseUIContextExistence.OperateArgs>
    {
        public struct StartArgs
        {
            
        }
        public struct OperateArgs
        {
            
        }

        protected override void OnBegin(StartArgs arg)
        {
            base.OnBegin(arg);
            DirectoryHelpers.EnsurePathExistence(_sharedSettings.ProjectTestRootPath + "/Base/Context");
        }

        protected override void OnOperate(OperateArgs arg)
        {
            string path = _sharedSettings.ProjectTestRootPath + "/Base/Context/" + "TestBaseUIContext.cs";
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

            string data;
            
            Debug.Log("Ensuring existence of BaseTestUIContext.cs");
            if (obj == null)
            {
                data = LoadTemplate();
                
                CodeUtilities.SaveFile(data, path);
                Debug.Log("File didn't existed generating...");
            }
        }
        
        private string LoadTemplate()
        {
            try
            {
                string data = string.Empty;
                string path = _sharedSettings.TestTemplatePath + "/TemplateTestBaseUIContext" + ".txt";
                StreamReader theReader = new StreamReader(path, Encoding.Default);
                using (theReader)
                {
                    data = theReader.ReadToEnd();
                    theReader.Close();
                }

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}\n", e.Message);
                return string.Empty;
            }
        }
        
        private string LoadFileOnPath(string filePath)
        {
            try
            {
                Debug.Log("Loading File = " + filePath);
                string data = string.Empty;
                string path = filePath;
                StreamReader theReader = new StreamReader(path, Encoding.Default);
                using (theReader)
                {
                    data = theReader.ReadToEnd();
                    theReader.Close();
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return string.Empty;
            }
        }
    }
}