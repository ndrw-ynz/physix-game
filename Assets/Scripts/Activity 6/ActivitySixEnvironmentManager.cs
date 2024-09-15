using System.Collections.Generic;
using UnityEngine;

public class ActivitySixEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivitySixManager activitySixManager;

    [Header("Views")]
    [SerializeField] private DotProductView dotProductView;
	[SerializeField] private WorkView workView;
	[SerializeField] private WorkGraphInterpretationView workGraphInterpretationView;

	[Header("Submission Status Displays")]
	[Header("Work Submission Status Displays")]
	[SerializeField] private WorkSubmissionStatusDisplay workSubmissionStatusDisplay;

	[Header("Environment Cameras")]
	[SerializeField] private Camera satelliteEnvironmentCamera;
	[SerializeField] private Camera mainSatelliteCamera;
	[SerializeField] private Camera targetObjectCamera;
	[SerializeField] private Camera crashedDroneEnvironmentAreaCamera;

	[Header("Main Satellite Area Game Objects")]
	[SerializeField] private GameObject mainSatelliteArea;
	[SerializeField] private InteractableViewOpenerObject mainSatelliteControlPanel;
	[SerializeField] private GameObject mainSatelliteAreaIndicatorEffect;
	[SerializeField] private InteractableViewOpenerObject satelliteTruck;
	[SerializeField] private GameObject satelliteTruckAreaIndicatorEffect;

	[Header("Desert Trail Area Game Objects")]
	[Header("Desert Straight Path Area")]
	[SerializeField] private GameObject desertStraightPathArea;
	[Header("Desert Curved Path Area")]
	[SerializeField] private GameObject desertCurvedPathArea;

	[Header("Crashed Drone Area Game Objects")]
	[SerializeField] private GameObject crashedDroneArea;
	[SerializeField] private GameObject crashedDroneAreaSpawnPoint;

	private DesertEnvironmentStateMachine desertEnvironmentStateMachine;
	private Queue<DesertEnvironmentState> desertEnvironmentStateQueue;

	private void Start()
	{
		SubscribeEnvironmentEvents();

		InitializeDesertEnvironmentStateMachine();
	}

	private void SubscribeEnvironmentEvents()
	{
		// Satellite environment area related events
		dotProductView.OpenViewEvent += () => SetSatelliteEnvironmentState(true);
		dotProductView.QuitViewEvent += () => SetSatelliteEnvironmentState(false);
		activitySixManager.MainSatelliteAreaClearEvent += ClearSatelliteEnvironmentState;


		workView.OpenViewEvent += UpdateDesertEnvironmentStateMachine;
		workView.QuitViewEvent += () => desertEnvironmentStateMachine.TransitionToState(DesertEnvironmentState.None);
		workSubmissionStatusDisplay.ProceedEvent += DequeueDesertEnvironmentStateQueue;

		workGraphInterpretationView.OpenViewEvent += () => SetCrashedDroneEnvironmentState(true);
		workGraphInterpretationView.QuitViewEvent += () => SetCrashedDroneEnvironmentState(false);
	}

	private void InitializeDesertEnvironmentStateMachine()
	{
		desertEnvironmentStateMachine = new DesertEnvironmentStateMachine(this);
		desertEnvironmentStateMachine.Initialize(DesertEnvironmentState.None);

		desertEnvironmentStateQueue = new Queue<DesertEnvironmentState>();
		int numCycles = ActivitySixManager.difficultyConfiguration switch
		{
			Difficulty.Easy => 1,
			Difficulty.Medium => 2,
			Difficulty.Hard => 3,
		};
		for (int i = 0; i < numCycles; i++)
		{
			desertEnvironmentStateQueue.Enqueue(DesertEnvironmentState.StraightPath);
			desertEnvironmentStateQueue.Enqueue(DesertEnvironmentState.CurvedPath);
			desertEnvironmentStateQueue.Enqueue(DesertEnvironmentState.StraightPath);
		}
	}

	private void SetSatelliteEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		satelliteEnvironmentCamera.gameObject.SetActive(isActive);
		mainSatelliteCamera.gameObject.SetActive(isActive);
		targetObjectCamera.gameObject.SetActive(isActive);
	}

	private void ClearSatelliteEnvironmentState()
	{
		SetSatelliteEnvironmentState(false);
		mainSatelliteControlPanel.SetInteractable(false);
		mainSatelliteAreaIndicatorEffect.gameObject.SetActive(false);

		satelliteTruck.SetInteractable(true);
		satelliteTruckAreaIndicatorEffect.gameObject.SetActive(true);
	}

	public void SetDesertStraightPathEnvironmentState(bool isActive)
	{
		desertStraightPathArea.gameObject.SetActive(isActive);
	}

	public void SetDesertCurvedPathEnvironmentState(bool isActive)
	{
		desertCurvedPathArea.gameObject.SetActive(isActive);
	}

	private void DequeueDesertEnvironmentStateQueue()
	{
		desertEnvironmentStateQueue.Dequeue();
		UpdateDesertEnvironmentStateMachine();
	}

	private void UpdateDesertEnvironmentStateMachine()
	{
		if (desertEnvironmentStateQueue.Count == 0)
		{
			// Deactivate area effect and interactable truck
			satelliteTruckAreaIndicatorEffect.gameObject.SetActive(false);
			satelliteTruck.SetInteractable(false);

			desertEnvironmentStateMachine.TransitionToState(DesertEnvironmentState.None);

			// Set crashed drone area active, and teleport player to specified spawn point on said area
			mainSatelliteArea.gameObject.SetActive(false); // deactivate unacessible location for optimized performance
			crashedDroneArea.gameObject.SetActive(true);
			player.transform.position = crashedDroneAreaSpawnPoint.transform.position;
		}
		else
		{
			desertEnvironmentStateMachine.TransitionToState(desertEnvironmentStateQueue.Peek());
		}
	}

	private void SetCrashedDroneEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		crashedDroneEnvironmentAreaCamera.gameObject.SetActive(isActive);
	}
}