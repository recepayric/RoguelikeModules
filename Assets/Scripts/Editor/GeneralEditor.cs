using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GeneralEditor : OdinEditorWindow
    {
        [MenuItem("Idle/GeneralEditor")]
        private static void OpenWindow()
        {
            GetWindow<GeneralEditor>().Show();
        }

        [EnumToggleButtons, BoxGroup("Settings")]
        public ScaleMode ScaleMode;

        [FolderPath(RequireExistingPath = true), BoxGroup("Settings")]
        public string OutputPath;

        [HorizontalGroup(0.5f)] [CanBeNull] public List<Texture> InputTextures;

        [HorizontalGroup, InlineEditor(InlineEditorModes.LargePreview)]
        public Texture Preview;

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void PerformSomeAction()
        {
        }
    }
}