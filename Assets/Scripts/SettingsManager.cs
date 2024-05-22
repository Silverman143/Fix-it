using Firebase.Extensions;
using UnityEngine;

namespace FixItGame
{

    public class SettingsManager : MonoBehaviour
    {
        private float _soundVolume;
        public float SoundVolume
        {
            get => _soundVolume;
            private set
            {
                _soundVolume = value;
                DBController.SaveSoundVolume(value);
            }
        }

        private float _musicVolume;
        public float MusicVolume
        {
            get => _musicVolume;
            private set
            {
                _musicVolume = value;
                _bgSound.volume = value;
                DBController.SaveMusicVolume(value);
            }
        }

        [SerializeField] private AudioSource _bgSound;
        [SerializeField] private bool _use60FPS = false;

        public static SettingsManager Instance { get; private set; }
        public bool isMobile = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadSettingsValues();
                CheckDevice();
            }
            else
            {
                Destroy(gameObject);
            }

            if (_use60FPS)
            {
                Application.targetFrameRate = 60;
            }
        }

        private void OnEnable()
        {
            GlobalEvents.OnMusicVolumeChanged += UpdateMusicVolume;
            GlobalEvents.OnSoundVolumeChanged += UpdateSoundVolume;
        }

        private void OnDisable()
        {
            GlobalEvents.OnMusicVolumeChanged -= UpdateMusicVolume;
            GlobalEvents.OnSoundVolumeChanged -= UpdateSoundVolume;
        }

        private void CheckDevice()
        {
            isMobile = Application.isMobilePlatform;
        }

        private void LoadSettingsValues()
        {
            SoundVolume = DBController.GetSoundValue();
            MusicVolume = DBController.GetMusicValue();
        }

        private void UpdateSoundVolume(float value)
        {
            SoundVolume = value;
        }

        private void UpdateMusicVolume(float value)
        {
            MusicVolume = value;
        }
    }
}
