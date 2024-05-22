using UnityEngine;
using TMPro;
using System;

namespace FixItGame
{
    [System.Serializable]
    public class DropDownLanguageData
    {
        public Sprite flagSprite;
        public string name;
        public Language lang;
    }

    public class DropDownHandler : MonoBehaviour
    {
        [SerializeField] private DropDownLanguageData[] _itemsData;
        private TMP_Dropdown _dropDown;

        private void Awake()
        {
            _dropDown = GetComponent<TMP_Dropdown>();
            _dropDown.onValueChanged.AddListener(delegate { ChangeValue(_dropDown.value); });
        }

        private void Start()
        {
            CreateItems();
        }

        private void CreateItems()
        {
            _dropDown.options.Clear();


            foreach (Language lang in Enum.GetValues(typeof(Language)))
            {
                string languageName = Enum.GetName(typeof(Language), lang);
                _dropDown.options.Add(new TMP_Dropdown.OptionData(languageName));
            }
            _dropDown.value = (int)LocalizationManager.Instance.Language;
            _dropDown.RefreshShownValue();
        }

        public void ChangeValue(int index)
        {
            Language selectedLanguage = (Language)index;
            LocalizationManager.Instance.SetLanguage(selectedLanguage);
        }
    }
}
