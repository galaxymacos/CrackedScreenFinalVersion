using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerMovement _playerMovement;

    [SerializeField] private InputMaster controls;
    private readonly float counterAttackActivationDuration = 0.5f;

    private float counterAttackTimeRemains;
    [SerializeField] private Skill[] playerSkills;

    [SerializeField] private GameObject forceUppercutParticleEffect;
    


    private void OnEnable()
    {
        controls.Player.DashUppercut.Enable();
        controls.Player.DashUppercut.performed += HandleDashUppercut;
        controls.Player.BlackHoleAttack.Enable();
        controls.Player.BlackHoleAttack.performed += HandleHeadCatchAttack;
        controls.Player.Defend.Enable();
        controls.Player.Defend.performed += HandleDefend;
        controls.Player.BasicAttack.Enable();
        controls.Player.BasicAttack.performed += HandleBasicAttack;
        controls.Player.AirSlash.Enable();
        controls.Player.AirSlash.performed += HandleAirSlash;
        controls.Player.CounterAttack.Enable();
        controls.Player.CounterAttack.performed += HandleCounterAttack;
    }

    private void OnDisable()
    {
        controls.Player.DashUppercut.Disable();
        controls.Player.DashUppercut.performed -= HandleDashUppercut;
        controls.Player.BlackHoleAttack.Disable();
        controls.Player.BlackHoleAttack.performed -= HandleHeadCatchAttack;
        controls.Player.Defend.Disable();
        controls.Player.Defend.performed -= HandleDefend;
        controls.Player.BasicAttack.Disable();
        controls.Player.BasicAttack.performed -= HandleBasicAttack;
        controls.Player.AirSlash.Disable();
        controls.Player.AirSlash.performed -= HandleAirSlash;
        controls.Player.CounterAttack.Disable();
        controls.Player.CounterAttack.performed -= HandleCounterAttack;
    }

    private void HandleDashUppercut(InputAction.CallbackContext context)
    {
        if (CanPlayerPerformGroundAttack() || CanPlayerPerformDashUpperAttack())
        {
            if (CanPlayerPerformDashUpperAttack())
            {
                ForceAttackDashUppercut();
            }
            playerSkills[0].Play();
        }
    }

    /// <summary>
    /// This method is played when player has successfully used ability in a continuous strike
    /// </summary>
    private void ForceAttackDashUppercut()
    {
        print("force uppercut");
        forceUppercutParticleEffect.SetActive(true);
        StartCoroutine(DisableForceUppercut());
    }

    public IEnumerator DisableForceUppercut()
    {
        yield return new WaitForSeconds(1f);
        forceUppercutParticleEffect.SetActive(false);
    }

    public void HandleHeadCatchAttack(InputAction.CallbackContext context)
    {
        if (CanPlayerPerformGroundAttack() || CanPlayerPerformHeadCatchAttack()) {
            playerSkills[1].Play();
        }
    }

    public void HandleDefend(InputAction.CallbackContext context)
    {
        if (CanPlayerPerformGroundAttack()) playerSkills[3].Play();
    }

    public void HandleBasicAttack(InputAction.CallbackContext context)
    {
        if (CanPlayerPerformGroundAttack()) playerSkills[4].Play();
    }

    public void HandleAirSlash(InputAction.CallbackContext context)
    {
        if(CanPlayerPerformAirAttack()) playerSkills[2].Play();
    }

    public void HandleCounterAttack(InputAction.CallbackContext context)
    {
        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block && counterAttackTimeRemains > 0)
        {
            playerSkills[5].Play();
            counterAttackTimeRemains = 0;
        }
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private bool CanPlayerPerformGroundAttack()
    {
        return _playerController.canControl &&
               (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Stand ||
                _playerMovement.playerCurrentState == PlayerMovement.PlayerState.Walk);
    }

    private bool CanPlayerPerformHeadCatchAttack() {
        return PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Counter Attack") ||
               PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Uppercut");
    }

    private bool CanPlayerPerformDashUpperAttack() {
        DashUpper dashUpper = FindObjectOfType<DashUpper>();
        return PlayerProperty.animator.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack") && dashUpper._skillNotOnCooldown;
    }

    private bool CanPlayerPerformAirAttack()
    {
        return _playerController.canControl
                   && (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Jump
                   || _playerMovement.playerCurrentState == PlayerMovement.PlayerState.DoubleJump 
                   ||_playerMovement.playerCurrentState == PlayerMovement.PlayerState.FallDown);
    }

    private void Update()
    {
        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block && counterAttackTimeRemains > 0)
        {
            counterAttackTimeRemains -= Time.deltaTime;
        }
    }

    public void EnterCounterAttackMode()
    {
        counterAttackTimeRemains = counterAttackActivationDuration;
    }
}