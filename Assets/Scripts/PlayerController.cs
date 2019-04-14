using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraPosIn3D;


    // Player ability
    public bool canAwake;
    public bool canControl;
    public bool canControlDialogueBox;
    public float facingOffset = 1; // 1 if player is facing right and -1 if player is facing left
    private bool is3D;
    public bool isFacingRight;


    public delegate void OnFacingChange(bool isFacingRight);

    public OnFacingChange onFacingChangeCallback;

    // Player State
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float enter3DWorldDuration = 2f;
    [SerializeField] private GameObject dimensionLeapParticleEffect;
    [SerializeField] private GameObject dimensionLeapSucceedParticleEffect;
    public float powerAccumulateTime;
    public bool transferStoragePowerFull;

    private CameraEffect _cameraEffect;

    private PlayerMovement playerMovement;

    public float horizontalMovement;
    public float verticalMovement;

    [SerializeField] private InputMaster controls;

    private void OnEnable()
    {
        controls.Player.Movement.Enable();
        controls.Player.Movement.performed += HandleMovement;
        controls.Player.Jump.Enable();
       controls.Player.Jump.performed += HandleJump;
        controls.Player.DimensionLeap.Enable();
       controls.Player.DimensionLeap.performed += HandleTransform;
        controls.Player.DimensionLeap.cancelled += HandleTransformRelease;
    }

    private void OnDisable()
    {
        GameManager.Instance.controls.Player.Movement.Disable();
        GameManager.Instance.controls.Player.Movement.performed -= HandleMovement;
        GameManager.Instance.controls.Player.Jump.Disable();
        GameManager.Instance.controls.Player.Jump.performed -= HandleJump;
        GameManager.Instance.controls.Player.DimensionLeap.Disable();
        GameManager.Instance.controls.Player.DimensionLeap.performed -= HandleTransform;
        GameManager.Instance.controls.Player.DimensionLeap.cancelled -= HandleTransformRelease;
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (Camera.main != null)
        {
            mainCamera = Camera.main;
            _cameraEffect = mainCamera.GetComponent<CameraEffect>();
        }

        isFacingRight = true;
        onFacingChangeCallback?.Invoke(true);
        canControl = true;
        canControlDialogueBox = true;
    }

    public void HandleMovement(InputAction.CallbackContext context)
    {
        if (LevelManager.Instance.piercingPlayer) {    // Can't move when the player is being pierced
            
            return;
        }
        Vector2 moveAxis = context.ReadValue<Vector2>();
        horizontalMovement = moveAxis.x;
        verticalMovement = moveAxis.y;
    }

    public void HandleTransform(InputAction.CallbackContext context)
    {
        if (canAwake && canControl && playerMovement.playerCurrentState == PlayerMovement.PlayerState.Stand && !GameManager.Instance.is3D)
        {
            print("??");
            canControl = false;
            dimensionLeapParticleEffect.SetActive(true);
            AudioManager.instance.PlaySound(AudioGroup.Character, "DimensionLeapPreparing");
            Time.timeScale = 0.5f;
            _cameraEffect.StartShaking(0.1f); // Keep shaking
            isTransforming = true;
//            canControl = false;
        }
    }

    private bool isTransforming;

    public void HandleTransformRelease(InputAction.CallbackContext context)
    {
        if (!isTransforming)
            return;
        canControl = true;
        dimensionLeapParticleEffect.SetActive(false);
        AudioManager.instance.StopSound(AudioGroup.Character);
        isTransforming = false;
        powerAccumulateTime = 0;
        _cameraEffect.StopShaking();
        Time.timeScale = 1f;
        print(transferStoragePowerFull);
        if (transferStoragePowerFull)
        {
            dimensionLeapSucceedParticleEffect.SetActive(true);
            AudioManager.instance.PlaySound(AudioGroup.Character,"DimensionLeapTo3D");
            print("play 3d sound");
            GameManager.Instance.is3D = !GameManager.Instance.is3D;
            if (GameManager.Instance.is3D)
            {
                GameManager.Instance.OnSceneChangeCallback?.Invoke(true);
            }
            else
            {
                GameManager.Instance.OnSceneChangeCallback?.Invoke(false);
            }

            transferStoragePowerFull = false; // reset storage power full
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.is3D)
        {
            float rage = Mathf.Clamp(PlayerProperty.playerClass.maxRage * (powerAccumulateTime / enter3DWorldDuration), 0,
                PlayerProperty.playerClass.maxRage);
            PlayerProperty.playerClass.ChangeRageTo(rage);    
        }
        else
        {
            PlayerProperty.playerClass.ChangeRageTo(PlayerProperty.playerClass.rage-10*Time.deltaTime);    // TODO change to rageLoseSpeed
            if (PlayerProperty.playerClass.rage <= 0)
            {
                GameManager.Instance.OnSceneChangeCallback.Invoke(false);
            }
        }
        
        if (playerMovement.playerCurrentState == PlayerMovement.PlayerState.Stunned ||
            playerMovement.playerCurrentState == PlayerMovement.PlayerState.KnockUp ||
            playerMovement.playerCurrentState == PlayerMovement.PlayerState.Defend)
        {
            return;
        }
        if (canAwake)
        {
            CheckIfPlayerAwakes();
        }

    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        playerMovement.Jump();
        playerMovement.isGliding = true;
    }


    private void CheckIfPlayerAwakes()
    {
        if (isTransforming)
        {
            powerAccumulateTime += Time.deltaTime / Time.timeScale;
        }
        if (powerAccumulateTime > enter3DWorldDuration)
        {
            transferStoragePowerFull = true;
        }
    }

    private void FixedUpdate()
    {
        if (!canControl)
        {
            return;
        }

        if (isFacingRight)
            facingOffset = 1;
        else
            facingOffset = -1;


        playerMovement.Move(horizontalMovement, verticalMovement);

        ChangeFaceDirection(horizontalMovement);
    }

    private void ChangeFaceDirection(float horizontalMovement) // Player Controller
    {
        {
            if (horizontalMovement < 0)
            {
                if (isFacingRight)
                {
                    var localScale = transform.localScale;
                    transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
                    isFacingRight = false;
                    onFacingChangeCallback?.Invoke(false);
                }
            }
            else if (horizontalMovement > 0)
            {
                if (!isFacingRight)
                {
                    var localScale = transform.localScale;
                    transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
                    isFacingRight = true;
                    onFacingChangeCallback?.Invoke(true);
                }
            }
        }
    }
}