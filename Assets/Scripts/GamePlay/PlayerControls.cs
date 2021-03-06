// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/GamePlay/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""eb40caa6-f35d-4b0f-b42d-8597af13926b"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""0fc2dc98-0da4-4200-abae-20d299a9b53a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""7c493351-d973-4f00-a51e-0e2e1bdffa0b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraLock"",
                    ""type"": ""Button"",
                    ""id"": ""50b5854b-cc2e-403a-9476-df1a799dbc5c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1,behavior=2)""
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""da605d2d-b5c0-46e7-9b74-a5e1a0223413"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                },
                {
                    ""name"": ""Interrupt"",
                    ""type"": ""Button"",
                    ""id"": ""08d31b66-d4c7-416e-966a-6cb44f0a9558"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e41aa536-7a15-4aa5-819f-146865301763"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""23e4ef09-8b7b-4a6b-a9b9-63b7df1ac527"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1,behavior=2)""
                },
                {
                    ""name"": ""CannonLook"",
                    ""type"": ""Value"",
                    ""id"": ""aab71267-33cc-49cd-b508-5d0a19c06a90"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                },
                {
                    ""name"": ""Stats"",
                    ""type"": ""Button"",
                    ""id"": ""b4fd4554-4571-4f60-abd1-2e6c309770a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                },
                {
                    ""name"": ""NextWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""3b985f4a-7e8f-4dba-bc56-ba7182359fab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                },
                {
                    ""name"": ""PreviousWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""f48650a3-66a6-431d-ad89-c65220c24028"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""dfcc1671-b013-4326-b3e8-3e6346d94889"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e66f0e43-db18-44ff-8cb1-95998c427ba3"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0a13d788-13b9-4613-aa3c-c7be3696638e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3d07499d-e13e-48c8-a1f7-0ba44cb7adf3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0a6df2ff-de77-4aee-8dd5-610102870a95"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2f215745-110e-4558-92a9-18e5bf9076c1"",
                    ""path"": ""<DualShockGamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb390a5f-1e68-43c2-9226-bd2c187841d5"",
                    ""path"": ""<DualShockGamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5fd0976a-b563-43a1-a65d-52ff99c87c94"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3402fee3-f9dd-44c0-a7b8-cbebb04fd659"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""CameraLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5bc100e-51cb-4507-9143-67f4fa612772"",
                    ""path"": ""<DualShockGamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb900a64-e4cd-489d-a910-352aa1d6e3be"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44e17d13-7ad7-4eb0-ba3d-db802f1bc720"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b736f7cc-5696-4916-8591-70080c7a0f80"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23f05b8f-6a88-49dd-aa1f-8def03f78beb"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Interrupt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7f2dea7-8376-4b14-818e-64a92ee881a0"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Interrupt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70c8ae7e-08fd-4172-a016-e019760689b4"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b73c341f-3438-4c4a-863f-af705caf6fa0"",
                    ""path"": ""<DualShockGamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3abf141-37eb-4e48-b9cf-6414880068cf"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""CannonLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""285b65ab-f986-4f4f-ad53-cb5c59feee00"",
                    ""path"": ""<DualShockGamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Stats"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ce80b76-e8d7-4a75-a3bc-f25cbffc006f"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Stats"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa04d26a-dbdc-4028-be02-0a4691db0076"",
                    ""path"": ""<Mouse>/forwardButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""NextWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6dc8db7f-a247-451c-9b53-d4c85c00c0d8"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""NextWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6be41da7-91d5-4227-8086-a85121085200"",
                    ""path"": ""<DualShockGamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""NextWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80504d4b-3fe4-4aca-8508-5df5cbc3485c"",
                    ""path"": ""<Mouse>/backButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""PreviousWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2089204-d2dd-48d9-b389-32f0f558defc"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""PreviousWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5254e4fc-bc59-44f5-954a-b7c0798fae28"",
                    ""path"": ""<DualShockGamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PreviousWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardAndMouse"",
            ""bindingGroup"": ""KeyboardAndMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_Movement = m_GamePlay.FindAction("Movement", throwIfNotFound: true);
        m_GamePlay_Look = m_GamePlay.FindAction("Look", throwIfNotFound: true);
        m_GamePlay_CameraLock = m_GamePlay.FindAction("CameraLock", throwIfNotFound: true);
        m_GamePlay_Interaction = m_GamePlay.FindAction("Interaction", throwIfNotFound: true);
        m_GamePlay_Interrupt = m_GamePlay.FindAction("Interrupt", throwIfNotFound: true);
        m_GamePlay_Pause = m_GamePlay.FindAction("Pause", throwIfNotFound: true);
        m_GamePlay_Attack = m_GamePlay.FindAction("Attack", throwIfNotFound: true);
        m_GamePlay_CannonLook = m_GamePlay.FindAction("CannonLook", throwIfNotFound: true);
        m_GamePlay_Stats = m_GamePlay.FindAction("Stats", throwIfNotFound: true);
        m_GamePlay_NextWeapon = m_GamePlay.FindAction("NextWeapon", throwIfNotFound: true);
        m_GamePlay_PreviousWeapon = m_GamePlay.FindAction("PreviousWeapon", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private IGamePlayActions m_GamePlayActionsCallbackInterface;
    private readonly InputAction m_GamePlay_Movement;
    private readonly InputAction m_GamePlay_Look;
    private readonly InputAction m_GamePlay_CameraLock;
    private readonly InputAction m_GamePlay_Interaction;
    private readonly InputAction m_GamePlay_Interrupt;
    private readonly InputAction m_GamePlay_Pause;
    private readonly InputAction m_GamePlay_Attack;
    private readonly InputAction m_GamePlay_CannonLook;
    private readonly InputAction m_GamePlay_Stats;
    private readonly InputAction m_GamePlay_NextWeapon;
    private readonly InputAction m_GamePlay_PreviousWeapon;
    public struct GamePlayActions
    {
        private @PlayerControls m_Wrapper;
        public GamePlayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_GamePlay_Movement;
        public InputAction @Look => m_Wrapper.m_GamePlay_Look;
        public InputAction @CameraLock => m_Wrapper.m_GamePlay_CameraLock;
        public InputAction @Interaction => m_Wrapper.m_GamePlay_Interaction;
        public InputAction @Interrupt => m_Wrapper.m_GamePlay_Interrupt;
        public InputAction @Pause => m_Wrapper.m_GamePlay_Pause;
        public InputAction @Attack => m_Wrapper.m_GamePlay_Attack;
        public InputAction @CannonLook => m_Wrapper.m_GamePlay_CannonLook;
        public InputAction @Stats => m_Wrapper.m_GamePlay_Stats;
        public InputAction @NextWeapon => m_Wrapper.m_GamePlay_NextWeapon;
        public InputAction @PreviousWeapon => m_Wrapper.m_GamePlay_PreviousWeapon;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void SetCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLook;
                @CameraLock.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnCameraLock;
                @CameraLock.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnCameraLock;
                @CameraLock.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnCameraLock;
                @Interaction.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnInteraction;
                @Interrupt.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnInterrupt;
                @Interrupt.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnInterrupt;
                @Interrupt.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnInterrupt;
                @Pause.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPause;
                @Attack.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAttack;
                @CannonLook.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnCannonLook;
                @CannonLook.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnCannonLook;
                @CannonLook.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnCannonLook;
                @Stats.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnStats;
                @Stats.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnStats;
                @Stats.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnStats;
                @NextWeapon.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnNextWeapon;
                @NextWeapon.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnNextWeapon;
                @NextWeapon.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnNextWeapon;
                @PreviousWeapon.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPreviousWeapon;
                @PreviousWeapon.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPreviousWeapon;
                @PreviousWeapon.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPreviousWeapon;
            }
            m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @CameraLock.started += instance.OnCameraLock;
                @CameraLock.performed += instance.OnCameraLock;
                @CameraLock.canceled += instance.OnCameraLock;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Interrupt.started += instance.OnInterrupt;
                @Interrupt.performed += instance.OnInterrupt;
                @Interrupt.canceled += instance.OnInterrupt;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @CannonLook.started += instance.OnCannonLook;
                @CannonLook.performed += instance.OnCannonLook;
                @CannonLook.canceled += instance.OnCannonLook;
                @Stats.started += instance.OnStats;
                @Stats.performed += instance.OnStats;
                @Stats.canceled += instance.OnStats;
                @NextWeapon.started += instance.OnNextWeapon;
                @NextWeapon.performed += instance.OnNextWeapon;
                @NextWeapon.canceled += instance.OnNextWeapon;
                @PreviousWeapon.started += instance.OnPreviousWeapon;
                @PreviousWeapon.performed += instance.OnPreviousWeapon;
                @PreviousWeapon.canceled += instance.OnPreviousWeapon;
            }
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardAndMouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IGamePlayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnCameraLock(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnInterrupt(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnCannonLook(InputAction.CallbackContext context);
        void OnStats(InputAction.CallbackContext context);
        void OnNextWeapon(InputAction.CallbackContext context);
        void OnPreviousWeapon(InputAction.CallbackContext context);
    }
}
