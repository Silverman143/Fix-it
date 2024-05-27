using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

using System;
using System.Linq;
using System.Collections.Generic;

namespace FixItGame
{

    public class TextLocalizator : MonoBehaviour
    {
        [SerializeField] private TextLocalization _data;
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private bool _withDig = false;

        private Language _language = Language.English;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _language = LocalizationManager.Instance.Language;
            UpdateLanguage(_language);
            LocalizationManager.Instance?.OnLanguageChanged?.AddListener(UpdateLanguage);
        }

        private void OnEnable()
        {
            LocalizationManager.Instance?.OnLanguageChanged?.AddListener(UpdateLanguage);
        }

        private void OnDisable()
        {
            LocalizationManager.Instance.OnLanguageChanged?.RemoveListener(UpdateLanguage);
        }

        //public bool ContainsDigitRegex(string input)
        //{
        //    _withDig = input.Any(char.IsNumber);
        //    return _withDig;
        //}

        private void UpdateLanguage(Language language)
        {
            _language = language;

            if (!_withDig)
            {
                _tmp.text = _data.GetLocalization(_language);
            }
            else
            {
                string currentText = _tmp.text;
                string localizedText = _data.GetLocalization(_language);

                // Найдите все числа в исходном тексте
                var numberMatches = Regex.Matches(currentText, @"\d+");

                // Создайте список для хранения найденных чисел
                var numbers = new List<string>();

                foreach (Match match in numberMatches)
                {
                    numbers.Add(match.Value);
                }

                // Преобразуем список в массив строк для использования в string.Format
                string[] numberArray = numbers.ToArray();

                // Используем string.Format для замены placeholders на числа
                string result = string.Format(localizedText, numberArray);

                _tmp.text = result;
            }
        }
    }
}
