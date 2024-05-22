using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixItGame
{
    public class TutorialFingerController : MonoBehaviour
    {
        public SpriteRenderer _sprite; // Ссылка на sprite, который нужно активировать и скрывать
        public float _speed = 5f; // Базовая скорость движения
        public AnimationCurve speedCurve = AnimationCurve.Linear(0, 1, 1, 1); // Кривая скорости движения
        public float startDelay = 1f; // Задержка в начальной точке
        public float endDelay = 1f; // Задержка в конечной точке

        private Transform _puzzleController;
        private Transform _phantomController;
        private bool isMoving = false;

        void Start()
        {
            SearchingObjects();
        }

        void Update()
        {
            if (_puzzleController == null || _phantomController == null)
            {
                SearchingObjects();
            }
        }

        private void SearchingObjects()
        {
            var puzzleObj = FindObjectOfType<PuzzleController>();
            var phantomObj = FindObjectOfType<PhantomController>();

            if (puzzleObj != null && phantomObj != null)
            {
                _puzzleController = puzzleObj.transform;
                _phantomController = phantomObj.transform;
                StartCoroutine(MoveBetweenPoints());
            }
        }

        private IEnumerator MoveBetweenPoints()
        {
            while (true)
            {
                // Начальная точка
                Vector3 startPoint = _puzzleController.position;
                Vector3 endPoint = _phantomController.position;

                transform.position = _puzzleController.position;
                _sprite.enabled = true; // Активируем sprite

                yield return new WaitForSeconds(startDelay);

                // Движение к конечной точке с использованием кривой скорости
                yield return MoveToPoint(startPoint, endPoint);

                yield return new WaitForSeconds(endDelay);

                // Возвращение к начальной точке
                transform.position = _puzzleController.position;

                yield return new WaitForSeconds(startDelay);
            }
        }

        private IEnumerator MoveToPoint(Vector3 startPoint, Vector3 endPoint)
        {
            float journeyLength = Vector3.Distance(startPoint, endPoint);
            float journeyProgress = 0f;
            float journeyTime = 0f;
            isMoving = true;

            while (journeyProgress < 1f)
            {
                journeyTime += Time.deltaTime;
                journeyProgress = journeyTime / (journeyLength / _speed);
                float curveValue = speedCurve.Evaluate(journeyProgress);
                transform.position = Vector3.Lerp(startPoint, endPoint, journeyProgress);

                yield return null;
            }

            isMoving = false;
        }
    }
}
