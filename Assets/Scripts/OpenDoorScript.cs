using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorScript : MonoBehaviour
{
    [SerializeField] GameObject inputfieldUI;
    public Animation door;
    private bool doorOpen = false;

    private void Start()
    {
        door = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            inputfieldUI.SetActive(true);
          
        
        }
    }
    private void Update()
    {
        if (GetandSetText.passwordCorrect)
        {
            if (!doorOpen)
            {
                door.Play("open");
                doorOpen = true;
            }
      
        }
    }

}
