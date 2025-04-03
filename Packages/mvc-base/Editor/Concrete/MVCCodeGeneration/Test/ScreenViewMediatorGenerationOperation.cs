using System;
using System.IO;
using System.Text;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Code;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Test
{
    [CreateAssetMenu(fileName = "ScreenViewMediatorGenerationOperation", menuName = "MVC/Admin/Code Generation Operations/ScreenViewMediatorGenerationOperation")]
    public class ScreenViewMediatorGenerationOperation 
        : CodeGenerationOperation<
            ScreenViewMediatorGenerationOperation,
            ScreenViewMediatorGenerationOperation.StartArgs,
            ScreenViewMediatorGenerationOperation.OperateArgs>
    {
        public struct StartArgs
        {
            public string Name;
            public string Type;
            public string[] ViewEventList;
        }

        public struct OperateArgs
        {
            
        }
        
        private string _type = "Screens";
        private string _name = "";
        
        //Placeholder definitions.
        private const string NamespacePlaceholder = "%TemplateNS%";
        private const string TemplatePlaceholder = "%Template%";
        private const string ClassnamePlaceholder = "%ClassName%";
        private const string TypePlaceholder = "%Type%";
        private const string UnityActionPlaceholder = "%UnityAction%";
        private const string AddListenerPlaceholder = "%AddListener%";
        private const string RemoveListenerPlaceholder = "%RemoveListener%";
        private const string FunctionPlaceholder = "%ListenerFunction%";
        
        private const string ViewUnityActionTemplate = "public event UnityAction {0};\n";
        private const string MediatorFunctionTemplate = "private void On{0}()\r\t\t{{\r\t\t\t\r\t\t}}";
        
        private const string AddListenerTemplate = "view.{0}+=On{0};";
        private const string RemoveListenerTemplate = "view.{0}-=On{0};";
        
        private const string ViewFunctionTemplate =
            "public void On{0}()\r\t\t{{\r\t\t\t{0}?.Invoke();\r\t\t}}";
        
        private string[] _viewEventList;
        
        protected override void OnBegin(StartArgs startArg)
        {
            _viewEventList = startArg.ViewEventList;
            _type = startArg.Type;
            _name = startArg.Name;
            
            //Ensuring existance of mediator path.
            DirectoryHelpers.CreateOrGetFolderPath(_sharedSettings.ProjectMediatorPath, _type);
            
            //Ensuring existance of view path.
            DirectoryHelpers.CreateOrGetFolderPath(_sharedSettings.ProjectViewPath, _type);            
        }

        protected override void OnOperate(OperateArgs startArg)
        {
            CreateViewScript();
            CreateMediatorScript();
        }
        private void CreateViewScript()
        {
            //Creating View File
            Debug.Log("Creating View File");

            var data = LoadTemplate(TemplateType.ScreenView);
            string scriptPathNamespace = (_sharedSettings.ProjectViewPath + "/" + _type).Replace("/", ".") + ".";
            string namespaceValue = "Runtime.Views."+_type;
            Debug.Log("Namespace = " + namespaceValue);
            
            //Set Namespaces for view
            data = data.Replace(NamespacePlaceholder, namespaceValue);
            
            //Set Class name for view
            data = data.Replace(TemplatePlaceholder, _name + _type);
            
            
            data = data.Replace(TypePlaceholder, _type);
            
            ReplacePlaceholderWithTemplatedEventList(ref data, UnityActionPlaceholder, ViewUnityActionTemplate,
                _viewEventList);
            
            ReplacePlaceholderWithTemplatedEventList(ref data, FunctionPlaceholder, ViewFunctionTemplate,
                _viewEventList);
            
            Debug.Log("Created Data \n" + data);

            CodeUtilities.SaveFileCheckAndReplaceName(data, _sharedSettings.ProjectViewPath + "/" + _type + "/" + _name + _type + "View.cs");
        }
        
        private void CreateMediatorScript()
        {
            //Creating Mediator File
            Debug.Log("Creating Mediator File");
            
            var data = LoadTemplate(TemplateType.Mediator);
            string scriptPathNamespace = (_sharedSettings.ProjectViewPath + "/" + _type).Replace("/", ".") + ".";
            string namespaceValue = "Runtime.Views."+_type;
            
            data = data.Replace(NamespacePlaceholder, namespaceValue);
            
            data = data.Replace(TemplatePlaceholder, _name + _type + "View");
            data = data.Replace(ClassnamePlaceholder, _name + _type);
            
            data = data.Replace(TypePlaceholder, _type);
            
            //View listener registering
            ReplacePlaceholderWithTemplatedEventList(ref data,AddListenerPlaceholder,AddListenerTemplate,_viewEventList);
            
            //View listener unregistering
            ReplacePlaceholderWithTemplatedEventList(ref data,RemoveListenerPlaceholder,RemoveListenerTemplate,_viewEventList);
            
            //Mediator functions that are subscribed to view events.
            ReplacePlaceholderWithTemplatedEventList(ref data,FunctionPlaceholder,MediatorFunctionTemplate,_viewEventList);

            CodeUtilities.SaveFileCheckAndReplaceName(data, _sharedSettings.ProjectMediatorPath + "/" + _type + "/" + _name + _type + "Mediator.cs");
        }
        
        private void ReplacePlaceholderWithTemplatedEventList(ref string data, string placeholder, string template, string[] eventList)
        {
            //This is where we add actions.
            if (eventList.Length == 0)
                data = data.Replace(placeholder, "");
            else
            {
                string functions = "";
                for (int i = 0; i < eventList.Length; i++)
                {
                    functions += string.Format(template, eventList[i]);
                    if (i < eventList.Length - 1)
                        functions += "\r\t\t\r\t\t";
                }

                data = data.Replace(placeholder, functions);
            }
        }
        
        private string LoadTemplate(TemplateType type)
        {
            try
            {
                string data = string.Empty;
                string path = _sharedSettings.TestTemplatePath + "/Template" + type + ".txt";
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