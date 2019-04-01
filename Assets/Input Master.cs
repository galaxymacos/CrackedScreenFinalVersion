// GENERATED AUTOMATICALLY FROM 'Assets/Input Master.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class InputMaster : InputActionAssetReference
{
    public InputMaster()
    {
    }
    public InputMaster(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // Player
        m_Player = asset.GetActionMap("Player");
        m_Player_Movement = m_Player.GetAction("Movement");
        m_Player_BasicAttack = m_Player.GetAction("Basic Attack");
        m_Player_Defend = m_Player.GetAction("Defend");
        m_Player_DashUppercut = m_Player.GetAction("Dash Uppercut");
        m_Player_BlackHoleAttack = m_Player.GetAction("BlackHole Attack");
        m_Player_AirSlash = m_Player.GetAction("Air Slash");
        m_Player_Jump = m_Player.GetAction("Jump");
        m_Player_Run = m_Player.GetAction("Run");
        m_Player_CounterAttack = m_Player.GetAction("Counter Attack");
        m_Player_DimensionLeap = m_Player.GetAction("DimensionLeap");
        m_Player_Interact = m_Player.GetAction("Interact");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_Player = null;
        m_Player_Movement = null;
        m_Player_BasicAttack = null;
        m_Player_Defend = null;
        m_Player_DashUppercut = null;
        m_Player_BlackHoleAttack = null;
        m_Player_AirSlash = null;
        m_Player_Jump = null;
        m_Player_Run = null;
        m_Player_CounterAttack = null;
        m_Player_DimensionLeap = null;
        m_Player_Interact = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Player
    private InputActionMap m_Player;
    private InputAction m_Player_Movement;
    private InputAction m_Player_BasicAttack;
    private InputAction m_Player_Defend;
    private InputAction m_Player_DashUppercut;
    private InputAction m_Player_BlackHoleAttack;
    private InputAction m_Player_AirSlash;
    private InputAction m_Player_Jump;
    private InputAction m_Player_Run;
    private InputAction m_Player_CounterAttack;
    private InputAction m_Player_DimensionLeap;
    private InputAction m_Player_Interact;
    public struct PlayerActions
    {
        private InputMaster m_Wrapper;
        public PlayerActions(InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement { get { return m_Wrapper.m_Player_Movement; } }
        public InputAction @BasicAttack { get { return m_Wrapper.m_Player_BasicAttack; } }
        public InputAction @Defend { get { return m_Wrapper.m_Player_Defend; } }
        public InputAction @DashUppercut { get { return m_Wrapper.m_Player_DashUppercut; } }
        public InputAction @BlackHoleAttack { get { return m_Wrapper.m_Player_BlackHoleAttack; } }
        public InputAction @AirSlash { get { return m_Wrapper.m_Player_AirSlash; } }
        public InputAction @Jump { get { return m_Wrapper.m_Player_Jump; } }
        public InputAction @Run { get { return m_Wrapper.m_Player_Run; } }
        public InputAction @CounterAttack { get { return m_Wrapper.m_Player_CounterAttack; } }
        public InputAction @DimensionLeap { get { return m_Wrapper.m_Player_DimensionLeap; } }
        public InputAction @Interact { get { return m_Wrapper.m_Player_Interact; } }
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
    }
    public PlayerActions @Player
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new PlayerActions(this);
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get

        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.GetControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get

        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.GetControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
}
