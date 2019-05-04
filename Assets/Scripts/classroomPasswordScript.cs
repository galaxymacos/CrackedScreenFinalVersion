using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class classroomPasswordScript : MonoBehaviour
{
    [SerializeField] GameObject PasswordUI;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PasswordUI.SetActive(true);

        }
    }
}
