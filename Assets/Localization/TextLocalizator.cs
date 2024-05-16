using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

using System;
using System.Linq;

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

                // Find all numbers in the current text and save their positions
                var numberMatches = Regex.Matches(currentText, @"\d+");
                var numbers = new System.Collections.Generic.List<string>();
                foreach (Match match in numberMatches)
                {
                    numbers.Add(match.Value);
                }

                // Replace letters in current text with localized text while preserving spaces, punctuation, and numbers
                int localizedIndex = 0;
                string updatedText = Regex.Replace(currentText, @"[^\d]", m => {
                    if (char.IsLetter(m.Value[0]) && localizedIndex < localizedText.Length)
                    {
                        return localizedText[localizedIndex++].ToString();
                    }
                    return m.Value;
                });

                // Replace numbers back in the text
                int numberIndex = 0;
                updatedText = Regex.Replace(updatedText, @"\d+", m => {
                    return numberIndex < numbers.Count ? numbers[numberIndex++] : m.Value;
                });

                _tmp.text = updatedText;
            }
        }
    }
}
