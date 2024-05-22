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
            _maskSize = _puzzles[0].GetComponent<RectTransform>().rect.height;

            (_currentPuzzle, _currentLevel) = GameManager.Instance.GetNewPuzzleProgressData();

            int levelsCount = _currentPuzzle.levelEnd - _currentPuzzle.levelStart;
            float lastProgress = (float)(_currentLevel - 1 - _currentPuzzle.levelStart) / levelsCount;
            float targetProgress = (float)(_currentLevel - _currentPuzzle.levelStart) / levelsCount;
            float paddingTop = _maskSize - _maskSize * lastProgress;
            float targetPaddingTop = _maskSize - _maskSize * targetProgress;

            ShowPuzzle(_currentPuzzle.puzzleNumber, paddingTop);
            Debug.Log(string.Format("new Puzzle last progress: {0}, current progress:{1}", lastProgress, targetProgress));

            // Activate needed puzzle
            StartCoroutine(AnimatePaddingTop(paddingTop, targetPaddingTop, _duration, _puzzles[_currentPuzzle.puzzleNumber]));
        }

        private void ShowPuzzle(int number, float paddingTop)
        {
            foreach (RectMask2D puzzle in _puzzles)
            {
                puzzle.transform.parent.gameObject.SetActive(false);
            }
            _puzzles[number].transform.parent.gameObject.SetActive(true);
            _puzzles[number].padding = new Vector4(0, 0, 0, paddingTop);
            //_puzzles[_currentPuzzle.puzzleNumber].enabled = false;
            //_puzzles[_currentPuzzle.puzzleNumber].enabled = true;
        }

        IEnumerator AnimatePaddingTop(float startValue, float endValue, float time, RectMask2D mask)
        {
            float elapsed = 0.0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float newPaddingTop = Mathf.Lerp(startValue, endValue, elapsed / time);
                mask.padding = new Vector4(0, 0, 0, newPaddingTop);
                //mask.enabled = false; // Toggle to force update
                //mask.enabled = true;
                yield return null;
            }
            mask.padding = new Vector4(0, 0, 0, endValue); // Установка конечного значения для избежания неточностей
            //mask.enabled = false; // Toggle to force update
            //mask.enabled = true;
        }
    }
}
