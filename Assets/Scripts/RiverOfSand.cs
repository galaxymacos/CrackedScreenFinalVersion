using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RiverOfSandTilingDirection
{
    Positive,
    Negative
}
public class RiverOfSand : MonoBehaviour
{
    
    private EnemyDetector playerDetector;
    [SerializeField] private RiverOfSandTilingDirection direction;
    [SerializeField] private float speed;

    private bool playerWasInRiver;
    // Start is called before the first frame update
    void Start()
    {
        playerDetector = GetComponent<EnemyDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetector.playerInRange())
        {
            playerWasInRiver = true;
            if (direction == RiverOfSandTilingDirection.Positive)
            {
                PlayerProperty.player.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,speed));
            }
            else
            {
                PlayerProperty.player.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,-speed));
            }
        }
        else
        {
            if (playerWasInRiver)
            {
                playerWasInRiver = false;
                PlayerProperty.player.GetComponent<Rigidbody>().velocity = new Vector3(PlayerProperty.player.GetComponent<Rigidbody>().velocity.x,PlayerProperty.player.GetComponent<Rigidbody>().velocity.y,0);
            }

        }
    }
    

}
