using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRun : MonoBehaviour {

    [SerializeField] private Vector3 knockOffForce;
    [SerializeField] private EnemyDetector homeRunHitBox;

    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void HomeRunHitPlayer() {
        AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"DragonFistStrike");
        print("Home run hits player");
        animator.SetBool("DragonFistHitPlayer",false);
        if (homeRunHitBox.playerInRange())
        {
            PlayerProperty.playerClass.GetKnockOff(knockOffForce);
            PlayerProperty.playerClass.TakeDamage(100);
        }
    }

    public void CameraSizeLower()
    {
        Camera.main.GetComponent<CameraEffect>().EnlargeCamera(Camera.main.orthographicSize*0.7f);
    }

    public void PlayHomeRunPrepareSound()
    {
        AudioManager.instance.PlaySound(AudioGroup.SecondBoss,"HomeRunPrepare");
    }
}
