using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextLevelDelay : MonoBehaviour
{
    [SerializeField] private float displayTime = 3f;

    private float displayTimeRemains;
    private LevelChanger levelCurtain;

    private void Start()
    {
        displayTimeRemains = displayTime;
        levelCurtain = GetComponent<LevelChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (displayTimeRemains > 0)
        {
            displayTimeRemains -= Time.deltaTime;
            if (displayTimeRemains <= 0)
            {
                levelCurtain.FadeToNextLevel();
                enabled = false;
            }
        }
    }
}
