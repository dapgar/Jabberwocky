//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/PlayerInput/Player Controls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Controls"",
    ""maps"": [
        {
            ""name"": ""Board"",
            ""id"": ""5882b727-e438-467f-8993-dbfcf1888c73"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Value"",
                    ""id"": ""77f05210-71d8-4d9a-8663-96a74805c9ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e67af31c-55f5-46d6-9b79-1e53b9df67d0"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Minigame"",
            ""id"": ""20645a21-6654-473c-be3d-36db525d5976"",
            ""actions"": [
                {
                    ""name"": ""Button"",
                    ""type"": ""Value"",
                    ""id"": ""c092131a-04d5-45aa-b5af-07c9206315cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""04cccf47-dc66-48f6-9e5e-28fda8381a51"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bbcd672d-e043-411d-a2d8-f21b72bcea53"",
                    ""path"": ""<HID::USB Gamepad >/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0944d471-a470-46fa-8d84-7ec3ea376b0c"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""DPAD"",
                    ""id"": ""58d37fd3-8917-449c-8ad1-50540b6225b2"",
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
                    ""id"": ""a5e3c36c-02bd-411a-99dd-21c426f06d06"",
                    ""path"": ""<HID::USB Gamepad >/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""090d00cc-a791-48f2-ad40-9bf3ce9e3ae3"",
                    ""path"": ""<HID::USB Gamepad >/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""892f7f53-a999-444d-ad87-2499177aae0e"",
                    ""path"": ""<HID::USB Gamepad >/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""769f22ed-e228-4840-bad7-cd35d5d0300a"",
                    ""path"": ""<HID::USB Gamepad >/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""af85c50d-3698-49a8-b257-7f79a728f1d1"",
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
                    ""id"": ""0e5c1c9a-108c-4927-b6df-85770a6d3b27"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e2b2c8bb-ef52-45e0-8819-eaceb4732e33"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d46d174d-4612-4d89-9cca-b3b313fc6410"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9f08bddd-0d2f-4be3-94fc-db52e25ae617"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": [
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
                    ""devicePath"": ""<HID::USB Gamepad >"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Board
        m_Board = asset.FindActionMap("Board", throwIfNotFound: true);
        m_Board_Newaction = m_Board.FindAction("New action", throwIfNotFound: true);
        // Minigame
        m_Minigame = asset.FindActionMap("Minigame", throwIfNotFound: true);
        m_Minigame_Button = m_Minigame.FindAction("Button", throwIfNotFound: true);
        m_Minigame_Move = m_Minigame.FindAction("Move", throwIfNotFound: true);
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

    // Board
    private readonly InputActionMap m_Board;
    private List<IBoardActions> m_BoardActionsCallbackInterfaces = new List<IBoardActions>();
    private readonly InputAction m_Board_Newaction;
    public struct BoardActions
    {
        private @PlayerControls m_Wrapper;
        public BoardActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Board_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Board; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BoardActions set) { return set.Get(); }
        public void AddCallbacks(IBoardActions instance)
        {
            if (instance == null || m_Wrapper.m_BoardActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BoardActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        private void UnregisterCallbacks(IBoardActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        public void RemoveCallbacks(IBoardActions instance)
        {
            if (m_Wrapper.m_BoardActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBoardActions instance)
        {
            foreach (var item in m_Wrapper.m_BoardActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BoardActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BoardActions @Board => new BoardActions(this);

    // Minigame
    private readonly InputActionMap m_Minigame;
    private List<IMinigameActions> m_MinigameActionsCallbackInterfaces = new List<IMinigameActions>();
    private readonly InputAction m_Minigame_Button;
    private readonly InputAction m_Minigame_Move;
    public struct MinigameActions
    {
        private @PlayerControls m_Wrapper;
        public MinigameActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Button => m_Wrapper.m_Minigame_Button;
        public InputAction @Move => m_Wrapper.m_Minigame_Move;
        public InputActionMap Get() { return m_Wrapper.m_Minigame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MinigameActions set) { return set.Get(); }
        public void AddCallbacks(IMinigameActions instance)
        {
            if (instance == null || m_Wrapper.m_MinigameActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MinigameActionsCallbackInterfaces.Add(instance);
            @Button.started += instance.OnButton;
            @Button.performed += instance.OnButton;
            @Button.canceled += instance.OnButton;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
        }

        private void UnregisterCallbacks(IMinigameActions instance)
        {
            @Button.started -= instance.OnButton;
            @Button.performed -= instance.OnButton;
            @Button.canceled -= instance.OnButton;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
        }

        public void RemoveCallbacks(IMinigameActions instance)
        {
            if (m_Wrapper.m_MinigameActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMinigameActions instance)
        {
            foreach (var item in m_Wrapper.m_MinigameActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MinigameActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MinigameActions @Minigame => new MinigameActions(this);
    private int m_NewcontrolschemeSchemeIndex = -1;
    public InputControlScheme NewcontrolschemeScheme
    {
        get
        {
            if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
            return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
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
    public interface IBoardActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface IMinigameActions
    {
        void OnButton(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
    }
}