using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");
    private AsyncOperation operation;


    public Animator Animator;

    private int levelToLoad;
    public static LevelChanger Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    public void FadeToLevel(int levelIndex)
    {
        operation = SceneManager.LoadSceneAsync(levelIndex);
        operation.allowSceneActivation = false;
        levelToLoad = levelIndex;
        Animator.SetTrigger(FadeOut);
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete()
    {
        operation.allowSceneActivation = true;
    }
}