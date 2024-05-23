using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FixItGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private OpeningPuzzleData[] _newPuzzlesOpenData;

        private int _currentLevelProgress;
        private int _currentLevel = 0;
        public int CurrentLevel => _currentLevel;
        public static GameManager Instance;
        public float PuzzlesScale = 1;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            _currentLevel = DBController.GetLevel();
        }

        private void OnEnable()
        {
            GlobalEvents.OnPhantomFilled += AddProgress;
            GlobalEvents.OnPhantomEmpty += RemoveProgress;
            GlobalEvents.OnLevelComplete += CompleteLevel;
            GlobalEvents.OnGamePaused += StopGame;
            GlobalEvents.OnGameContinue += ContinueGame;
        }
        private void OnDisable()
        {
            GlobalEvents.OnPhantomFilled -= AddProgress;
            GlobalEvents.OnPhantomEmpty -= RemoveProgress;
            GlobalEvents.OnLevelComplete -= CompleteLevel;
            GlobalEvents.OnGamePaused -= StopGame;
            GlobalEvents.OnGameContinue -= ContinueGame;
        }

        private void StopGame()
        {
            Time.timeScale = 0f;
        }

        private void ContinueGame()
        {
            Time.timeScale = 1f;
        }
        private void CompleteLevel()
        {
            _currentLevel++;
            DBController.SaveLevel(_currentLevel);
        }

        private void AddProgress()
        {
            _currentLevelProgress++;
        }
        private void RemoveProgress()
        {
            _currentLevelProgress--;
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(1);
        }

        public void LevelUp()
        {
            _currentLevel++;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        public void LevelDown()
        {
            _currentLevel--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void SetPuzzlesScale(float value)
        {
            PuzzlesScale = value;
        }

        public (OpeningPuzzleData, int) GetNewPuzzleProgressData()
        {
            OpeningPuzzleData data = _newPuzzlesOpenData
                .Where(puzzleData => puzzleData.levelEnd >= _currentLevel-1)
                .OrderBy(puzzleData => puzzleData.levelStart)
                .FirstOrDefault();
            // Current level was upgrade by level complete
            Debug.Log(string.Format("current new puzzle: {0}", data.ToString()));
            return (data, _currentLevel-1);
        }
    }
}
