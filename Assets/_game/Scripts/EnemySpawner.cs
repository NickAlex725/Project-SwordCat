using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private bool _spawnInOrder;
    [SerializeField] private GameObject[] _enemiesList;
    [SerializeField] private GameObject[] _wavesList;
    [SerializeField] private float _spawnPerX;
    [SerializeField] private int _spawnXTimes;

    private int _numberOfLevel3;
    private int _numberOfLevel2;
    private int _numberOfLevel1;

    private float _currentSpawnTimer;
    private void OnEnable()
    {
        _currentSpawnTimer = _spawnPerX;
    }

    private void Update()
    {
        if(_currentSpawnTimer <= 0)
        {
            //For Spawning individual enemies
            if(_enemiesList.Length > 0 && !_spawnInOrder)
            {
                int rand = Random.Range(0, _enemiesList.Length);
                GameObject objectToSpawn = _enemiesList[rand];
                Instantiate(objectToSpawn, transform.position, Quaternion.identity);

            }
            else if(_enemiesList.Length > 0 && _spawnInOrder && _spawnXTimes > 0)
            {
                GameObject objectToSpawn = _enemiesList[_spawnXTimes-1];
                Instantiate(objectToSpawn, transform.position, Quaternion.identity);
                _spawnXTimes--;
            }
            
            //For spawning groups/waves of multiple enemies
            if(_wavesList.Length > 0 && !_spawnInOrder)
            {
                int rand = Random.Range(0, _wavesList.Length);
                GameObject objectToSpawn = _wavesList[rand];
                Instantiate(objectToSpawn);
            }else if(_wavesList.Length > 0 && _spawnInOrder && _spawnXTimes > 0)
            {
                GameObject objectToSpawn = _wavesList[_spawnXTimes-1];
                Instantiate(objectToSpawn);
                _spawnXTimes--;
            }

            _currentSpawnTimer = _spawnPerX;
        }
        else
        {
            _currentSpawnTimer -= Time.deltaTime;
        }
    }
}
