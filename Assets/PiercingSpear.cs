using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingSpear : BossAbility
{
    private float pierceSpeed = 10f;
    

    private void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PiercingSpear"))
        {
            
        }
        
    }

    public override void Play()
    {
        GetComponent<Animator>().SetTrigger("PiercingSpear");
    }
}
