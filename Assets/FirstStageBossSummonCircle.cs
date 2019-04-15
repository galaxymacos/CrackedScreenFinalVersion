using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStageBossSummonCircle : MonoBehaviour
{


    public void ChangeBgm()
    {
        AudioManager.instance.ChangeBgm("BaJianShenQu");
    }

    public void ShowFirstBossDialogue()
    {
        GetComponent<DialogueTrigger>().enabled = true;

    }
}
