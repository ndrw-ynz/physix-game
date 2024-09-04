public enum RockMotionEnvironmentState
{
	None,
	Stationary,
	Rolling,
	Bouncing,
	Flying
}

public class RockMotionEnvironmentStateMachine : EnvironmentStateMachine<RockMotionEnvironmentState, ActivityFiveEnvironmentManager>
{
	public RockMotionEnvironmentStateMachine(ActivityFiveEnvironmentManager manager) : base(manager)
	{
	}

	public override void EnterState(RockMotionEnvironmentState state)
	{
		switch (state)
		{
			case RockMotionEnvironmentState.None:
				environmentManager.SetPlayerActivityState(true);
				environmentManager.SetRockStationaryState(false);
				environmentManager.SetRockRollingState(false);
				environmentManager.SetRockBouncingState(false);
				environmentManager.SetRockFlyingState(false);
				break;
			case RockMotionEnvironmentState.Stationary:
				environmentManager.SetRockStationaryState(true);
				break;
			case RockMotionEnvironmentState.Rolling:
				environmentManager.SetRockRollingState(true);
				break;
			case RockMotionEnvironmentState.Bouncing:
				environmentManager.SetRockBouncingState(true);
				break;
			case RockMotionEnvironmentState.Flying:
				environmentManager.SetRockFlyingState(true);
				break;
		}
	}

	public override void ExitState(RockMotionEnvironmentState state)
	{
		switch (state)
		{
			case RockMotionEnvironmentState.None:
				environmentManager.SetPlayerActivityState(false);
				break;
			case RockMotionEnvironmentState.Stationary:
				environmentManager.SetRockStationaryState(false);
				break;
			case RockMotionEnvironmentState.Rolling:
				environmentManager.SetRockRollingState(false);
				break;
			case RockMotionEnvironmentState.Bouncing:
				environmentManager.SetRockBouncingState(false);
				break;
			case RockMotionEnvironmentState.Flying:
				environmentManager.SetRockFlyingState(false);
				break;
		}
	}
}