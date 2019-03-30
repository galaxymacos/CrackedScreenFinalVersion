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


    private void OnEnable()
    {
        controls.Player.DashUppercut.Enable();
        controls.Player.DashUppercut.performed += HandleDashUppercut;
        controls.Player.BlackHoleAttack.Enable();
        controls.Player.BlackHoleAttack.performed += HandleBlackHoleAttack;
        controls.Player.Defend.Enable();
        controls.Player.Defend.performed += HandleDefend;
        controls.Player.BasicAttack.Enable();
        controls.Player.BasicAttack.performed += HandleAirSlash;
        controls.Player.AirSlash.Enable();
        controls.Player.AirSlash.performed += HandleAirSlash;
        controls.Player.CounterStrike.Enable();
        controls.Player.CounterStrike.performed += HandleCounterAttack;
    }

    private void OnDisable()
    {
        controls.Player.DashUppercut.Disable();
        controls.Player.DashUppercut.performed -= HandleDashUppercut;
        controls.Player.BlackHoleAttack.Disable();
        controls.Player.BlackHoleAttack.performed -= HandleBlackHoleAttack;
        controls.Player.Defend.Disable();
        controls.Player.Defend.performed -= HandleDefend;
        controls.Player.BasicAttack.Disable();
        controls.Player.BasicAttack.performed -= HandleBasicAttack;
        controls.Player.AirSlash.Disable();
        controls.Player.AirSlash.performed -= HandleAirSlash;
        controls.Player.CounterStrike.Disable();
        controls.Player.CounterStrike.performed -= HandleCounterAttack;
    }

    private void HandleDashUppercut(InputAction.CallbackContext context)
    {
        if (CanPlayerPerformGroundAttack()) playerSkills[0].Play();
    }

    public void HandleBlackHoleAttack(InputAction.CallbackContext context)
    {
        if (CanPlayerPerformGroundAttack()) playerSkills[1].Play();
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