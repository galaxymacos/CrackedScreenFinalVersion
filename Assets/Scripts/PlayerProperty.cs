using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    internal static GameObject player;

    public static Vector3 playerPosition => player.transform.position;

    internal static Player playerClass;

    internal static PlayerMovement movementClass;
    
    internal static Animator animator;

    internal static PlayerController controller;

    internal static bool hasSuckedEnemy;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
        playerClass = player.GetComponent<Player>();
        movementClass = player.GetComponent<PlayerMovement>();
        controller = player.GetComponent<PlayerController>();
        animator = GameManager.Instance.animator;
    }
}
