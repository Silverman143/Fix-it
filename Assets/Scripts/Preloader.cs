using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace FixItGame
{
    public class Preloader : MonoBehaviour
    {
        private string levelsDirectory = "Levels";
        private string localPath => Path.Combine(Application.persistentDataPath, levelsDirectory);

        private void Awake()
        {
            StartCoroutine(DownloadLevelFiles());
        }

        private IEnumerator DownloadLevelFiles()
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, levelsDirectory);
            if (fullPath.Contains("://") || fullPath.Contains(":///"))
            {
                UnityWebRequest www = UnityWebRequest.Get(fullPath);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError("Error downloading levels: " + www.error);
                }
                else
                {
                    SaveFilesLocally(www.downloadHandler.data);
                }
            }
            else
            {
                CopyFilesDirectly(fullPath);
            }
        }

        private void SaveFilesLocally(byte[] data)
        {
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            // Extract data and save locally (implement the extraction logic depending on data format)
            // This is a placeholder for the real implementation
            File.WriteAllBytes(Path.Combine(localPath, "levelsData"), data);
        }

        private void CopyFilesDirectly(string sourcePath)
        {
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            if (!Directory.Exists(sourcePath))
            {
                Debug.LogError("Source directory does not exist: " + sourcePath);
                return;
            }

            foreach (var file in Directory.GetFiles(sourcePath))
            {
                var destFile = Path.Combine(localPath, Path.GetFileName(file));
                if (!File.Exists(destFile) || File.GetLastWriteTimeUtc(sourcePath) > File.GetLastWriteTimeUtc(destFile))
                {
                    File.Copy(file, destFile, true);
                }
            }
        }

        public void DeleteLevelData()
        {
            if (Directory.Exists(localPath))
            {
                Directory.Delete(localPath, true);
                Debug.Log("All level data deleted.");
            }
        }

        public void UpdateLevelData()
        {
            DeleteLevelData();
            StartCoroutine(DownloadLevelFiles());
        }
    }
}
