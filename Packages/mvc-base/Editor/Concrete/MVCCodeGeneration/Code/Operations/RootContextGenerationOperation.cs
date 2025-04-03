using System;
using System.IO;
using System.Text;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Code.Operations
{
    [CreateAssetMenu(fileName = "RootContextGenerationOperation", menuName = "MVC/Admin/Code Generation Operations/RootContextGenerationOperation")]
    public class RootContextGenerationOperation  : CodeGenerationOperation<
        RootContextGenerationOperation,
        RootContextGenerationOperation.StartArgs,
        RootContextGenerationOperation.OperateArgs>
    {
        public struct StartArgs
        {
            public string Name;
        }

        public struct OperateArgs
        {
            
        }

        private string _name;
        private string _contextPath;
        private string _rootPath;
        private const string NamespacePlaceholder = "%TemplateNS%";
        private const string ClassnamePlaceholder = "%Template%";

        protected override void OnBegin(StartArgs arg)
        {
            _name = arg.Name;
            _contextPath = DirectoryHelpers.CreateOrGetFolderPath(_sharedSettings.ProjectContextPath , "");
            _rootPath = DirectoryHelpers.CreateOrGetFolderPath(_sharedSettings.ProjectRootPath , "");
        }

        protected override void OnOperate(OperateArgs arg)
        {
            AddScript(_contextPath, _name, TemplateType.Context, "Context");
            AddScript(_rootPath, _name, TemplateType.Root, "Root");
        }
        
        private void AddScript(string path, string SName, TemplateType templateType, string testType)
        {
            var data = LoadTemplate(templateType);
            data = data.Replace(NamespacePlaceholder, "Runtime."+testType);
            data = data.Replace(ClassnamePlaceholder, SName);
            CodeUtilities.SaveFileCheckAndReplaceName(data, path + "/" + SName + testType +".cs");

            UnityEngine.Object rootScript = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path + "/" + SName + testType + "Test" + templateType + ".cs");
            AssetDatabase.SetLabels(rootScript, new string[] { "Exclude" });
        }
        
        private string LoadTemplate(TemplateType type)
        {
            try
            {
                string data = string.Empty;
                string path = _sharedSettings.CodeTemplatePath + "/Template" + type + ".txt";
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
    }
}