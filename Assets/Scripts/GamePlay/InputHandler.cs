using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlScheme { KeyboardAndMouse, GamePad }

public class InputHandler : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerInput playerInput;

    public float movementHorizontal { get; private set; }
    public float movementVertical { get; private set; }

    public bool cameraLockButtonPressed { get; private set; }
    public float cameraHorizontal { get; private set; }
    public float cameraVertical { get; private set; }

    public Vector3 mousePosition { get { return playerControls.GamePlay.CannonLook.ReadValue<Vector2>(); } }

    public bool attackButtonPressed { get; private set; }

    public bool interactButtonPressed { get { return playerControls.GamePlay.Interaction.triggered; } }

    public bool interruptButtonPressed { get { return playerControls.GamePlay.Interrupt.triggered; } }
    public bool pauseButtonPressed { get { return playerControls.GamePlay.Pause.triggered; } }

    public bool statsButtonPressed { get { return playerControls.GamePlay.Stats.triggered; } }

    public bool nextWeaponButtonPressed { get { return playerControls.GamePlay.NextWeapon.triggered; } }
    public bool previousWeaponButtonPressed { get { return playerControls.GamePlay.PreviousWeapon.triggered; } }

    public ControlScheme currentControlScheme { get; private set; }

    public void Init()
    {
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        DisableAllInputs();
        SetGamePlayCallbacks();

        //Register callbacks
        GP_EventSystem.OnStartRoundEvent += EnableAllInputs;
        GP_EventSystem.OnBlockPlayerInputsEvent += DisableAllInputs;
    }

    private void OnDestroy()
    {
        GP_EventSystem.OnStartRoundEvent -= EnableAllInputs;
        GP_EventSystem.OnBlockPlayerInputsEvent -= DisableAllInputs;
    }

    private void SetGamePlayCallbacks()
    {
        playerInput.onControlsChanged += OnControlsChanged;

        //MOVEMENT
        playerControls.GamePlay.Movement.performed += ctx =>
        {
            movementHorizontal = playerControls.GamePlay.Movement.ReadValue<Vector2>().x;
            movementVertical = playerControls.GamePlay.Movement.ReadValue<Vector2>().y;
        };
        playerControls.GamePlay.Movement.canceled += ctx => { movementHorizontal = 0; movementVertical = 0; };

        //CAMERA
        playerControls.GamePlay.Look.performed += ctx =>
        {
            cameraHorizontal = Mathf.Clamp(playerControls.GamePlay.Look.ReadValue<Vector2>().x, -1, 1);//camera horizontal value
            cameraVertical = Mathf.Clamp(playerControls.GamePlay.Look.ReadValue<Vector2>().y, -1, 1);//camera vertical input
        };
        playerControls.GamePlay.Look.canceled += ctx => { cameraHorizontal = 0; cameraVertical = 0; };

        playerControls.GamePlay.CameraLock.started += ctx => cameraLockButtonPressed = true;
        playerControls.GamePlay.CameraLock.canceled += ctx => cameraLockButtonPressed = false;

        playerControls.GamePlay.Attack.started += ctx => attackButtonPressed = true;
        playerControls.GamePlay.Attack.canceled += ctx => attackButtonPressed = false;
    }

    private void OnControlsChanged(PlayerInput obj)
    {
        if (obj.currentControlScheme.Equals("KeyboardAndMouse"))
            currentControlScheme = ControlScheme.KeyboardAndMouse;
        else
            currentControlScheme = ControlScheme.GamePad;
    }

    private void EnableAllInputs()
    {
        playerControls.GamePlay.Enable();
    }

    private void DisableAllInputs()
    {
        playerControls.GamePlay.Disable();
    }
}