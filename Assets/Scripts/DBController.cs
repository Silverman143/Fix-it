using UnityEngine;
#if UNITY_WEBGL
using YG;
using System;
#endif

namespace FixItGame
{
    public static class DBController
    {
        private static T GetValue<T>(string key, T defaultValue)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)PlayerPrefs.GetFloat(key, (float)(object)defaultValue);
            }
            throw new System.NotImplementedException($"Not implemented type for PlayerPrefs: {typeof(T)}");
        }

        private static void SetValue<T>(string key, T value)
        {
            if (typeof(T) == typeof(int))
            {
                PlayerPrefs.SetInt(key, (int)(object)value);
            }
            else if (typeof(T) == typeof(float))
            {
                PlayerPrefs.SetFloat(key, (float)(object)value);
            }
            else
            {
                throw new System.NotImplementedException($"Not implemented type for PlayerPrefs: {typeof(T)}");
            }
            PlayerPrefs.Save();
        }


        public static int GetLevel()
        {
#if UNITY_WEBGL
            if (YandexGame.Instance != null)
            {
                return YandexGame.savesData.currentLevel;
            }
            else
            {
                return GetValue("level", 0);
            }
#else
            return GetValue("level", 0);
#endif
        }

        public static void SaveLevel(int value)
        {
#if UNITY_WEBGL
         if (YandexGame.Instance != null)
            {
                YandexGame.savesData.currentLevel = value;
                YandexGame.SaveProgress();
            }
            else
            {
                SetValue("level", value);
            }
#else
            SetValue("level", value);
#endif
        }

        public static float GetSoundValue() => GetValue("sound", 1f);
        public static void SaveSoundVolume(float value) => SetValue("sound", value);

        public static float GetMusicValue() => GetValue("music", 1f);
        public static void SaveMusicVolume(float value) => SetValue("music", value);

        public static int GetLanguage()
        {
#if UNITY_WEBGL
            if (YandexGame.Instance != null)
            {
                string lang = YandexGame.savesData.language;
                if (Enum.TryParse(lang, out Language langEnum))
                {
                    return (int)langEnum;
                }
                else
                {
                    Console.WriteLine("Invalid language string.");
                    return 0;
                }
            }
            else
            {
                return GetValue("language", 0);
            }
#else
            return GetValue("language", 0);
#endif
        }

        public static void SaveLanguage(int value)
        {
#if UNITY_WEBGL
            if (YandexGame.Instance != null)
            {
                Language lang = (Language)value;
                YandexGame.savesData.language = lang.ToString();
                YandexGame.SaveProgress();
            }
            else
            {
                SetValue("language", value);
            }
#else
            SetValue("language", value);
#endif
        }

        public static void CleanData()
        {
#if UNITY_WEBGL
            if (YandexGame.Instance != null)
            {
                YandexGame.savesData.currentLevel = 0;
                YandexGame.SaveProgress();
            }
            else
            {
                SetValue("level", 0);
            }
#else
            SetValue("level", 0);
#endif
        }

    }
}
