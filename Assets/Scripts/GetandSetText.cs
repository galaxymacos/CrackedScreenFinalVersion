using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetandSetText : MonoBehaviour
{
    [SerializeField] GameObject keyPadUI;
    public InputField password;
    static public bool passwordCorrect=false;

 public void Confirm()
    {
        if (password.text == "YOURTEAMDESERVES100")
        {
            passwordCorrect = true;
            keyPadUI.SetActive(false);
        }
    }
    public void QuitKeyPad()
    {
        keyPadUI.SetActive(false);
    }
}
