using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The InputReader class is where all input events are delegated.
/// </summary>
[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IGameplayPauseMenuActions, GameInput.IGameplayUIActions
{
    public enum GameplayState
    {
        Gameplay,
        UI
    }

	private GameInput _gameInput;
    private GameplayState _previousGameplayState;

    private void OnEnable()
    {
        // Enable object
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            // Mapping events here to GameInput, wherein input events are triggered with events defined on InputReader.
            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.GameplayPauseMenu.SetCallbacks(this);
            _gameInput.GameplayUI.SetCallbacks(this);

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
		_gameInput.GameplayUI.Disable();
		_gameInput.GameplayPauseMenu.Disable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Enables gameplay pause menu as current state.
    /// </summary>
    public void SetGameplayPauseMenu()
    {
        _gameInput.GameplayPauseMenu.Enable();
		_gameInput.Gameplay.Disable();
		_gameInput.GameplayUI.Disable();
		Cursor.lockState = CursorLockMode.None;
	}

	/// <summary>
	/// Enables UI as current state.
	/// </summary>
	public void SetUI()
    {
        _gameInput.GameplayUI.Enable();
		_gameInput.Gameplay.Disable();
		_gameInput.GameplayPauseMenu.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    // Gameplay action map
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action JumpEvent;
    public event Action InteractEvent;
	public event Action PauseGameplayEvent;
    // GameplayPauseMenu action map
	public event Action ResumeGameplayEvent;
    public event Action<Vector2> PauseMenuNavigationEvent;
    // GameplayUI action map
    public event Action PauseGameplayUIEvent;

    // Gameplay functions
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }

    public void OnPauseGameplay(InputAction.CallbackContext context)
    {
        _previousGameplayState = GameplayState.Gameplay;
		PauseGameplayEvent?.Invoke();
		SetGameplayPauseMenu(); 
    }

	// GameplayPauseMenu functions
	public void OnResumeGame(InputAction.CallbackContext context)
	{
        SetGameplayPreviousState();
		ResumeGameplayEvent?.Invoke();
	}

	public void SetGameplayPreviousState()
	{
		if (_previousGameplayState == GameplayState.Gameplay)
		{
			SetGameplay();
		}
		else if (_previousGameplayState == GameplayState.UI)
		{
			SetUI();
		}
	}

	public void OnPauseMenuNavigation(InputAction.CallbackContext context)
	{
        if (context.performed) PauseMenuNavigationEvent?.Invoke(context.ReadValue<Vector2>());
	}

    // GameplayUI functions
	public void OnPauseGameplayUI(InputAction.CallbackContext context)
	{
        _previousGameplayState = GameplayState.UI;
        PauseGameplayUIEvent?.Invoke();
		SetGameplayPauseMenu();
	}
}