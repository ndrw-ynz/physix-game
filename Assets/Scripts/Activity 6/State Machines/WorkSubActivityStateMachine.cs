public enum WorkSubActivityState{
	LinearWork,
	AngularWork,
	None
}

public class WorkSubActivityStateMachine : ViewStateMachine<WorkSubActivityState, WorkView>
{
	public WorkSubActivityStateMachine(WorkView view) : base(view)
	{
	}

	public override void EnterState(WorkSubActivityState state)
	{
		switch (state)
		{
			case WorkSubActivityState.LinearWork:
				view.SetLinearEquationDisplayState(true);
				break;
			case WorkSubActivityState.AngularWork:
				view.SetAngularEquationDisplayState(true);
				break;
			case WorkSubActivityState.None:
				view.gameObject.SetActive(false);
				view.SetLinearEquationDisplayState(false);
				view.SetAngularEquationDisplayState(false);
				break;
		}
	}

	public override void ExitState(WorkSubActivityState state)
	{
		switch (state)
		{
			case WorkSubActivityState.LinearWork:
				view.SetLinearEquationDisplayState(false);
				break;
			case WorkSubActivityState.AngularWork:
				view.SetAngularEquationDisplayState(false);
				break;
			case WorkSubActivityState.None:
				view.gameObject.SetActive(true);
				break;
		}
	}
}