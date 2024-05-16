using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        
        public static int GetLevel() => GetValue("level", 0);
        public static void SaveLevel(int value) => SetValue("level", value);

        public static float GetSoundValue() => GetValue("sound", 1f);
        public static void SaveSoundVolume(float value) => SetValue("sound", value);

        public static float GetMusicValue() => GetValue("music", 1f);
        public static void SaveMusicVolume(float value) => SetValue("music", value);

        public static int GetLanguage() => GetValue("language", 0);
        public static void SaveLanguage(int value) => SetValue("language", value);
    }
}
