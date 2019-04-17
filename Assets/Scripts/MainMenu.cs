using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject startArm;
    [SerializeField] GameObject optionArm;
    [SerializeField] GameObject QuitArm;
    [SerializeField] AudioClip clickSound;
    private LevelChanger levelChanger;
    private AudioSource souce;

    private void Start()
    {
        souce = GetComponent<AudioSource>();
        levelChanger = GameObject.Find("Level Curtain").GetComponent<LevelChanger>();
    }

    public void OptionMenuQuit()
    {
        souce.Play();
    }

    public void PlayGame()
    {
        levelChanger.FadeToNextLevel();
//        souce.Play();
        startArm.SetActive(true);
        optionArm.SetActive(false);
        QuitArm.SetActive(false);
        
    }

    public void OptionArm()
    {
        souce.Play();
        startArm.SetActive(false);
        optionArm.SetActive(true);
        QuitArm.SetActive(false);
    }

    public void QuitGame()
    {
        souce.Play();
        startArm.SetActive(false);
        optionArm.SetActive(false);
        QuitArm.SetActive(true);
        Debug.Log("Quit");
        Application.Quit();
    }
}