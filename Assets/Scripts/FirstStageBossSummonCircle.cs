using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStageBossSummonCircle : SummonCircle
{


    public void ChangeBgm()
    {
        AudioManager.instance.ChangeBgm("BaJianShenQu");
    }

    public void ShowFirstBossDialogue()
    {
        GetComponent<DialogueTrigger>().enabled = true;

    }
    
    public override void Update()
    {
        if (!enemy && hasSummoned)
        {
            var secondBossRock = GameObject.Find("SecondBossRock");
            secondBossRock.GetComponent<SecondBossGate>().firstBossDie = true;
            print("set first boss die = true");
            Destroy(gameObject);
        }
    }
}
