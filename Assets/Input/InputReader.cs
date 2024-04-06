using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The InputReader class is where all input events are delegated.
/// </summary>
[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    private GameInput _gameInput;

    private void OnEnable()
    {
        // Enable object
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            // Mapping events here to GameInput, wherein input events are triggered with events defined on InputReader.
            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);

            // Have gameplay as the first mapping to be enabled
            SetGameplay();
        }
    }

    /// <summary>
    /// Enables gameplay as current state.
    /// </summary>
    public void SetGameplay()
    {
        _gameInput.Gameplay.Enable();
        _gameInput.UI.Disable();
    }

    /// <summary>
    /// Enables UI as current state.
    /// </summary>
    public void SetUI()
    {
        _gameInput.UI.Enable();
        _gameInput.Gameplay.Disable();
    }

    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action JumpEvent;
    public event Action InteractEvent;
    public event Action PauseEvent;
    public event Action ResumeEvent;

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }
}
