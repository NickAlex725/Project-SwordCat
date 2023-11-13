using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneText : MonoBehaviour
{
    public string[] intro;
    private TextMeshProUGUI textMesh;
    private int currentText;
    // Start is called before the first frame update
    void Start()
    {
        currentText = 0;
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = intro[currentText];
    }

    void OnMouseDown()
    {
        currentText++;
        //check if reached end of text array
        if(currentText == intro.Length)
        {
            SceneManager.LoadScene("MainPlay");
        }
        else
        {
            textMesh.text = intro[currentText];
        }
    }
}
