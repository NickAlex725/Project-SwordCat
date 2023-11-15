using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitioner : MonoBehaviour
{
    [SerializeField] GameObject _level1Prefab;
    [SerializeField] GameObject _level2Prefab;
    [SerializeField] GameObject _level3Prefab;

    private GameObject _currentLevel;
    private int _levelCount = 1;
    public bool _isLastLevel;

    // Start is called before the first frame update
    void Start()
    {
        _level1Prefab = _currentLevel;
    }

    void OnTriggerEnter2D(Collider2D playerCol)
    {
        if(_isLastLevel) //win if on last level
        {
            SceneManager.LoadScene("WinCutscene");
        }
        else
        {
            //run fade to black animation (optional)

            //remove all current wave/level of enemies and obstacles
            GameObject[] waves = GameObject.FindGameObjectsWithTag("Wave");
            for(int i = 0; i < waves.Length; i++)
            {
                Destroy(waves[i]);
            }
            _levelCount++;

            //spawn new level prefab
            _currentLevel = null;
            if(_levelCount == 1)
            {
                _currentLevel = _level1Prefab;
                _level1Prefab.SetActive(true);
            }
            else if (_levelCount == 2)
            {
                _currentLevel = _level2Prefab;
                _level1Prefab.SetActive(false);
                _level2Prefab.SetActive(true);
            }
            else if (_levelCount == 3)
            {
                _currentLevel = _level3Prefab;
                _level2Prefab.SetActive(false);
                _level3Prefab.SetActive(true);
                _isLastLevel = true;
            }
            //make player come in from opposite screen
        }
    }
}
