using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PhantomController _phantomPrefab;
    [SerializeField] private PuzzleController _puzzlePrefab;
    [SerializeField] private Puzzle[] _puzzlesDatas;
    [SerializeField] private BoxCollider2D _spawnCollider;

    [SerializeField] private LevelData _levelData = new LevelData();

    private string _levelsFilesPath;
    private int _activeLevelNumber;
    [SerializeField] private List<PuzzleController> _levelPuzzles = new List<PuzzleController>();
    private List<PhantomController> _levelPhantoms = new List<PhantomController>();

    public void LevelUp() => GameManager.Instance.LevelUp();
    public void LevelDawn() => GameManager.Instance.LevelDown();

    private void Awake()
    {
        _levelsFilesPath = Path.Combine(Application.streamingAssetsPath, "Levels");
        _activeLevelNumber = GameManager.Instance.CurrentLevel;
        Debug.Log(string.Format("Active level: {0}", _activeLevelNumber));
        LoadLevel();
        CreatePhantoms();
        CreatePuzzles();
    }

    private void CreatePhantoms()
    {
        foreach (Puzzle puzzle in _puzzlesDatas)
        {
            List<PhantomData> boxes = _levelData.phantoms.FindAll(phantom => phantom.type == puzzle.type);
            foreach(PhantomData phantom in boxes)
            {
                PhantomController newPhantom = Instantiate(_phantomPrefab, this.transform);
                newPhantom.Init(sprite: puzzle.phantomSprite, type: puzzle.type, position: phantom.position);
                _levelPhantoms.Add(newPhantom);
            }
        }
    }
    private void CreatePuzzles()
    {
        foreach (Puzzle puzzle in _puzzlesDatas)
        {
            List<PhantomData> boxes = _levelData.phantoms.FindAll(phantom => phantom.type == puzzle.type);
            foreach (PhantomData phantom in boxes)
            {
                Vector2 newPos = GetRandomPointInBoxCollider(_spawnCollider);
                PuzzleController newPuzzle = Instantiate(_puzzlePrefab, this.transform);
                newPuzzle.Init(sprite: puzzle.phantomSprite, type: puzzle.type, position: newPos);
                _levelPuzzles.Add(newPuzzle);
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

        foreach (PuzzleController puzzle in _levelPuzzles)
        {
            levelData.phantoms.Add(new PhantomData
            {
                type = puzzle.Type,
                position = puzzle.transform.position,
                rotation = puzzle.transform.rotation
            });
        }

        string json = JsonUtility.ToJson(levelData, true);
        string levelFilePath = Path.Combine(_levelsFilesPath, string.Format("{0}.json", fileCount));

        File.WriteAllText(levelFilePath, json);
        Debug.Log("Level saved");
    }

    public void AddNewPuzzle()
    {
        PuzzleType type = PuzzleType.Box;
        Puzzle puzzle = _puzzlesDatas.FirstOrDefault(p => p.type == type);
        Vector2 newPos = GetRandomPointInBoxCollider(_spawnCollider);
        PuzzleController newPuzzle = Instantiate(_puzzlePrefab, this.transform);
        newPuzzle.Init(sprite: puzzle.puzzleSprite, type: type, position: newPos);
        _levelPuzzles.Add(newPuzzle);
    }

    /// Support math
    Vector2 GetRandomPointInBoxCollider(BoxCollider2D collider)
    {
        Vector2 boundsMin = collider.bounds.min;
        Vector2 boundsMax = collider.bounds.max;

        float randomX = Random.Range(boundsMin.x, boundsMax.x);
        float randomY = Random.Range(boundsMin.y, boundsMax.y);

        return new Vector2(randomX, randomY);
    }
}