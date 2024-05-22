using UnityEngine;
using UnityEngine.UI;

namespace FixItGame
{
    public class ImageLocalizator : MonoBehaviour
    {
        [SerializeField] private ImageLocalization _data;
        [SerializeField] private Image _image;
        private Language _language = Language.English;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _language = LocalizationManager.Instance.Language;
            _image.sprite = _data.GetLocalization(_language);
            _image.SetNativeSize();

            LocalizationManager.Instance.OnLanguageChanged?.AddListener(UpdateLanguage);
        }

        private void OnEnable()
        {
            LocalizationManager.Instance?.OnLanguageChanged?.AddListener(UpdateLanguage);
        }

        private void OnDisable()
        {
            LocalizationManager.Instance.OnLanguageChanged?.RemoveListener(UpdateLanguage);
        }

        private void UpdateLanguage(Language language)
        {
            _language = language;

            _image.sprite = _data.GetLocalization(_language);
            _image.SetNativeSize();
        }
    }
}
