using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManger : MonoBehaviour
{
    public void StartButton(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void CreditsButton()
    {

    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
