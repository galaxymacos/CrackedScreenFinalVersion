using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class finalPuzzleFixBug : MonoBehaviour
{
    [SerializeField] GameObject finalPuzzleUI;
    [SerializeField] GameObject saveUI;
    [SerializeField] public InputField bugFixedInput;
    public bool bugFixedCorrect = false;

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
        }
    }
    public void SaveScripBotton()
    {
        saveUI.SetActive(false);
        finalPuzzleUI.SetActive(false);
        Confirm();
    }


}
