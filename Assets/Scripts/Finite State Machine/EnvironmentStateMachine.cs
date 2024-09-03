using System;

public abstract class EnvironmentStateMachine<TState, TManager> : BaseFiniteStateMachine<TState>
	where TState : Enum
	where TManager : ActivityEnvironmentManager
{
	protected TManager environmentManager;

	public EnvironmentStateMachine(TManager manager)
	{
		environmentManager = manager;
	}
}