using MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.Constants
{
    [CreateAssetMenu(fileName = "CodeGenerationSettings", menuName = "MVC/Admin/Code Generation Settings")]
    public class CodeGenerationSettings : ScriptableObject
    {
        [SerializeField] private CodeGenerationOperationConfig _screenCodeGenerationConfig;
        public CodeGenerationOperationConfig ScreenCodeGenerationConfig
        {
            get => _screenCodeGenerationConfig;
        }
        
        [SerializeField] private CodeGenerationOperationConfig _viewManagerCodeGenerationConfig;
        public CodeGenerationOperationConfig ViewManagerCodeGenerationConfig
        {
            get => _viewManagerCodeGenerationConfig;
        }
        
        [SerializeField] private CodeGenerationOperationConfig _rootAndContextCodeGenerationConfig;
        public CodeGenerationOperationConfig RootAndContextCodeGenerationConfig
        {
            get => _rootAndContextCodeGenerationConfig;
        }
        
        [SerializeField] private CodeGenerationOperationConfig _testRootAndContextCodeGenerationConfig;
        public CodeGenerationOperationConfig TestRootAndContextCodeGenerationConfig
        {
            get => _testRootAndContextCodeGenerationConfig;
        }
        
        
        [SerializeField][BoxGroup("MVC-Base Paths")]
        private DefaultAsset _testTemplateFolderInfo;
        public string TestTemplatePath
        {
            get => GetFolderInfoFolderPath(_testTemplateFolderInfo);
        }
        [SerializeField][BoxGroup("MVC-Base Paths")]
        private DefaultAsset _screenTemplateFolderInfo;
        public string TemplatePrefabPath
        {
            get => GetFolderInfoFolderPath(_screenTemplateFolderInfo);
        }
        [SerializeField][BoxGroup("MVC-Base Paths")]
        private DefaultAsset _codeTemplateFolderInfo;
        public string CodeTemplatePath
        {
            get => GetFolderInfoFolderPath(_codeTemplateFolderInfo);
        }
        
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectViewPath;
        public string ProjectViewPath
        {
            get => _projectViewPath;
        }
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectMediatorPath;
        public string ProjectMediatorPath
        {
            get => _projectMediatorPath;
        }
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectConstantsPath;
        public string ProjectConstantsPath
        {
            get => _projectConstantsPath;
        }
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectTestRootPath;
        public string ProjectTestRootPath
        {
            get => _projectTestRootPath;
        }
        
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectRootPath;
        public string ProjectRootPath
        {
            get => _projectRootPath;
        }
        
        [SerializeField][FolderPath][BoxGroup("Project Paths")]
        private string _projectContextPath;
        public string ProjectContextPath
        {
            get => _projectContextPath;
        }
        
        [SerializeField][BoxGroup("Project Paths")][FolderPath]
        private string _projectResourcesPath;
        public string ProjectResourcesPath
        {
            get => _projectResourcesPath;
        }
        private string GetFolderInfoFolderPath(UnityEngine.Object asset)
        {
            return AssetDatabase.GetAssetPath(asset).Replace("/" + asset.name + ".folderInfo","");
        }
        
        [SerializeField][BoxGroup("Project Paths")][FolderPath]
        private string _projectEnumsPath;
        public string ProjectEnumsPath
        {
            get => _projectEnumsPath;
        }
    }
}