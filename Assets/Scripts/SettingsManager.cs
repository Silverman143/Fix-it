using System.Collections;
using System.Collections.Generic;
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

        public static SettingsManager Instance { get; private set; }
        public bool isMobile = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadSettingsValues();
            }
            else
            {
                Destroy(gameObject);
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
            isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ||
                   (Application.platform == RuntimePlatform.WebGLPlayer && IsMobileBrowser());
        }

        private bool IsMobileBrowser()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        // Check for mobile device user agent using JavaScript interop
        return Application.ExternalEval(
            @"function isMobile() {
                if (navigator.userAgent.match(/Android/i) ||
                    navigator.userAgent.match(/webOS/i) ||
                    navigator.userAgent.match(/iPhone/i) ||
                    navigator.userAgent.match(/iPad/i) ||
                    navigator.userAgent.match(/iPod/i) ||
                    navigator.userAgent.match(/BlackBerry/i) ||
                    navigator.userAgent.match(/Windows Phone/i)) {
                    return true;
                }
                return false;
            }
            isMobile();");
#else
            return SystemInfo.deviceType == DeviceType.Handheld;
#endif
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
