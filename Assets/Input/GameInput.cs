//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/GameInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""770abc1c-30ab-4c39-ab01-c0ce1a0fbe96"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""cee7f681-c025-494f-8774-d968bef78b31"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""804f5231-04b4-4033-ab1b-f2c75d886da7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f9b0a4b8-3217-4db5-b791-52c34b7cb9b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""f58c2670-191e-4de6-a5aa-08e99c559bb7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PauseGameplay"",
                    ""type"": ""Button"",
                    ""id"": ""edf38e1b-90fb-468c-b5a0-d1cbf0d8351e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""7bcfba86-7ac2-44c5-87b4-185d4df27dcc"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3bd12e5a-4fb6-41c2-ad90-fbb07addbd84"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3880a21e-2567-4b7a-a6c6-45758f2cc1dd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b8bfa489-fccd-43c7-8c4f-431eebdfcc76"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fd1037d5-2e97-4597-8543-0da92a9865d5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""50129076-5fb0-40bd-89cc-1fcf3a0b6e56"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b31cc18f-b643-43ea-9a57-bb41b1a7b711"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5b3a8da0-a1d0-470c-a9ff-d5b0b4c84d6d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2f275375-0146-4177-90e7-bd7400bd4998"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ae76a44a-f0ee-4eb5-8455-1c5ee1678604"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4a006b52-f0a1-4fdf-864d-794d7557befb"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6772e47d-078f-4607-9b4e-087d90d5b16b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2fefe08-1a24-4301-8596-a941db6f4683"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ac4e3ed-d71c-4fa1-a154-c5df64c23f8d"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGameplay"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GameplayPauseMenu"",
            ""id"": ""3a48c27f-edcc-4b7f-ba81-92f9899f30f3"",
            ""actions"": [
                {
                    ""name"": ""ResumeGame"",
                    ""type"": ""Button"",
                    ""id"": ""cb65c0d5-ea88-48ef-bf55-c46e80414c5a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PauseMenuNavigation"",
                    ""type"": ""Value"",
                    ""id"": ""7000f78b-860a-446a-a499-5f75a98533c0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PauseMenuSelectChoice"",
                    ""type"": ""Button"",
                    ""id"": ""b08f5ee2-19d2-4d0b-b1be-8e9b8686dfa0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2da83663-00f8-4db7-9546-0b95f8e19d65"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResumeGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""d2e0a7ff-8c7d-4d22-abeb-fea7b90b5f9d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenuNavigation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""14d07181-393b-42b5-9954-41ccef174496"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenuNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b11c360f-042a-4642-bbaf-eb992536356c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenuNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ac908498-72f2-4f7c-9398-fa5a29d953f4"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenuNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2308f91d-1d50-4087-9958-261039d22a84"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenuNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""42a8a641-1fa6-480f-ac72-a6da05470190"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenuSelectChoice"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GameplayUI"",
            ""id"": ""dc83973f-4524-4fe8-8124-c2a2879aee24"",
            ""actions"": [
                {
                    ""name"": ""PauseGameplayUI"",
                    ""type"": ""Button"",
                    ""id"": ""fb80e025-b0ba-4317-a5ad-3c2df7002535"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bf9b8493-ba61-4c9b-b284-a667e181b837"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGameplayUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
        m_Gameplay_PauseGameplay = m_Gameplay.FindAction("PauseGameplay", throwIfNotFound: true);
        // GameplayPauseMenu
        m_GameplayPauseMenu = asset.FindActionMap("GameplayPauseMenu", throwIfNotFound: true);
        m_GameplayPauseMenu_ResumeGame = m_GameplayPauseMenu.FindAction("ResumeGame", throwIfNotFound: true);
        m_GameplayPauseMenu_PauseMenuNavigation = m_GameplayPauseMenu.FindAction("PauseMenuNavigation", throwIfNotFound: true);
        m_GameplayPauseMenu_PauseMenuSelectChoice = m_GameplayPauseMenu.FindAction("PauseMenuSelectChoice", throwIfNotFound: true);
        // GameplayUI
        m_GameplayUI = asset.FindActionMap("GameplayUI", throwIfNotFound: true);
        m_GameplayUI_PauseGameplayUI = m_GameplayUI.FindAction("PauseGameplayUI", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Look;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Interact;
    private readonly InputAction m_Gameplay_PauseGameplay;
    public struct GameplayActions
    {
        private @GameInput m_Wrapper;
        public GameplayActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Look => m_Wrapper.m_Gameplay_Look;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputAction @PauseGameplay => m_Wrapper.m_Gameplay_PauseGameplay;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @PauseGameplay.started += instance.OnPauseGameplay;
            @PauseGameplay.performed += instance.OnPauseGameplay;
            @PauseGameplay.canceled += instance.OnPauseGameplay;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @PauseGameplay.started -= instance.OnPauseGameplay;
            @PauseGameplay.performed -= instance.OnPauseGameplay;
            @PauseGameplay.canceled -= instance.OnPauseGameplay;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // GameplayPauseMenu
    private readonly InputActionMap m_GameplayPauseMenu;
    private List<IGameplayPauseMenuActions> m_GameplayPauseMenuActionsCallbackInterfaces = new List<IGameplayPauseMenuActions>();
    private readonly InputAction m_GameplayPauseMenu_ResumeGame;
    private readonly InputAction m_GameplayPauseMenu_PauseMenuNavigation;
    private readonly InputAction m_GameplayPauseMenu_PauseMenuSelectChoice;
    public struct GameplayPauseMenuActions
    {
        private @GameInput m_Wrapper;
        public GameplayPauseMenuActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @ResumeGame => m_Wrapper.m_GameplayPauseMenu_ResumeGame;
        public InputAction @PauseMenuNavigation => m_Wrapper.m_GameplayPauseMenu_PauseMenuNavigation;
        public InputAction @PauseMenuSelectChoice => m_Wrapper.m_GameplayPauseMenu_PauseMenuSelectChoice;
        public InputActionMap Get() { return m_Wrapper.m_GameplayPauseMenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayPauseMenuActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayPauseMenuActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayPauseMenuActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayPauseMenuActionsCallbackInterfaces.Add(instance);
            @ResumeGame.started += instance.OnResumeGame;
            @ResumeGame.performed += instance.OnResumeGame;
            @ResumeGame.canceled += instance.OnResumeGame;
            @PauseMenuNavigation.started += instance.OnPauseMenuNavigation;
            @PauseMenuNavigation.performed += instance.OnPauseMenuNavigation;
            @PauseMenuNavigation.canceled += instance.OnPauseMenuNavigation;
            @PauseMenuSelectChoice.started += instance.OnPauseMenuSelectChoice;
            @PauseMenuSelectChoice.performed += instance.OnPauseMenuSelectChoice;
            @PauseMenuSelectChoice.canceled += instance.OnPauseMenuSelectChoice;
        }

        private void UnregisterCallbacks(IGameplayPauseMenuActions instance)
        {
            @ResumeGame.started -= instance.OnResumeGame;
            @ResumeGame.performed -= instance.OnResumeGame;
            @ResumeGame.canceled -= instance.OnResumeGame;
            @PauseMenuNavigation.started -= instance.OnPauseMenuNavigation;
            @PauseMenuNavigation.performed -= instance.OnPauseMenuNavigation;
            @PauseMenuNavigation.canceled -= instance.OnPauseMenuNavigation;
            @PauseMenuSelectChoice.started -= instance.OnPauseMenuSelectChoice;
            @PauseMenuSelectChoice.performed -= instance.OnPauseMenuSelectChoice;
            @PauseMenuSelectChoice.canceled -= instance.OnPauseMenuSelectChoice;
        }

        public void RemoveCallbacks(IGameplayPauseMenuActions instance)
        {
            if (m_Wrapper.m_GameplayPauseMenuActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayPauseMenuActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayPauseMenuActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayPauseMenuActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayPauseMenuActions @GameplayPauseMenu => new GameplayPauseMenuActions(this);

    // GameplayUI
    private readonly InputActionMap m_GameplayUI;
    private List<IGameplayUIActions> m_GameplayUIActionsCallbackInterfaces = new List<IGameplayUIActions>();
    private readonly InputAction m_GameplayUI_PauseGameplayUI;
    public struct GameplayUIActions
    {
        private @GameInput m_Wrapper;
        public GameplayUIActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PauseGameplayUI => m_Wrapper.m_GameplayUI_PauseGameplayUI;
        public InputActionMap Get() { return m_Wrapper.m_GameplayUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayUIActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayUIActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayUIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayUIActionsCallbackInterfaces.Add(instance);
            @PauseGameplayUI.started += instance.OnPauseGameplayUI;
            @PauseGameplayUI.performed += instance.OnPauseGameplayUI;
            @PauseGameplayUI.canceled += instance.OnPauseGameplayUI;
        }

        private void UnregisterCallbacks(IGameplayUIActions instance)
        {
            @PauseGameplayUI.started -= instance.OnPauseGameplayUI;
            @PauseGameplayUI.performed -= instance.OnPauseGameplayUI;
            @PauseGameplayUI.canceled -= instance.OnPauseGameplayUI;
        }

        public void RemoveCallbacks(IGameplayUIActions instance)
        {
            if (m_Wrapper.m_GameplayUIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayUIActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayUIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayUIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayUIActions @GameplayUI => new GameplayUIActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnPauseGameplay(InputAction.CallbackContext context);
    }
    public interface IGameplayPauseMenuActions
    {
        void OnResumeGame(InputAction.CallbackContext context);
        void OnPauseMenuNavigation(InputAction.CallbackContext context);
        void OnPauseMenuSelectChoice(InputAction.CallbackContext context);
    }
    public interface IGameplayUIActions
    {
        void OnPauseGameplayUI(InputAction.CallbackContext context);
    }
}
