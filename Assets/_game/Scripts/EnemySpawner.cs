using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemiesList;
    [SerializeField] private float _spawnRate;

    private float _currentSpawnTimer;
    private void Start()
    {
        _currentSpawnTimer = _spawnRate;
    }

    private void Update()
    {
        if(_currentSpawnTimer <= 0)
        {
            int rand = Random.Range(0, _enemiesList.Length);
            GameObject objectToSpawn = _enemiesList[rand];
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            _currentSpawnTimer = _spawnRate;
        }
        else
        {
            _currentSpawnTimer -= Time.deltaTime;
        }
    }

}
