using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMaker : BossAbility
{
    private Rigidbody rb;

    [SerializeField] private GameObject Tornado;
    [SerializeField] private float jumpForce = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TornadoJump()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0),ForceMode.Impulse);
    }

    public void spawnTornado()
    {
        GameObject tornadoIns = Instantiate(Tornado, transform.position, Quaternion.identity);
    }

    public override void Play()
    {
        GetComponent<Animator>().SetTrigger("TornadoMaker");
    }
}
