using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private Image _splitterPrefab;
    [SerializeField] private RectTransform _slider;
    [SerializeField] private TextMeshProUGUI _levelTMP;
    private RectTransform _rectTransform;
    private float _maxProgress;
    private float _currentProgress;
    private int _splitAmount = 3;

    private void Awake()
    {
        _levelTMP.text = string.Format("Level {0}", GameManager.Instance.CurrentLevel);
        _rectTransform = GetComponent<RectTransform>();
        CreateSplits();
    }

    private void Init(float maxProgress, float currentProgress, int splitAmount)
    {
        _maxProgress = maxProgress;
        _currentProgress = currentProgress;
        _splitAmount = splitAmount;
        CreateSplits();
    }

    private void CreateSplits()
    {
        float width = _slider.sizeDelta.x;

        float step = width / _splitAmount;
        
        for (int i = 1; i <= _splitAmount-1; i++)
        {

            Image newSplit = Instantiate(_splitterPrefab, _slider);
            RectTransform newSplitRect = newSplit.GetComponent<RectTransform>();
            newSplitRect.anchoredPosition = new Vector2(step * i, 0);
        }

    }
}
