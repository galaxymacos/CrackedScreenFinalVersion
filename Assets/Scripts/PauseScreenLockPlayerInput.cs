using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenLockPlayerInput : MonoBehaviour
{


    private void OnEnable()
    {
        PlayerProperty.controller.canControlDialogueBox = false;
    }

    private void OnDisable()
    {
        PlayerProperty.controller.canControlDialogueBox = true;
    }
}
