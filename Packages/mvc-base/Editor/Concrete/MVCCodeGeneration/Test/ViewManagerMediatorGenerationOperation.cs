using System;
using System.IO;
using System.Text;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Code;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants;
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Wizards;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Test
{
    [CreateAssetMenu(fileName = "ViewManagerMediatorGenerationOperation", menuName = "MVC/Admin/Code Generation Operations/ViewManagerMediatorGenerationOperation")]
    public class ViewManagerMediatorGenerationOperation 
        : CodeGenerationOperation<
            ViewManagerMediatorGenerationOperation,
            ViewManagerMediatorGenerationOperation.StartArgs,
            ViewManagerMediatorGenerationOperation.OperateArgs>
    {
        public struct StartArgs
        {
            public string Name;
            public CreateViMaType Type;
            public string[] ViewEventList;
        }

        public struct OperateArgs
        {
            
        }
        private string _name = "";
        private CreateViMaType _type;
        
        //Placeholder definitions.
        private const string NamespacePlaceholder = "%TemplateNS%";
        private const string TemplatePlaceholder = "%Template%";
        private const string ClassnamePlaceholder = "%ClassName%";
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
            _name = startArg.Name;
            _type = startArg.Type;
            
            //Ensuring existance of view and mediator path.
            DirectoryHelpers.CreateOrGetFolderPath(_sharedSettings.ProjectMediatorPath, _name);
        }

        protected override void OnOperate(OperateArgs startArg)
        {
            CreateManagerScript();
            CreateMediatorScript();
        }
        private void CreateManagerScript(string name = "")
        {

            if (string.IsNullOrEmpty(name))
                name = _name;
            //Creating View File
            Debug.Log("Creating View File");

            string data = string.Empty; 
            data = LoadTemplate(TemplateType.View);
            
            string scriptPathNamespace = (_sharedSettings.ProjectViewPath + "/" + name).Replace("/", ".") + ".";
            string namespaceValue = "Runtime.Views."+name;
            Debug.Log("Namespace = " + namespaceValue);
            
            //Set Namespaces for view
            data = data.Replace(NamespacePlaceholder, namespaceValue);
            
            //Set Class name for view
            data = data.Replace(TemplatePlaceholder, name + _type.ToString());
            
            ReplacePlaceholderWithTemplatedEventList(ref data, UnityActionPlaceholder, ViewUnityActionTemplate,
                _viewEventList,"\r\t\t");
            
            ReplacePlaceholderWithTemplatedEventList(ref data, FunctionPlaceholder, ViewFunctionTemplate,
                _viewEventList,"\r\t\t");
            
            Debug.Log("Created Data \n" + data);
            CodeUtilities.SaveFileCheckAndReplaceName(data, _sharedSettings.ProjectViewPath + "/" + _name + "/" + name + _type +".cs");
        }
        
        private void CreateMediatorScript(string name = "")
        {
            //Creating Mediator File
            Debug.Log("Creating Mediator File");
            
            if (string.IsNullOrEmpty(name))
                name = _name;
            
            var data = LoadTemplate(TemplateType.Mediator);
            string scriptPathNamespace = (_sharedSettings.ProjectViewPath + "/" + name).Replace("/", ".") + ".";
            string namespaceValue = "Runtime.Views."+name;
            
            data = data.Replace(NamespacePlaceholder, namespaceValue);
            
            data = data.Replace(TemplatePlaceholder, name + _type);
            
            data = data.Replace(ClassnamePlaceholder, name);
            
            //View listener registering
            ReplacePlaceholderWithTemplatedEventList(ref data,AddListenerPlaceholder,AddListenerTemplate,_viewEventList,"\r\t\t\t");
            
            //View listener unregistering
            ReplacePlaceholderWithTemplatedEventList(ref data,RemoveListenerPlaceholder,RemoveListenerTemplate,_viewEventList,"\r\t\t\t");
            
            //Mediator functions that are subscribed to view events.
            ReplacePlaceholderWithTemplatedEventList(ref data,FunctionPlaceholder,MediatorFunctionTemplate,_viewEventList,"\r\t\t");

            CodeUtilities.SaveFileCheckAndReplaceName(data, _sharedSettings.ProjectMediatorPath + "/" + _name + "/" + name  + "Mediator.cs");
        }
        
        private void ReplacePlaceholderWithTemplatedEventList(ref string data, string placeholder, string template, string[] eventList, string moreEventLetters)
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
                        functions += moreEventLetters;
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