public class AppleMotionViewStateMachine : ViewStateMachine<ActivityFiveSubActivityState, AppleMotionView>
{
	public AppleMotionViewStateMachine(AppleMotionView view) : base(view)
	{
	}

	public override void EnterState(ActivityFiveSubActivityState state)
	{
		switch (state)
		{
			case ActivityFiveSubActivityState.SolveForceDiagram:
				view.SetForceDiagramDisplayState(true);
				break;
			case ActivityFiveSubActivityState.SolveForceCalculation:
				view.SetForceCalculationDisplayState(true);
				break;
			case ActivityFiveSubActivityState.None:
				view.gameObject.SetActive(true);
				view.SetForceDiagramDisplayState(false);
				view.SetForceCalculationDisplayState(false);
				break;
		}
	}

	public override void ExitState(ActivityFiveSubActivityState state)
	{
		switch (state)
		{
			case ActivityFiveSubActivityState.SolveForceDiagram:
				view.SetForceDiagramDisplayState(false);
				break;
			case ActivityFiveSubActivityState.SolveForceCalculation:
				view.SetForceCalculationDisplayState(false);
				break;
			case ActivityFiveSubActivityState.None:
				view.gameObject.SetActive(false);
				break;
		}
	}
}