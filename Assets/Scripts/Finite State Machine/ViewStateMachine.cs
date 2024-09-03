using System;
using UnityEngine;

public abstract class ViewStateMachine<TState, TView> : BaseFiniteStateMachine<TState>
	where TState : Enum
	where TView : Component
{
	protected TView view;

	public ViewStateMachine(TView view)
	{
		this.view = view;
	}
}