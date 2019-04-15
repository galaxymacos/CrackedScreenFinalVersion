﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // delegate methods
    public delegate void OnPlayerDie();

    internal OnPlayerDie onPlayerDieCallback;

    public delegate void OnSceneChange(bool is3D);

    internal OnSceneChange OnSceneChangeCallback;

    [SerializeField] internal InputMaster controls;


    private const string _playerSaveCoordinate = "PlayerLastCoordinate";


    public bool gameIsOver;
    public bool gameIsPaused;
    public bool isPlayerDying;
    // 打擊頓幀
    private bool istimeSlowing;
    internal float HitPauseTimeRemain;

    public List<GameObject> gameObjects; // Including enemy, player, and environment
    public GameObject gameOverPanel; // TODO
    [SerializeField] private Sprite healthImage;

    internal bool is3D;

    public List<Image> livesUI;

    public GameObject player;
    public int playerLives = 3;

    private float originalZ;    // The z value of every game object in 2D

    public Animator animator;
    public PlayerAnimator PlayerAnimator;
    
    
    //

    #region UI

    public GameObject floatingDamage;
    public GameObject blood;
    public GameObject smallBlood;

    #endregion

    #region Enemy

    internal GameObject lastHitEnemy;
    internal float lastHitEnemyTime;

    #endregion

    public static GameManager Instance { get; private set; }

    internal readonly List<LiveObject> liveObjects = new List<LiveObject>();    // use to change the collider of all the gameobjects in run time when switch between 2D and 3D

    private void Awake()
    {
        if (Instance == null) Instance = this;
        originalZ = player.transform.position.z;
        
    }

    public bool PlayerDying;

    private void Start()
    {
        CreatePlayerSaveSpot();

        RefreshHeartUi();
        OnSceneChangeCallback += RearrangeObjectsBasedOnScene;
        OnSceneChangeCallback?.Invoke(false);

        onPlayerDieCallback += RefreshHeartUi;
        
        
    }

    private void Update()
    {
        if (!DialogueManager.Instance._dialogueStarted && !gameIsPaused)
        {
            if (HitPauseTimeRemain > 0)
            {
                Time.timeScale = 0.1f;
                HitPauseTimeRemain -= Time.deltaTime / Time.timeScale;
            }
            else
            {
                Time.timeScale = 1f;
            }    
        }
        
    }
    
    [SerializeField] private GameObject[] nums;

    
    public void SpawnText(int damage,Vector3 position)
    {
        print("Spawn damage text");
//        if (damage < 10)
//        {
//            var num = Instantiate(nums[damage], position, Quaternion.identity);
//            num.transform.SetParent(null);
//        }
//        else if (damage < 100)
//        {
//            int NumInTen = damage / 10;
//            var num1 = Instantiate(nums[NumInTen], position+new Vector3(-1,0), Quaternion.identity);
//            
//            int NumInOne = damage % 10;
//            var num2 = Instantiate(nums[NumInOne], position+new Vector3(1,0), Quaternion.identity);
//
//            num1.transform.SetParent(null);
//            num2.transform.SetParent(null);
//        }
    }

    public void CreatePlayerSaveSpot()
    {
        var playerPosition = player.transform.position;

        PlayerPrefs.SetString(_playerSaveCoordinate, $"{playerPosition.x},{playerPosition.y},{playerPosition.z}");
    }

    private void RearrangeObjectsBasedOnScene(bool changeTo3D)
    {
        if (changeTo3D)
        {
            is3D = true;
            foreach (LiveObject liveObject in liveObjects)
            {
                if (liveObject != null)
                {
                    if (liveObject.boxTriggerColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<BoxCollider, float> pair in liveObject.boxTriggerColliderSizeDictionary)
                        {
                            pair.Key.size = new Vector3(pair.Key.size.x, pair.Key.size.y,pair.Value);
                        }
                    }

                    if (liveObject.boxColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<BoxCollider, float> pair in liveObject.boxColliderSizeDictionary)
                        {
                            pair.Key.size = new Vector3(pair.Key.size.x, pair.Key.size.y,pair.Value);
                        }
                    }
                    
                    if (liveObject.capsuleTriggerColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<CapsuleCollider, float> pair in liveObject.capsuleTriggerColliderSizeDictionary)
                        {
                            pair.Key.height = pair.Value;
                        }
                    }

                    if (liveObject.capsuleColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<CapsuleCollider, float> pair in liveObject.capsuleColliderSizeDictionary)
                        {
                            pair.Key.height = pair.Value;
                        }
                    }
                }
            }

        }
        else
        {
            is3D = false;
            print("Change to 2D scene");
            foreach (LiveObject liveObject in liveObjects)
            {
                if (liveObject != null)
                {
                    if (liveObject.boxTriggerColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<BoxCollider, float> pair in liveObject.boxTriggerColliderSizeDictionary)
                        {
                            pair.Key.size = new Vector3(pair.Key.size.x, pair.Key.size.y,1000);
                        }
                    }
                    
                    if (liveObject.boxColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<BoxCollider, float> pair in liveObject.boxColliderSizeDictionary)
                        {
                            pair.Key.size = new Vector3(pair.Key.size.x, pair.Key.size.y,1000);
                        }
                    }
                    
                    if (liveObject.capsuleTriggerColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<CapsuleCollider, float> pair in liveObject.capsuleTriggerColliderSizeDictionary)
                        {
                            pair.Key.height = 5000;
                        }
                    }

                    if (liveObject.capsuleColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<CapsuleCollider, float> pair in liveObject.capsuleColliderSizeDictionary)
                        {
                            pair.Key.height = 5000;
                        }
                    }
                }
            }
        }
    }

    public void DecreaseLifeNum()
    {
        playerLives -= 1;
        if (playerLives <= 0)
        {
            Gameover();
        }
    }

    public bool isGameOver;
    private void Gameover()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true); // Turn on the game over menu
//        AudioManager.instance.SwitchBgm("");
        Time.timeScale = 0f;
        
        PlayerPrefs.DeleteAll(); // Delete all player data when game is turned off
    }

    private void RefreshHeartUi()
    {
        for (var i = 0; i < livesUI.Count; i++) {
            if (i < playerLives) {

                livesUI[i].enabled = true;
            }
            else {
                livesUI[i].enabled = false;
            }
        }
    }
    
    
}