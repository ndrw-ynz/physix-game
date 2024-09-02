using System;

public abstract class BaseFiniteStateMachine<TState> where TState : Enum
{
	public TState currentState { get; private set; }

	public void Initialize(TState initialState)
	{
		currentState = initialState;
		EnterState(initialState);
	}

	public void TransitionToState(TState newState)
	{
		ExitState(currentState);
		currentState = newState;
		EnterState(newState);
	}

	public abstract void EnterState(TState state);
	public abstract void ExitState(TState state);
}