using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FixItGame
{
    public class LocalizationManager : MonoBehaviour
    {
        private Language _currentLanguage;
        public Language Language => _currentLanguage;

        public static LocalizationManager Instance;
        public UnityEvent<Language> OnLanguageChanged;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            _currentLanguage = (Language)DBController.GetLanguage();
            SetLanguage(_currentLanguage);
        }

        public void SetLanguage(Language language)
        {
            _currentLanguage = language;
            OnLanguageChanged?.Invoke(_currentLanguage);
            DBController.SaveLanguage((int)language);
        }
    }
}
