using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteriousMan : MonoBehaviour
{
    public GameObject dialogueBox;
    private FirstStageBoss _firstStageBoss;

    private void Start()
    {
        _firstStageBoss = transform.GetComponent<FirstStageBoss>();
//        bossEnemy.OnBossDieCallback += ActivateBossDieDialogue;
    }

    public void ActivateBossDieDialogue()
    {
        dialogueBox.SetActive(true);
    }
}