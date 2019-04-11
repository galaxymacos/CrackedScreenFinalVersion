using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] internal GameObject selfDropPanel;
    [SerializeField] internal GameObject patrolEnemy;
    [SerializeField] internal GameObject ArcherEnemy;
    [SerializeField] internal GameObject FirstStageBoss;
    [SerializeField] internal GameObject SecondStageBoss;

    internal bool isDashingForward;

    internal bool piercingPlayer;
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
