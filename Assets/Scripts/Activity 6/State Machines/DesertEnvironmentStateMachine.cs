public enum DesertEnvironmentState
{
    StraightPath,
    CurvedPath,
    None
}

public class DesertEnvironmentStateMachine : EnvironmentStateMachine<DesertEnvironmentState, ActivitySixEnvironmentManager>
{
	public DesertEnvironmentStateMachine(ActivitySixEnvironmentManager manager) : base(manager)
	{
	}
	
	public override void EnterState(DesertEnvironmentState state)
	{
		switch (state)
		{
			case DesertEnvironmentState.None:
				environmentManager.SetPlayerActivityState(true);
				environmentManager.SetDesertStraightPathEnvironmentState(false);
				environmentManager.SetDesertCurvedPathEnvironmentState(false);
				break;
			case DesertEnvironmentState.StraightPath:
				environmentManager.SetDesertStraightPathEnvironmentState(true);
				break;
			case DesertEnvironmentState.CurvedPath:
				environmentManager.SetDesertCurvedPathEnvironmentState(true);
				break;
		}
	}

	public override void ExitState(DesertEnvironmentState state)
	{
		switch (state)
		{
			case DesertEnvironmentState.None:
				environmentManager.SetPlayerActivityState(false);
				break;
			case DesertEnvironmentState.StraightPath:
				environmentManager.SetDesertStraightPathEnvironmentState(false);
				break;
			case DesertEnvironmentState.CurvedPath:
				environmentManager.SetDesertCurvedPathEnvironmentState(false);
				break;
		}
	}
}
