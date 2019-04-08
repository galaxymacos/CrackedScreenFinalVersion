using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitWall : MonoBehaviour
{
    private bool hasHitWall;
    internal bool piercingPlayer;
    [SerializeField] private int hitWallDamage = 15;
    [SerializeField] private AudioSource piercingHitWall;
    private PiercingSpear _piercingSpear;

    private void Start()
    {
        _piercingSpear = transform.parent.parent.GetComponent<PiercingSpear>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && !hasHitWall && piercingPlayer)
        {
            _piercingSpear.tookDamageInFirstStage = false;
            hasHitWall = true;
            piercingPlayer = false;
            piercingHitWall.Play();
            transform.parent.parent.GetComponent<Animator>().SetTrigger("PiercingSpearHitWall");
            PlayerProperty.controller.canControl = true;
            PlayerProperty.playerClass.TakeDamage(hitWallDamage);

            if (transform.parent.parent.position.x > PlayerProperty.player.transform.position.x)
            {
                PlayerProperty.playerClass.GetKnockOff(PlayerProperty.player.transform.position-new Vector3(2,0,0));
            }
            else
            {
                PlayerProperty.playerClass.GetKnockOff(PlayerProperty.player.transform.position+new Vector3(2,0,0));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            hasHitWall = false;
        }
    }
}
