public enum BoatMotionEnvironmentState
{
	None,
	Stationary,
	MovingRight,
	MovingLeft,
}

public class BoatMotionEnvironmentStateMachine : EnvironmentStateMachine<BoatMotionEnvironmentState, ActivityFiveEnvironmentManager>
{
	public BoatMotionEnvironmentStateMachine(ActivityFiveEnvironmentManager manager) : base(manager)
	{
	}

	public override void EnterState(BoatMotionEnvironmentState state)
	{
		switch (state)
		{
			case BoatMotionEnvironmentState.None:
				environmentManager.SetPlayerActivityState(true);
				environmentManager.SetBoatStationaryState(false);
				environmentManager.SetBoatMovingRightState(false);
				environmentManager.SetBoatMovingLeftState(false);
				break;
			case BoatMotionEnvironmentState.Stationary:
				environmentManager.SetBoatStationaryState(true);
				break;
			case BoatMotionEnvironmentState.MovingRight:
				environmentManager.SetBoatMovingRightState(true);
				break;
			case BoatMotionEnvironmentState.MovingLeft:
				environmentManager.SetBoatMovingLeftState(true);
				break;
		}
	}

	public override void ExitState(BoatMotionEnvironmentState state)
	{
		switch (state)
		{
			case BoatMotionEnvironmentState.None:
				environmentManager.SetPlayerActivityState(false);
				break;
			case BoatMotionEnvironmentState.Stationary:
				environmentManager.SetBoatStationaryState(false);
				break;
			case BoatMotionEnvironmentState.MovingRight:
				environmentManager.SetBoatMovingRightState(false);
				break;
			case BoatMotionEnvironmentState.MovingLeft:
				environmentManager.SetBoatMovingLeftState(false);
				break;
		}
	}
}