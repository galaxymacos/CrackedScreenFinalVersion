using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMaker : BossAbility
{
    private Rigidbody rb;
    private float extraGravity = -130f;
    private float jumpForce = 100f;
    private float horizontalJumpForce = 60f;

    private bool appliedExtraGravity;

    [SerializeField] private GameObject Tornado;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("TornadoMaker"))
        {
            rb.AddForce(new Vector3(0, extraGravity));
        }
    }

    public void TornadoJump()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0),ForceMode.Impulse);
        if (PlayerProperty.playerPosition.x<transform.position.x)
        {
            rb.AddForce(new Vector3(-horizontalJumpForce, 0, 0),ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(new Vector3(horizontalJumpForce, 0, 0),ForceMode.Impulse);
        }
        AudioManager.instance.StopSound(AudioGroup.FirstBoss);
        AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"TornadoJump");
    }

    public void spawnTornado()
    {
        LayerMask groundLayer = 1 << 11;
        if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1000, groundLayer))
        {
            GameObject tornadoIns = Instantiate(Tornado, hitInfo.point, Quaternion.identity);
        }
        AudioManager.instance.PlaySound(AudioGroup.FirstBoss,"TornadoStomp");
        
    }

    public override void Play()
    {
        GetComponent<Animator>().SetTrigger("TornadoMaker");
        appliedExtraGravity = true;
    }
}
