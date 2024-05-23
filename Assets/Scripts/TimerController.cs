using FixItGame;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private float _startTimeInSeconds;
    private TextMeshProUGUI _timerText; // Ссылка на TMP Text для отображения таймера
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _warningColor = Color.yellow;
    [SerializeField] private Color _dangerColor = Color.red;
    [SerializeField] private float _blinkSpeed = 2f; // Скорость моргания
    [SerializeField] private float _extraTime = 62.0f;

    private float _currentTime;
    private bool isActive = false;

    private void Awake()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
        Debug.Log("AWAKE !!!!!!");
    }

    private void OnEnable()
    {
        GlobalEvents.OnLevelStart += StartTimer;
        GlobalEvents.OnLevelPaused += SetPause;
        GlobalEvents.OnLevelComplete += StopTimer;

        AdsManager.OnRewardConfirmed += AddExtraTime;
    }

    private void OnDisable()
    {
        GlobalEvents.OnLevelStart -= StartTimer;
        GlobalEvents.OnLevelPaused -= SetPause;
        GlobalEvents.OnLevelComplete -= StopTimer;

        AdsManager.OnRewardConfirmed -= AddExtraTime;

    }

    private void Update()
    {
        if (!isActive)
            return;

        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
        {
            _currentTime = 0;
            isActive = false;
            GlobalEvents.RaiseTimerEnded();
        }

        UpdateTimerText();
        HandleBlinking();
    }

    public void Init(float value)
    {
        _currentTime = value;
    }

    private void AddExtraTime()
    {
        _currentTime += _extraTime;
        isActive = true;
    }

    private void StartTimer()
    {
        isActive = true;
    }

    private void StopTimer()
    {
        isActive = false;
    }

    private void SetPause(bool value)
    {
        isActive = !value;
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60);
        int seconds = Mathf.FloorToInt(_currentTime % 60);
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void HandleBlinking()
    {
        if (_currentTime < 60 && _currentTime >= 30)
        {
            float t = Mathf.PingPong(Time.time * _blinkSpeed, 1f);
            _timerText.color = Color.Lerp(_normalColor, _warningColor, t);
        }
        else if (_currentTime < 30)
        {
            float t = Mathf.PingPong(Time.time * _blinkSpeed, 1f);
            _timerText.color = Color.Lerp(_normalColor, _dangerColor, t);
        }
        else
        {
            _timerText.color = _normalColor;
        }
    }
}
