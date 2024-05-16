using UnityEngine;

namespace FixItGame
{
    [CreateAssetMenu(fileName = "Text Localization", menuName = "Localization/Text")]
    public class TextLocalization: ScriptableObject
    {
        [SerializeField]
        public string[] localizations = new string[System.Enum.GetNames(typeof(Language)).Length];
        public string GetLocalization(Language language)
        {
            int index = (int)language;
            if (index < 0 || index >= localizations.Length)
            {
                Debug.LogError("Language index is out of range!");
                return string.Empty;
            }
            return localizations[index];
        }
    }
}
