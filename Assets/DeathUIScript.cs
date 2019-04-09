using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIScript : MonoBehaviour
{
    private float noDamageDuration = 1.0f;
    private bool noDamage = false;
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
        noDamage = true;
    }
    void Update()
    {
        if (noDamage)
        {
            noDamageDuration = noDamageDuration-= Time.deltaTime;
            fenceScript.death = false;
            if (noDamageDuration < 0)
            {
                noDamageDuration = 1.0f;
                noDamage = false;
            }
        }   
    }
}
