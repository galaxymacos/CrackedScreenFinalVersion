using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class finalPuzzleFixBug : MonoBehaviour
{
    [SerializeField] GameObject finalPuzzleUI;
    [SerializeField] GameObject saveUI;
    [SerializeField] GameObject FadeInUI;
    [SerializeField] public InputField bugFixedInput;
    [SerializeField] public Image whiteFade;
    public bool bugFixedCorrect = false;

    private void Start()
    {
        whiteFade.canvasRenderer.SetAlpha(0.0f);
     
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            finalPuzzleUI.SetActive(true);

        }
    }

    public void QuitFinalPuzzle()
    {
        saveUI.SetActive(true);
        //finalPuzzleUI.SetActive(false);
    }

    public void Confirm()
    {
        if (bugFixedInput.text == "canGoTo3dWorld=false;")
        {
            bugFixedCorrect = true;
            Debug.Log("Bug Has Been Fiexed");
            FadeInUI.SetActive(true);
            fadeIn();
        }
    }
    public void SaveScripBotton()
    {
        saveUI.SetActive(false);
        finalPuzzleUI.SetActive(false);
        Confirm();
    }

    void fadeIn()
    {
        whiteFade.CrossFadeAlpha(1, 2, false);
    }
    void fadeOut()
    {
        whiteFade.CrossFadeAlpha(0, 2, false);
    }
}
