using UnityEngine;
using UnityEditor;

// ReSharper disable once CheckNamespace
namespace RT
{
    public class PrefixerWindow : EditorWindow
    {
        private GameObject parent;
        private string prefix;
        private int selectedSeparator;
        private bool placeAtStart;

        private readonly string[] separators =
        {
            "Dash", "Dot", "Underscore",
        };

        private readonly string[] prefixPosition = { "Start", "End" };

        private void UpdatePrefixes(Transform parentTransform, string newPrefix, string separator, bool start)
        {
            foreach (Transform child in parentTransform)
            {
                if (start)
                {
                    child.name = newPrefix + separator + child.name;
                }
                else
                {
                    child.name = child.name + separator + newPrefix;
                }

                UpdatePrefixes(child, newPrefix, separator, start); // Recursively update prefixes for nested children
            }
        }


        private void OnGUI()
        {
            titleContent = new GUIContent(" Prefixer");

            GUILayout.Space(15);
            GUILayout.Label("Prefixer", EditorStyles.boldLabel);

            parent = (GameObject)EditorGUILayout.ObjectField("Parent", parent, typeof(GameObject), true);
            prefix = EditorGUILayout.TextField("Prefix", prefix);
            selectedSeparator = EditorGUILayout.Popup("Separator", selectedSeparator, separators);
            placeAtStart = EditorGUILayout.Popup("Position", placeAtStart ? 0 : 1, prefixPosition) == 0;
            if (GUILayout.Button("Prefix it!"))
            {
                if (parent == null)
                {
                    Debug.LogError("Parent is null");
                    return;
                }

                if (string.IsNullOrEmpty(prefix))
                {
                    Debug.LogError("Prefix is empty");
                    return;
                }

                foreach (Transform child in parent.transform)
                {
                    string separator = selectedSeparator switch
                    {
                        1 => ".",
                        2 => "_",
                        _ => "-"
                    };

                    if (placeAtStart)
                    {
                        child.name = prefix + separator + child.name;
                    }
                    else
                    {
                        child.name = child.name + separator + prefix;
                    }

                    UpdatePrefixes(child, prefix, separator, placeAtStart);
                }
            }
        }
    }
}