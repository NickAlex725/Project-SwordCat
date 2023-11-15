using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    [SerializeField] Enemy _levelOneEnemy;
    [SerializeField] Enemy _levelTwoEnemy;
    [SerializeField] Enemy _levelThreeEnemy;
    [SerializeField] float _checkInterval;
    [SerializeField] GameObject _nextLevelTransition;

    private int _numberOfLevelOne;
    private int _numberOfLevelTwo;
    private int _numberOfLevelThree;
    private float _currentCheckTime;
    private void Start()
    {
        _currentCheckTime = _checkInterval;
        _nextLevelTransition.SetActive(false);
    }
    private void Update()
    {
        if (_numberOfLevelOne <= 0 && _numberOfLevelTwo <= 0 && _numberOfLevelThree <= 0)
        {
            _nextLevelTransition.SetActive(true);
        }
        if (_currentCheckTime <= 0)
        {
            IsLevelPossibleCheck();

            //reset counts
            _numberOfLevelOne = 0;
            _numberOfLevelTwo = 0;
            _numberOfLevelThree = 0;

            //make a list of all eneimes
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //loop through list and count how many of each level
            for (int i = 0; i < enemies.Length; i++)
            {
                var enemyLevel = enemies[i].GetComponent<Enemy>().GetEnemyLevel();
                if(enemyLevel == 1)
                {
                    _numberOfLevelOne++;
                }
                else if (enemyLevel == 2)
                {
                    _numberOfLevelTwo++;
                }
                else if (enemyLevel == 3)
                {
                    _numberOfLevelThree++;
                }
            }

            //print counts
            Debug.Log("level 1: " + _numberOfLevelOne);
            Debug.Log("level 2: " + _numberOfLevelTwo);
            Debug.Log("level 3: " + _numberOfLevelThree);

            //reset counter
            _currentCheckTime = _checkInterval;
        }
        else
        {
            _currentCheckTime -= Time.deltaTime;
        }
    }

    private void IsLevelPossibleCheck()
    {
        if (_numberOfLevelThree > _numberOfLevelTwo && _numberOfLevelTwo > _numberOfLevelOne)
        {
            Debug.Log("need more level 2 and 1 enemies");
        }
        else if (_numberOfLevelTwo > _numberOfLevelOne && _numberOfLevelThree !> _numberOfLevelTwo)
        {
            Debug.Log("need more level 1 enemies");
        }
        else if (_numberOfLevelTwo >= 0 && _numberOfLevelOne <= 0)
        {
            Debug.Log("level should be beatable");
        }
    }
}
