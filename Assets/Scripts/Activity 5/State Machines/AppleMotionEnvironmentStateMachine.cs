public enum AppleMotionEnvironmentState
{
	None,
	OnBranch,
	Falling,
}

public class AppleMotionEnvironmentStateMachine : EnvironmentStateMachine<AppleMotionEnvironmentState, ActivityFiveEnvironmentManager>
{
	public AppleMotionEnvironmentStateMachine(ActivityFiveEnvironmentManager manager) : base(manager)
	{
	}

	public override void EnterState(AppleMotionEnvironmentState state)
	{
		switch (state)
		{
			case AppleMotionEnvironmentState.None:
				environmentManager.SetPlayerActivityState(true);
				environmentManager.SetAppleOnBranchState(false);
				environmentManager.SetAppleFallingState(false);
				break;
			case AppleMotionEnvironmentState.OnBranch:
				// Implement logic for apple being on the branch
				environmentManager.SetAppleOnBranchState(true);
				break;
			case AppleMotionEnvironmentState.Falling:
				// Implement logic for apple falling
				environmentManager.SetAppleFallingState(true);
				break;
		}
	}

	public override void ExitState(AppleMotionEnvironmentState state)
	{
		switch (state)
		{
			case AppleMotionEnvironmentState.None:
				environmentManager.SetPlayerActivityState(false);
				break;
			case AppleMotionEnvironmentState.OnBranch:
				// Logic for exiting OnBranch state
				environmentManager.SetAppleOnBranchState(false);
				break;
			case AppleMotionEnvironmentState.Falling:
				// Logic for exiting Falling state
				environmentManager.SetAppleFallingState(false);
				break;
		}
	}
}