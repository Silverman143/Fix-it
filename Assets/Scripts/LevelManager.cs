using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace FixItGame
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private PhantomController _phantomPrefab;
        [SerializeField] private PuzzleController _puzzlePrefab;
        [SerializeField] private Puzzle[] _puzzlesDatas;
        [SerializeField] private GameObject _completeLevelPanel;
        [SerializeField] private float _completeLevelTimer = 0.5f;

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
            _levelsFilesPath = Path.Combine(Application.persistentDataPath, "Levels");
            _activeLevelNumber = GameManager.Instance.CurrentLevel;
            _spawner = FindObjectOfType<PuzzlesSpawner>();
            Debug.Log(string.Format("Active level: {0}", _activeLevelNumber));
            LoadLevel();
            CreatePhantoms();
            _spawner.Init(_levelData.puzzles);
            //CreatePuzzles();
        }

        private void OnEnable()
        {
            GlobalEvents.OnCreateNewPuzzle += AddLevelPuzzle;
            GlobalEvents.OnPhantomFilled += AddLevelProgress;
            GlobalEvents.OnPhantomEmpty += RemoveLevelProgress;
        }

        private void OnDisable()
        {
            GlobalEvents.OnCreateNewPuzzle -= AddLevelPuzzle;
            GlobalEvents.OnPhantomFilled -= AddLevelProgress;
            GlobalEvents.OnPhantomEmpty += RemoveLevelProgress;
        }

        private void CreatePhantoms()
        {
            foreach (Puzzle puzzle in _puzzlesDatas)
            {
                List<PhantomData> boxes = _levelData.phantoms.FindAll(phantom => phantom.type == puzzle.type);
                foreach (PhantomData phantom in boxes)
                {
                    PhantomController newPhantom = Instantiate(_phantomPrefab, this.transform);
                    newPhantom.Init(sprite: puzzle.phantomSprite, type: puzzle.type, position: phantom.position);
                    _levelPhantoms.Add(newPhantom);
                }
            }
        }

        private void DeleteLevel()
        {

        }
        private void LoadLevel()
        {
            string levelFilePath = Path.Combine(_levelsFilesPath, string.Format("{0}.json", _activeLevelNumber));
            if (File.Exists(levelFilePath))
            {
                string json = File.ReadAllText(levelFilePath);
                _levelData = JsonUtility.FromJson<LevelData>(json);
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
            string[] files = Directory.GetFiles(_levelsFilesPath, "*.json"); // Фильтр для поиска только JSON файлов
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
                            scale = part.transform.localScale
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
            string levelFilePath = Path.Combine(_levelsFilesPath, string.Format("{0}.json", fileCount));

            File.WriteAllText(levelFilePath, json);
            Debug.Log("Level saved");
        }

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