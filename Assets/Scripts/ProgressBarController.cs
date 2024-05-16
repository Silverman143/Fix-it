using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FixItGame
{
    public class ProgressBarController : MonoBehaviour
    {
        [SerializeField] private Image _splitterPrefab;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _progressPercentTMP;

        private LevelManager _levelManager;

        private RectTransform _rectTransform;
        private float _maxProgress;
        private float _currentProgress;
        private int _splitAmount = 3;

        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
            _rectTransform = GetComponent<RectTransform>();
            _currentProgress = 0;

            _levelManager = FindObjectOfType<LevelManager>();

            UpdateBar(0);
        }

        private void OnEnable()
        {
            _levelManager.OnLevelProgressChanged.AddListener(UpdateBar);
        }

        private void OnDisable()
        {
            _levelManager.OnLevelProgressChanged.RemoveListener(UpdateBar);
        }

        private void Init(float maxProgress, float currentProgress, int splitAmount)
        {
            _maxProgress = maxProgress;
            _currentProgress = currentProgress;
            _splitAmount = splitAmount;
        }

        private void UpdateBar(float progress)
        {
            _currentProgress = progress;
            StartCoroutine(UpdateProgressCoroutine(_currentProgress));

        }

        private IEnumerator UpdateProgressCoroutine(float targetProgress)
        {
            float startProgress = _slider.value;
            float elapsedTime = 0f;
            float duration = 0.5f; // Длительность анимации

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                _slider.value = Mathf.Lerp(startProgress, targetProgress, elapsedTime / duration);
                _progressPercentTMP.text = string.Format("{0}%", Mathf.RoundToInt(100 * _slider.value));
                yield return null;
            }

            _slider.value = targetProgress;
            _progressPercentTMP.text = string.Format("{0}%", Mathf.RoundToInt(100 * _slider.value));
        }

        //private void CreateSplits()
        //{
        //    float width = _slider.sizeDelta.x;

        //    float step = width / _splitAmount;

        //    for (int i = 1; i <= _splitAmount-1; i++)
        //    {

        //        Image newSplit = Instantiate(_splitterPrefab, _slider);
        //        RectTransform newSplitRect = newSplit.GetComponent<RectTransform>();
        //        newSplitRect.anchoredPosition = new Vector2(step * i, 0);
        //    }

        //}
    }
}
