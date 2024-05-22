using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace FixItGame
{
    public class LevelsCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsPanel;

       public void LoadNextLevel()
        {
            GameManager.Instance.LoadNextLevel();
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void ShowRewardAd()
        {
            YandexGame.RewVideoShow(1);
        }

        public void ShowSettingsPanel(bool value)
        {
            GlobalEvents.RaiseLevelPaused(value);
            _settingsPanel.SetActive(value);
        }
    }
}
