using UnityEngine;

public class ActivityFourManager : MonoBehaviour
{
    [Header("Level Data - Projectile Motion")]
    [SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelOne;
	[SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelTwo;
	[SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelThree;
	private ProjectileMotionSubActivitySO currentProjectileMotionLevel;

	[Header("Views")]
	[SerializeField] private ViewProjectileMotion viewProjectileMotion;

	[Header("Given Values - Projectile Motion")]
	private int initialProjectileVelocityValue;
	private int projectileHeightValue;
	private int projectileAngleValue;
	
	private void Start()
	{
		ViewProjectileMotion.SubmitMaximumHeightAnswerEvent += CheckMaximumHeightAnswer;

		currentProjectileMotionLevel = projectileMotionLevelOne; // modify this in the future, to add change of level

		InitializeProjectileMotionGiven(currentProjectileMotionLevel);

		viewProjectileMotion.SetupProjectileMotionProblemDisplay(initialProjectileVelocityValue, projectileHeightValue, projectileAngleValue);
	}

	#region Projectile Motion

	private void InitializeProjectileMotionGiven(ProjectileMotionSubActivitySO projectileMotionSO)
	{
		initialProjectileVelocityValue = Random.Range(projectileMotionSO.minimumVelocityValue, projectileMotionSO.maximumVelocityValue);
		projectileHeightValue = Random.Range(projectileMotionSO.minimumHeightValue, projectileMotionSO.maximumHeightValue);

		switch (projectileMotionSO.projectileAngleType)
		{
			case ProjectileAngleType.Standard90Angle:
				int[] standard90AngleValues = new int[] { 30, 45, 60, 90};
				projectileAngleValue = standard90AngleValues[Random.Range(0, standard90AngleValues.Length)];
				break;
			case ProjectileAngleType.Full90Angle:
				projectileAngleValue = Random.Range(1, 90);
				break;
		}
	}

	private void CheckMaximumHeightAnswer(float maximumHeightAnswer)
	{
		bool isMaximumHeightCorrect = ActivityFourUtilities.ValidateMaximumHeightSubmission(maximumHeightAnswer, initialProjectileVelocityValue, projectileAngleValue);

	}

	#endregion
}