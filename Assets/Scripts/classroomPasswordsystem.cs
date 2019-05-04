using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class classroomPasswordsystem : MonoBehaviour
{
    [SerializeField] GameObject classroomPasswordUI;
    public void QuitClassroomPassword()
    {
        classroomPasswordUI.SetActive(false);
    }
}
