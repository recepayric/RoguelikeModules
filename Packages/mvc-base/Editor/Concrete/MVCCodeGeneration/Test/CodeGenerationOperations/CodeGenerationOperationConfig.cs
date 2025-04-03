using System.Collections.Generic;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Test.CodeGenerationOperations
{
    [CreateAssetMenu(fileName = "CodeGenerationOperationConfig", menuName = "MVC/Admin/Code Generation Operation Config")]
    public class CodeGenerationOperationConfig : ScriptableObject
    {
        [SerializeField] private List<CodeGenerationOperation> _operations;
        public CodeGenerationOperation[] Operations
        {
            get => _operations.ToArray();
        }
    }
}