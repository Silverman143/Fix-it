using UnityEngine;
using UnityEditor;

namespace FixItGame
{
    [CustomEditor(typeof(TextLocalization))]
    public class TextLocalizationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Get link to ScriptableObject
            TextLocalization localization = (TextLocalization)target;

            // Nameing each field 
            string[] names = System.Enum.GetNames(typeof(Language));

            // Check size of localizations
            if (localization.localizations == null || localization.localizations.Length != names.Length)
            {
                localization.localizations = new string[names.Length];
            }

            // Createing input fields for each language
            for (int i = 0; i < names.Length; i++)
            {
                localization.localizations[i] = EditorGUILayout.TextField(names[i], localization.localizations[i]);
            }

            // Save naming
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

    [CustomEditor(typeof(ImageLocalization))]
    public class ImageLocalizationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Get link to ScriptableObject
            ImageLocalization localization = (ImageLocalization)target;

            // Naming each field
            string[] names = System.Enum.GetNames(typeof(Language));

            // Check size of localizations
            if (localization.localizations == null || localization.localizations.Length != names.Length)
            {
                localization.localizations = new Sprite[names.Length];
            }

            // Creating input fields for each language
            for (int i = 0; i < names.Length; i++)
            {
                localization.localizations[i] = (Sprite)EditorGUILayout.ObjectField(names[i], localization.localizations[i], typeof(Sprite), false);
            }

            // Save changes
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}