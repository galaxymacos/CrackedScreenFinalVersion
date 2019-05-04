using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class finalPuzzleFixBug : MonoBehaviour
{
    [SerializeField] GameObject finalPuzzleUI;
    [SerializeField] GameObject saveUI;
    [SerializeField] GameObject FadeInUI;
    [SerializeField] GameObject FadeOutUI;
    [SerializeField] GameObject EndImageUI;
    [SerializeField] public InputField bugFixedInput;
    [SerializeField] public Image whiteFade;
    [SerializeField] public Image whiteFadeOut;
    public bool bugFixedCorrect = false;
    private bool startTofadeIn = false;
    private float fadeInDuration = 2.0f;

    private void Start()
    {
        whiteFade.canvasRenderer.SetAlpha(0.0f);
        whiteFadeOut.canvasRenderer.SetAlpha(1.0f);

     
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
        if (bugFixedInput.text == "canGoTo3DWorld=false;")
        {
            bugFixedCorrect = true;
            Debug.Log("Bug Has Been Fiexed");
            FadeInUI.SetActive(true);
             fadeIn();
            startTofadeIn = true;
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
        whiteFadeOut.CrossFadeAlpha(0, 2, false);
    }

    private void Update()
    {
        if (startTofadeIn)
        {
            fadeInDuration -= Time.deltaTime;
            if (fadeInDuration < 0)
            { 
               EndImageUI.SetActive(true);
                FadeOutUI.SetActive(true);
                fadeOut();
                if (fadeInDuration < -5)
                {
                    SceneManager.LoadScene("StartMenu");
                }
            }
        }
    }
}
