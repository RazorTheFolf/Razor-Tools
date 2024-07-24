using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RT
{
    public class PrefixerWindow : EditorWindow
    {
        private readonly string[] prefixPosition = { "Start", "End" };

        private readonly string[] separators =
        {
            "Dash", "Dot", "Underscore"
        };

        private GameObject parent;
        private bool placeAtStart = true;
        private string prefix;
        private bool prefixParent = true;
        private int selectedSeparator = 1;


        private void OnGUI()
        {
            titleContent = new GUIContent("Prefixer");

            GUILayout.Space(15);
            GUILayout.Label("Prefixer", EditorStyles.boldLabel);

            parent = (GameObject)EditorGUILayout.ObjectField("Parent", parent, typeof(GameObject), true);
            prefix = EditorGUILayout.TextField("Prefix", prefix);
            selectedSeparator = EditorGUILayout.Popup("Separator", selectedSeparator, separators);
            placeAtStart = EditorGUILayout.Popup("Position", placeAtStart ? 0 : 1, prefixPosition) == 0;
            prefixParent = EditorGUILayout.Toggle("Prefix Parent?", prefixParent);
            if (GUILayout.Button("Prefix it!"))
            {
                if (!parent)
                {
                    Debug.LogError("Parent is null");
                    return;
                }

                if (string.IsNullOrEmpty(prefix))
                {
                    Debug.LogError("Prefix is empty");
                    return;
                }

                string separator = selectedSeparator switch
                {
                    0 => "-",
                    1 => ".",
                    _ => "_"
                };

                foreach (Transform child in parent.transform)
                {
                    child.name = placeAtStart
                        ? prefix + separator + child.name
                        : child.name + separator + prefix;

                    UpdatePrefixes(child, prefix, separator, placeAtStart);
                }

                if (prefixParent)
                    parent.name = placeAtStart
                        ? prefix + separator + parent.name
                        : parent.name + separator + prefix;
            }
        }

        private static void UpdatePrefixes(Transform parentTransform, string newPrefix, string separator, bool start)
        {
            foreach (Transform child in parentTransform)
            {
                child.name = start
                    ? newPrefix + separator + child.name
                    : child.name + separator + newPrefix;

                UpdatePrefixes(child, newPrefix, separator, start);
            }
        }
    }
}