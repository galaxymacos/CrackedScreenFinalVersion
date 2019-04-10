using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalPuzzleFixBug : MonoBehaviour
{
    [SerializeField] GameObject finalPuzzleUI;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            finalPuzzleUI.SetActive(true);

        }
    }

    public void QuitFinalPuzzle()
    {
        finalPuzzleUI.SetActive(false);
    }
}
