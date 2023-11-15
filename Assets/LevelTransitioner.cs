using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitioner : MonoBehaviour
{
    public bool _isLastLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D playerCol)
    {
        if(_isLastLevel) //win if on last level
        {
            SceneManager.LoadScene("WinCutscene");
        }else
        {
            //run fade to black animation (optional)

            //remove all current wave/level of enemies and obstacles
            GameObject[] waves = GameObject.FindGameObjectsWithTag("Wave");
            for(int i = 0; i < waves.Length; i++)
            {
                Destroy(waves[i]);
            }
            //spawn new level prefab

            //make player come in from opposite screen
        }
    }
}
