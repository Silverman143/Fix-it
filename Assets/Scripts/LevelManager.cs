using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using YG;

namespace FixItGame
{
    public class LevelManager : MonoBehaviour
    {
        //Prefabs
        [SerializeField] private PhantomController _phantomPrefab;
        [SerializeField] private PuzzleController _puzzlePrefab;
        [SerializeField] private Puzzle[] _puzzlesDatas;
        [SerializeField] private GameObject _fingerTutorial;

        //UI pannels
        [SerializeField] private GameObject _completeLevelPanel;
        [SerializeField] private GameObject _failedLevelPanel;

        [SerializeField] private float _completeLevelTimer = 0.5f;
        [SerializeField] private float _levelTimer = 180.0f;

        //Objects
        private TimerController _timerController;

        private PuzzlesSpawner _spawner;

        [SerializeField] private LevelData _levelData = new LevelData();

        private string _levelsFilesPath;
        private int _activeLevelNumber;
        [SerializeField] private List<PuzzleController> _levelPuzzles = new List<PuzzleController>();
        private List<PhantomController> _levelPhantoms = new List<PhantomController>();

        public void LevelUp() => GameManager.Instance.LevelUp();
        public void LevelDawn() => GameManager.Instance.LevelDown();

        private void AddLevelPuzzle(PuzzleController puzzle) => _levelPuzzles.Add(puzzle);

        //Events
        public UnityEvent<float> OnLevelProgressChanged;
        public UnityEvent OnLevelComplete;

        //Level params
        private int _levelProgress = 0;
        private float _progressPrecrnt = 0;
        private bool _isComplete = false;


        private void Awake()
        {
            _activeLevelNumber = GameManager.Instance.CurrentLevel;
            _spawner = FindObjectOfType<PuzzlesSpawner>();
            _timerController = FindObjectOfType<TimerController>();
            Debug.Log(string.Format("Active level: {0}", _activeLevelNumber));
            LoadLevel();
            CreatePhantoms();
            _spawner.Init(_levelData.puzzles, _levelData.puzzlesScale);
            //CreatePuzzles();
        }

        private void OnEnable()
        {
            GlobalEvents.OnCreateNewPuzzle += AddLevelPuzzle;
            GlobalEvents.OnPhantomFilled += AddLevelProgress;
            GlobalEvents.OnPhantomEmpty += RemoveLevelProgress;
            GlobalEvents.OnTimerEnded += ActivateFailedPanel;

            //Rew add
            YandexGame.OpenVideoEvent += DeactivateFailedPanel;
            YandexGame.ErrorVideoEvent += ActivateFailedPanel;
        }

        private void OnDisable()
        {
            GlobalEvents.OnCreateNewPuzzle -= AddLevelPuzzle;
            GlobalEvents.OnPhantomFilled -= AddLevelProgress;
            GlobalEvents.OnPhantomEmpty -= RemoveLevelProgress;
            GlobalEvents.OnTimerEnded -= ActivateFailedPanel;

            //Rew add
            YandexGame.OpenVideoEvent -= DeactivateFailedPanel;
            YandexGame.ErrorVideoEvent -= ActivateFailedPanel;
        }

        private void ActivateFailedPanel() => _failedLevelPanel.SetActive(true);
        private void DeactivateFailedPanel() => _failedLevelPanel.SetActive(false);
        private void ActivateCompletePanel() => _completeLevelPanel.SetActive(true);


        private void ActivateCompletePanel(bool value)
        {
            _completeLevelPanel.SetActive(value);
        }

        private void Start()
        {
            _timerController.Init(value: _levelTimer);
            GlobalEvents.RaiseLevelStart();

            if (_activeLevelNumber == 0)
            {
                Instantiate(_fingerTutorial);
            }
        }

        private void CreatePhantoms()
        {
            foreach (Puzzle puzzle in _puzzlesDatas)
            {
                List<PhantomData> boxes = _levelData.phantoms.FindAll(phantom => phantom.type == puzzle.type);
                foreach (PhantomData phantom in boxes)
                {
                    PhantomController newPhantom = Instantiate(_phantomPrefab, this.transform);
                    newPhantom.Init(sprite: puzzle.phantomSprite, type: puzzle.type, position: phantom.position, rotation: phantom.rotation, scale: phantom.scale);
                    _levelPhantoms.Add(newPhantom);
                }
            }
        }

        private void LoadLevel()
        {
            string levelFilePath = string.Format("Levels/{0}", _activeLevelNumber);

            TextAsset jsonFile = Resources.Load<TextAsset>(levelFilePath);

            if (jsonFile != null)
            {
                _levelData = JsonUtility.FromJson<LevelData>(jsonFile.text);
                Debug.Log("Level loaded");
            }
            else
            {
                Debug.LogError(string.Format("Level data is not exist: level {0}", _activeLevelNumber));
            }

        }

        public void SaveNewLevel()
        {
            // Получаем все файлы в папке
            string targetPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels");
            string[] files = Directory.GetFiles(targetPath, "*.json"); // Фильтр для поиска только JSON файлов
            int fileCount = files.Length; // Количество файлов минус один

            LevelData levelData = new LevelData();
            levelData.puzzlesScale = _levelPuzzles[0].transform.localScale;
            foreach (PuzzleController puzzle in _levelPuzzles)
            {
                PuzzlePart[] parts = puzzle.GetComponentsInChildren<PuzzlePart>();

                if (parts.Length > 0)
                {
                    foreach (PuzzlePart part in parts)
                    {
                        levelData.phantoms.Add(new PhantomData
                        {
                            type = PuzzleType.Box,
                            position = part.transform.position,
                            rotation = part.transform.rotation,
                            scale = part.transform.parent.localScale
                        });
                    }

                }
                else
                {
                    levelData.phantoms.Add(new PhantomData
                    {
                        type = PuzzleType.Box,
                        position = puzzle.transform.position,
                        rotation = puzzle.transform.rotation,
                        scale = puzzle.transform.localScale
                    });
                }

                levelData.puzzles.Add(puzzle.Type);
            }

            string json = JsonUtility.ToJson(levelData, true);
            string levelFilePath = Path.Combine(targetPath, string.Format("{0}.json", fileCount));

            File.WriteAllText(levelFilePath, json);
            Debug.Log("Level saved");
        }

        //IEnumerator LoadStreamingAssetWebGL(string filePath)
        //{

        //    string result;
        //    if (filePath.Contains("://") || filePath.Contains(":///"))
        //    {
        //        WWW www = new WWW(filePath);
        //        yield return www;
        //        result = www.text;
        //    }
        //    else
        //        result = System.IO.File.ReadAllText(filePath);

        //}

        private void AddLevelProgress()
        {
            _levelProgress++;
            UploadLevelProgress();
        }
        private void RemoveLevelProgress()
        {
            _levelProgress--;
            UploadLevelProgress();
        }

        private void UploadLevelProgress()
        {
            _progressPrecrnt = (float)_levelProgress / (float)_levelPhantoms.Count;
            OnLevelProgressChanged?.Invoke(_progressPrecrnt);
            if (_progressPrecrnt == 1)
            {
                StartCoroutine(TimerTillCompleteLevelCoroutine());
            }
        }

        IEnumerator TimerTillCompleteLevelCoroutine()
        {
            yield return new WaitForSeconds(_completeLevelTimer);

            if (_progressPrecrnt == 1 && !_isComplete)
            {
                _isComplete = true;

                OnLevelComplete.Invoke();
                GlobalEvents.RaiseLevelComplete();
                _completeLevelPanel.SetActive(true);
            }
        }

    }
}