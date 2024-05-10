using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _currentLevelProgress;
    private int _currentLevelTarget;
    private int _targetLevel = 0;
    public int CurrentLevel => _targetLevel;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        _targetLevel = PlayerPrefs.GetInt("LevelsComplete");
    }

    private void OnEnable()
    {
        GlobalEvents.OnPhantomFilled += AddProgress;
        GlobalEvents.OnPhantomEmpty += RemoveProgress;
    }
    private void OnDisable()
    {
        GlobalEvents.OnPhantomFilled -= AddProgress;
        GlobalEvents.OnPhantomEmpty -= RemoveProgress;
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

    public void LevelUp()
    {
        _targetLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void LevelDown()
    {
        _targetLevel--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
