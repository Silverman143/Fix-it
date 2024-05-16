using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FixItGame
{
    public class NewPuzzleProgressUI : MonoBehaviour
    {
        [SerializeField] private float _duration = 20.0f;
        [SerializeField] private Vector4 _startMask = new Vector4(0, 0, 0, 300); 
        [SerializeField] private RectMask2D[] _puzzles;

        private float _maskSize;
        private OpeningPuzzleData _currentPuzzle;
        private int _currentLevel;

        private void Awake()
        {
            _maskSize = _puzzles[0].GetComponent<RectTransform>().rect.height-100;

            (_currentPuzzle, _currentLevel) = GameManager.Instance.GetNewPuzzleProgressData();
            Debug.Log(string.Format("Current puzzle = {0}, current progress = {1}", _currentPuzzle, _currentLevel));
            ShowPuzzle(_currentPuzzle.puzzleNumber);

            int levelsCount = _currentPuzzle.levelEnd - _currentPuzzle.levelStart;
            float lastProgress = (float)(_currentLevel - 1 - _currentPuzzle.levelStart) / levelsCount;
            float targetProgress = (float)(_currentLevel - _currentPuzzle.levelStart) / levelsCount;
            float paddingTop = _maskSize - _maskSize * lastProgress;
            float targetPaddingTop = _maskSize - _maskSize * targetProgress;
            //Debug.Log(string.Format("levelsCount = {0}, lastProgress = {1}, targetProgress = {2}, paddingTop = {3}, targetPaddingTop = {4}", levelsCount, lastProgress, targetProgress, paddingTop, targetPaddingTop));
            Debug.Log(string.Format("level: {0}, puzzle level start: {1}", _currentLevel, _currentPuzzle.levelStart));
            _puzzles[_currentPuzzle.puzzleNumber].padding = new Vector4(0, 0, 0, paddingTop);
            _puzzles[_currentPuzzle.puzzleNumber].enabled = false; // Toggle to force update
            _puzzles[_currentPuzzle.puzzleNumber].enabled = true;

            // Activate needed puzzle
            StartCoroutine(AnimatePaddingTop(paddingTop, targetPaddingTop, _duration, _puzzles[_currentPuzzle.puzzleNumber]));
        }

        private void ShowPuzzle(int number)
        {
            foreach (RectMask2D puzzle in _puzzles)
            {
                puzzle.transform.parent.gameObject.SetActive(false);
            }
            _puzzles[number].transform.parent.gameObject.SetActive(true);
        }

        IEnumerator AnimatePaddingTop(float startValue, float endValue, float time, RectMask2D mask)
        {
            float elapsed = 0.0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float newPaddingTop = Mathf.Lerp(startValue, endValue, elapsed / time);
                mask.padding = new Vector4(0, 0, 0, newPaddingTop);
                mask.enabled = false; // Toggle to force update
                mask.enabled = true;
                yield return null;
            }
            mask.padding = new Vector4(0, 0, 0, endValue); // Установка конечного значения для избежания неточностей
            mask.enabled = false; // Toggle to force update
            mask.enabled = true;
        }
    }
}
