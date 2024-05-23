using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_WEBGL
using YG;
#endif

namespace FixItGame
{
    public class LevelsCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _rewAddButton;
        [SerializeField] private GameObject _failedLevelPanel;
        [SerializeField] private GameObject _levelCompletePanel;

        private void OnEnable()
        {
            GlobalEvents.OnTimerEnded += ActivateFailedPanel;


            AdsManager.OnRewardConfirmed += DeactivateFailedPanel;

            GlobalEvents.OnLevelComplete += ActivateLevelCompletePanel;
        }

        private void OnDisable()
        {
            GlobalEvents.OnTimerEnded -= ActivateFailedPanel;

            AdsManager.OnRewardConfirmed -= DeactivateFailedPanel;

            GlobalEvents.OnLevelComplete -= ActivateLevelCompletePanel;
        }

        private void ActivateFailedPanel()
        {
            _failedLevelPanel.SetActive(true);

            if (AdsManager.Instance.IsRewardAddReady())
            {
                _rewAddButton.SetActive(true);
            }
            else
            {
                _rewAddButton.SetActive(false);
            }
        }

        private void DeactivateFailedPanel()
        {
            _failedLevelPanel.SetActive(false);
        }

        private void ActivateLevelCompletePanel()
        {
            _levelCompletePanel.SetActive(true);
        }

        private void DeactivateLevelCompletePanel()
        {
            _levelCompletePanel.SetActive(false);
        }

        public void LoadNextLevel()
        {
            if (AdsManager.Instance != null)
            {
                AdsManager.Instance.ShowInterstitial(GameManager.Instance.CurrentLevel, GameManager.Instance.LoadNextLevel);
            }
            else
            {
                GameManager.Instance.LoadNextLevel();
            }
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void ShowRewardAd()
        {
#if UNITY_WEBGL
            YandexGame.RewVideoShow(1);
            _rewAddButton.SetActive(false);
#else
            if (AdsManager.Instance != null && AdsManager.Instance.IsRewardAddReady())
            {
                AdsManager.Instance.ShowRewardAd();
            }
#endif
            _rewAddButton.SetActive(false);
        }

        public void ShowSettingsPanel(bool value)
        {
            GlobalEvents.RaiseLevelPaused(value);
            _settingsPanel.SetActive(value);
        }
    }
}
