﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerComponent : MonoBehaviour
{
    private GameObject player;

    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
