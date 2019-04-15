using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private Transform arrowPos;
    internal Vector3 flyDirection;
    internal float flySpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.is3D)
        {
            GetComponent<BoxCollider>().size = new Vector3(GetComponent<BoxCollider>().size.x,GetComponent<BoxCollider>().size.y,1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(flyDirection*flySpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerProperty.player)
        {
            HitPlayer();
        }
    }

    public void HitPlayer()
    {
        if (flyDirection.x < 0)
        {
            PlayerProperty.playerClass.GetKnockOff(PlayerProperty.playerPosition+new Vector3(2,0,0));
        }
        else
        {
            PlayerProperty.playerClass.GetKnockOff(PlayerProperty.playerPosition+new Vector3(-2,0,0));
        }
        PlayerProperty.playerClass.TakeDamage(damage);

        Destroy(gameObject);
    }
}
