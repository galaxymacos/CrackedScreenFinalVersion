using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIScript : MonoBehaviour
{
    [SerializeField] GameObject deathUI;
    public void Restart()
    {
        SceneManager.LoadSceneAsync("SchoolScene", LoadSceneMode.Additive);
    }
    public void Continue()
    {
        fenceScript.death = false;
        deathUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
