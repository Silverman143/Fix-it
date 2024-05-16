using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FixItGame
{
    public class SettingsHandlerGUI : MonoBehaviour
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        private void Awake()
        {
            _soundSlider.value = SettingsManager.Instance.SoundVolume;
            _musicSlider.value = SettingsManager.Instance.MusicVolume;
        }

        private void OnEnable()
        {
            _soundSlider.onValueChanged.AddListener(value => GlobalEvents.RaiseSoundValueChanged(value));
            _musicSlider.onValueChanged.AddListener(value => GlobalEvents.RaiseMusicValueChanged(value));
        }
        private void OnDisable()
        {
            _soundSlider.onValueChanged.RemoveListener(value => GlobalEvents.RaiseSoundValueChanged(value));
            _musicSlider.onValueChanged.RemoveListener(value => GlobalEvents.RaiseMusicValueChanged(value));
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadSliders()
        {
            _musicSlider.value = SettingsManager.Instance.MusicVolume;
            _soundSlider.value = SettingsManager.Instance.SoundVolume;
        }


    }
}
