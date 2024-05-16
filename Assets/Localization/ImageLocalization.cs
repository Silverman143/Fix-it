using UnityEngine;

namespace FixItGame
{
    [CreateAssetMenu(fileName = "Image Localization", menuName = "Localization/Image")]
    public class ImageLocalization : ScriptableObject
    {
        [SerializeField]
        public Sprite[] localizations = new Sprite[System.Enum.GetNames(typeof(Language)).Length];
        public Sprite GetLocalization(Language language)
        {
            int index = (int)language;
            if (index < 0 || index >= localizations.Length)
            {
                Debug.LogError("Language index is out of range!");
                return null;
            }
            return localizations[index];
        }
    }
}
