using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingAttack_FirstStageBoss : BossAbility
{
    private Animator animator;    // The animator of the first stage boss
    [SerializeField] private GameObject frisbee;

    [SerializeField] private GameObject frisbeeSpawnPlace;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Play()
    {
        animator.SetTrigger("RollingAttack");
    }

    public void SpawnFrisbee(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject frisbeeIns = Instantiate(frisbee, frisbeeSpawnPlace.transform.position, Quaternion.identity);
            frisbeeIns.GetComponent<FrisBee>().master = gameObject;
        }
    }
}
