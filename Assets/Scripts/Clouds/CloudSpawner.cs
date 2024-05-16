using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixItGame
{
    public class CloudSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _cloudPrefabs; // Префаб облака
        [SerializeField] private int _cloudsAmount;
        [SerializeField] private float _spawnRate = 2f;   // Частота генерации облаков в секундах
        [SerializeField] private float _cloudMinY = -5f;  // Минимальная высота появления облаков
        [SerializeField] private float _cloudMaxY = 5f;   // Максимальная высота появления облаков
        [SerializeField] private float _maxCloudSpeed = 1f;
        [SerializeField] private float _minCloudSpeed = 0.5f;

        private float nextSpawnTime = 5f;

        private void Awake()
        {
            _cloudsAmount = _cloudPrefabs.Length - 1;
        }

        private void Update()
        {
            if (Time.time > nextSpawnTime)
            {
                SpawnCloud();
                nextSpawnTime = Time.time + _spawnRate;
            }
        }

        private void SpawnCloud()
        {
            float spawnY = Random.Range(_cloudMinY, _cloudMaxY);
            Vector3 spawnPosition = new Vector3(transform.position.x, spawnY, 0);
            GameObject cloud = Instantiate(_cloudPrefabs[Random.Range(0, _cloudsAmount)], spawnPosition, Quaternion.identity);
            cloud.transform.localScale *= Random.Range(0.8f, 1.1f); // Размер облака

            // Назначаем случайную скорость облаку
            float speed = Random.Range(_minCloudSpeed, _maxCloudSpeed);
            cloud.GetComponent<Cloud>().SetSpeed(speed);
        }
    }
}
