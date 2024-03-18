using UnityEngine;
using UnityEditor;

// ReSharper disable once CheckNamespace
namespace RT
{
    public static class ToolsMenu
    {
        [MenuItem("Tools/Razor's Tools/Prefixer")]
        public static void Prefixer()
        {
            EditorWindow.GetWindow(typeof(PrefixerWindow));
        }
    }
}
