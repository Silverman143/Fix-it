using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

namespace FixItGame
{
    public class LevelNumberText : MonoBehaviour
    {
        private TextMeshProUGUI _tmp;

        private void Start()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            UpdateTMP();
        }

        private void UpdateTMP()
        {
            string currentText = _tmp.text;

            string updatedText = Regex.Replace(currentText, @"\d+", GameManager.Instance.CurrentLevel.ToString());

            //Debug.Log(string.Format("replace `{0}` to `{1}`", currentText, updatedText));

            _tmp.text = updatedText;
        }
    }
}
