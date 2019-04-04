using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingAttack_FirstStageBoss : BossAbility
{
    private Animator animator;    // The animator of the first stage boss

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Play()
    {
        animator.SetTrigger("RollingAttack");
    }
}
