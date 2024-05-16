using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixItGame
{
    public class Cloud : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private bool _isVisible = false;
        [SerializeField] private float _minX = 0;

        private void Awake()
        {
            _minX = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect - 2;
        }

        private void Update()
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);

            // Проверка выхода за пределы экрана
            if (transform.position.x < _minX)
            {
                Destroy(gameObject);
            }
        }

        public void SetSpeed(float newSpeed)
        {
            _speed = newSpeed;
        }
    }
}
