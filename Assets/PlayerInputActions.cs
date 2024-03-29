// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""World"",
            ""id"": ""b870117b-3520-4725-93ac-67d2901531d3"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""50b75d2b-09c6-4ef2-be1e-f5932adde69b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""3bc0c98c-6344-4316-b44e-ac8bb78705ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f007b524-3daf-41e6-8954-8fb67c4939b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shift"",
                    ""type"": ""Button"",
                    ""id"": ""c90db89e-d25f-4805-bb6b-3704a7d6d959"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Control"",
                    ""type"": ""Button"",
                    ""id"": ""a3eddaaf-cd25-4216-a57d-573bbd081191"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""1e4749b2-249d-4b97-ade7-7a02a920e678"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Use"",
                    ""type"": ""Button"",
                    ""id"": ""0e9bb8f4-d61a-4098-88f9-3f7152c137a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""bba9eb3e-efc0-4805-ba7b-43c2f1724ecd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Explosion"",
                    ""type"": ""Button"",
                    ""id"": ""8a68642a-e7c5-4b1e-a5ef-0b931efdf47a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TestMode"",
                    ""type"": ""Button"",
                    ""id"": ""02b1215b-64f5-485e-aeb1-b22f29782995"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FlyUp"",
                    ""type"": ""Button"",
                    ""id"": ""f3c95352-e4bd-4fd5-8fa3-6d1e82fb5ad8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FlyDown"",
                    ""type"": ""Button"",
                    ""id"": ""af871b30-8b4d-47da-8568-92317faf1cb3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FlyFast"",
                    ""type"": ""Button"",
                    ""id"": ""1c2f512b-0ab9-45f2-bba5-9c68db954554"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c029ff21-7bfa-4f0f-872c-346d6cd31ad1"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d9c5005-b3b7-40b0-b639-f274abaace39"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5efc024-89a1-4ffd-8d0c-f3e5d8b72d2f"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8549590-1695-4d15-a440-1d5b359cc805"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""beeb9dbe-8e26-42b0-8bf6-11e31950ec00"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""Normalize(max=1)"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5a6fdd01-1de5-4cdd-b833-a683fe92b03a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""540391fc-bfe4-4789-9bcb-1a4aa1b8fc69"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a8c5a413-9b10-4288-92e6-b4e86acdb4f5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9e862c64-356d-4498-801d-16a32b0ae3f6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""51f1e8c3-1f25-4b20-b72b-a57b44ce0526"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""43603909-322a-465c-99b5-be02021fe7cf"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""down"",
                    ""id"": ""736e8172-9d5c-4016-aeb8-806011f6f274"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""95e6092e-5b8d-4e0d-ad93-3d082e0d561e"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cf030799-2bee-45c4-9611-0682413fd45f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d52c026-4b4d-4fb7-9cfe-5f9006f18bc5"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Explosion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3abfdd3b-c550-43a5-99f6-12be7fb8aab1"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""TestMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98f890df-4033-40c3-8a8d-a7f19b904d48"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""FlyUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0f07c6b-7b34-4d98-8c8a-756b1dd79e27"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""FlyDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c23304a-c34b-4877-a39d-cc42c448dd7a"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""FlyFast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Interface"",
            ""id"": ""9c496cb0-7b27-49de-9736-f1874021558b"",
            ""actions"": [
                {
                    ""name"": ""PauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""b248048c-2118-4e2c-87e0-acd840be8a3b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""18f480d8-75a3-4838-9de6-f43508210531"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Default"",
            ""bindingGroup"": ""Default"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // World
        m_World = asset.FindActionMap("World", throwIfNotFound: true);
        m_World_Look = m_World.FindAction("Look", throwIfNotFound: true);
        m_World_Move = m_World.FindAction("Move", throwIfNotFound: true);
        m_World_Jump = m_World.FindAction("Jump", throwIfNotFound: true);
        m_World_Shift = m_World.FindAction("Shift", throwIfNotFound: true);
        m_World_Control = m_World.FindAction("Control", throwIfNotFound: true);
        m_World_Interact = m_World.FindAction("Interact", throwIfNotFound: true);
        m_World_Use = m_World.FindAction("Use", throwIfNotFound: true);
        m_World_Pause = m_World.FindAction("Pause", throwIfNotFound: true);
        m_World_Explosion = m_World.FindAction("Explosion", throwIfNotFound: true);
        m_World_TestMode = m_World.FindAction("TestMode", throwIfNotFound: true);
        m_World_FlyUp = m_World.FindAction("FlyUp", throwIfNotFound: true);
        m_World_FlyDown = m_World.FindAction("FlyDown", throwIfNotFound: true);
        m_World_FlyFast = m_World.FindAction("FlyFast", throwIfNotFound: true);
        // Interface
        m_Interface = asset.FindActionMap("Interface", throwIfNotFound: true);
        m_Interface_PauseMenu = m_Interface.FindAction("PauseMenu", throwIfNotFound: true);
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

    // World
    private readonly InputActionMap m_World;
    private IWorldActions m_WorldActionsCallbackInterface;
    private readonly InputAction m_World_Look;
    private readonly InputAction m_World_Move;
    private readonly InputAction m_World_Jump;
    private readonly InputAction m_World_Shift;
    private readonly InputAction m_World_Control;
    private readonly InputAction m_World_Interact;
    private readonly InputAction m_World_Use;
    private readonly InputAction m_World_Pause;
    private readonly InputAction m_World_Explosion;
    private readonly InputAction m_World_TestMode;
    private readonly InputAction m_World_FlyUp;
    private readonly InputAction m_World_FlyDown;
    private readonly InputAction m_World_FlyFast;
    public struct WorldActions
    {
        private @PlayerInputActions m_Wrapper;
        public WorldActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_World_Look;
        public InputAction @Move => m_Wrapper.m_World_Move;
        public InputAction @Jump => m_Wrapper.m_World_Jump;
        public InputAction @Shift => m_Wrapper.m_World_Shift;
        public InputAction @Control => m_Wrapper.m_World_Control;
        public InputAction @Interact => m_Wrapper.m_World_Interact;
        public InputAction @Use => m_Wrapper.m_World_Use;
        public InputAction @Pause => m_Wrapper.m_World_Pause;
        public InputAction @Explosion => m_Wrapper.m_World_Explosion;
        public InputAction @TestMode => m_Wrapper.m_World_TestMode;
        public InputAction @FlyUp => m_Wrapper.m_World_FlyUp;
        public InputAction @FlyDown => m_Wrapper.m_World_FlyDown;
        public InputAction @FlyFast => m_Wrapper.m_World_FlyFast;
        public InputActionMap Get() { return m_Wrapper.m_World; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WorldActions set) { return set.Get(); }
        public void SetCallbacks(IWorldActions instance)
        {
            if (m_Wrapper.m_WorldActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnLook;
                @Move.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnJump;
                @Shift.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnShift;
                @Shift.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnShift;
                @Shift.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnShift;
                @Control.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnControl;
                @Control.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnControl;
                @Control.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnControl;
                @Interact.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnInteract;
                @Use.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnUse;
                @Use.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnUse;
                @Use.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnUse;
                @Pause.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnPause;
                @Explosion.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnExplosion;
                @Explosion.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnExplosion;
                @Explosion.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnExplosion;
                @TestMode.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnTestMode;
                @TestMode.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnTestMode;
                @TestMode.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnTestMode;
                @FlyUp.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyUp;
                @FlyUp.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyUp;
                @FlyUp.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyUp;
                @FlyDown.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyDown;
                @FlyDown.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyDown;
                @FlyDown.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyDown;
                @FlyFast.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyFast;
                @FlyFast.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyFast;
                @FlyFast.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnFlyFast;
            }
            m_Wrapper.m_WorldActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Shift.started += instance.OnShift;
                @Shift.performed += instance.OnShift;
                @Shift.canceled += instance.OnShift;
                @Control.started += instance.OnControl;
                @Control.performed += instance.OnControl;
                @Control.canceled += instance.OnControl;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Use.started += instance.OnUse;
                @Use.performed += instance.OnUse;
                @Use.canceled += instance.OnUse;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Explosion.started += instance.OnExplosion;
                @Explosion.performed += instance.OnExplosion;
                @Explosion.canceled += instance.OnExplosion;
                @TestMode.started += instance.OnTestMode;
                @TestMode.performed += instance.OnTestMode;
                @TestMode.canceled += instance.OnTestMode;
                @FlyUp.started += instance.OnFlyUp;
                @FlyUp.performed += instance.OnFlyUp;
                @FlyUp.canceled += instance.OnFlyUp;
                @FlyDown.started += instance.OnFlyDown;
                @FlyDown.performed += instance.OnFlyDown;
                @FlyDown.canceled += instance.OnFlyDown;
                @FlyFast.started += instance.OnFlyFast;
                @FlyFast.performed += instance.OnFlyFast;
                @FlyFast.canceled += instance.OnFlyFast;
            }
        }
    }
    public WorldActions @World => new WorldActions(this);

    // Interface
    private readonly InputActionMap m_Interface;
    private IInterfaceActions m_InterfaceActionsCallbackInterface;
    private readonly InputAction m_Interface_PauseMenu;
    public struct InterfaceActions
    {
        private @PlayerInputActions m_Wrapper;
        public InterfaceActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PauseMenu => m_Wrapper.m_Interface_PauseMenu;
        public InputActionMap Get() { return m_Wrapper.m_Interface; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InterfaceActions set) { return set.Get(); }
        public void SetCallbacks(IInterfaceActions instance)
        {
            if (m_Wrapper.m_InterfaceActionsCallbackInterface != null)
            {
                @PauseMenu.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPauseMenu;
                @PauseMenu.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPauseMenu;
                @PauseMenu.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPauseMenu;
            }
            m_Wrapper.m_InterfaceActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PauseMenu.started += instance.OnPauseMenu;
                @PauseMenu.performed += instance.OnPauseMenu;
                @PauseMenu.canceled += instance.OnPauseMenu;
            }
        }
    }
    public InterfaceActions @Interface => new InterfaceActions(this);
    private int m_DefaultSchemeIndex = -1;
    public InputControlScheme DefaultScheme
    {
        get
        {
            if (m_DefaultSchemeIndex == -1) m_DefaultSchemeIndex = asset.FindControlSchemeIndex("Default");
            return asset.controlSchemes[m_DefaultSchemeIndex];
        }
    }
    public interface IWorldActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnShift(InputAction.CallbackContext context);
        void OnControl(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnUse(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnExplosion(InputAction.CallbackContext context);
        void OnTestMode(InputAction.CallbackContext context);
        void OnFlyUp(InputAction.CallbackContext context);
        void OnFlyDown(InputAction.CallbackContext context);
        void OnFlyFast(InputAction.CallbackContext context);
    }
    public interface IInterfaceActions
    {
        void OnPauseMenu(InputAction.CallbackContext context);
    }
}
