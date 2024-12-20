using UnityEngine;

public class ActivityOneEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityOneManager activityOneManager;

	[Header("Handlers")]
	[SerializeField] private ContainerSelectionHandler containerSelectionHandler;

	[Header("Views")]
	[SerializeField] private ContainerPickerView containerPickerView;
	[SerializeField] private ScientificNotationView scientificNotationView;
	[SerializeField] private VarianceView varianceView;
	[SerializeField] private AccuracyPrecisionView accuracyPrecisionView;
	[SerializeField] private ErrorsView errorsView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera containerEnvironmentCamera;
	[SerializeField] private Camera SNRoomEnvironmentCamera;
	[SerializeField] private Camera varianceRoomEnvironmentCamera;
	[SerializeField] private Camera APRoomEnvironmentCamera;
	[SerializeField] private Camera errorsRoomEnvironmentCamera;

	[Header("Scientific Notation Terminal Area Game Objects")]
	[SerializeField] private InteractableViewOpenerObject containerPickerControlPanel;
	[SerializeField] private InteractableViewOpenerObject scientificNotationControlPanel;
	[SerializeField] private GameObject displayedContainerObject;
	[SerializeField] private GameObject scientificNotationTransitionDoor;
	[SerializeField] private GameObject scientificNotationTransitionBarrier;

	[Header("Variance Room Terminal Area Game Objects")]
	[SerializeField] private InteractableViewOpenerObject varianceControlPanel;
	[SerializeField] private GameObject varianceTransitionDoor;
	[SerializeField] private GameObject varianceTransitionBarrier;

	[Header("Accuracy Precision Terminal Area Game Objects")]
	[SerializeField] private InteractableViewOpenerObject accuracyPrecisionControlPanel;
	[SerializeField] private GameObject accuracyPrecisionTransitionDoor;
	[SerializeField] private GameObject accuracyPrecisionTransitionBarrier;

	private void Start()
	{
		containerPickerView.OpenViewEvent += () => SetContainerAreaEnvironmentState(true);
		containerPickerView.QuitViewEvent += () => SetContainerAreaEnvironmentState(false);

		scientificNotationView.OpenViewEvent += () => SetSNRoomAreaEnvironmentState(true);
		scientificNotationView.QuitViewEvent += () => SetSNRoomAreaEnvironmentState(false);

		varianceView.OpenViewEvent += () => SetVarianceRoomAreaEnvironmentState(true);
		varianceView.QuitViewEvent += () => SetVarianceRoomAreaEnvironmentState(false);

		accuracyPrecisionView.OpenViewEvent += () => SetAPRoomAreaEnvironmentState(true);
		accuracyPrecisionView.QuitViewEvent += () => SetAPRoomAreaEnvironmentState(false);

		errorsView.OpenViewEvent += () => SetErrorsRoomAreaEnvironmentState(true);
		errorsView.QuitViewEvent += () => SetErrorsRoomAreaEnvironmentState(false);

		containerSelectionHandler.UpdateSelectedContainerEvent += (boxContainer) => displayedContainerObject.SetActive(boxContainer != null);

		activityOneManager.SNRoomClearEvent += ClearSNRoomEnvironmentState;
		activityOneManager.SNCorrectAnswerEvent += ResetSNRoomAreaEnvironmentState;
		activityOneManager.VarianceRoomClearEvent += ClearVarianceRoomEnvironmentState;
		activityOneManager.APRoomClearEvent += ClearAPRoomEnvironmentState;
	}

	#region Scientific Notation Terminal Room
	private void SetContainerAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityOneManager.SetMissionObjectiveDisplay(!isActive);
		containerEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void SetSNRoomAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityOneManager.SetMissionObjectiveDisplay(!isActive);
		SNRoomEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void ResetSNRoomAreaEnvironmentState()
	{
		containerSelectionHandler.ClearSelectedContainer();
		SetSNRoomAreaEnvironmentState(false);
		displayedContainerObject.SetActive(false);
	}

	private void ClearSNRoomEnvironmentState()
	{
		SetSNRoomAreaEnvironmentState(false);
		containerPickerControlPanel.SetInteractable(false);
		scientificNotationControlPanel.SetInteractable(false);
		displayedContainerObject.SetActive(true);

		scientificNotationTransitionDoor.GetComponent<Animator>().SetBool("door_closed", false); ;
		scientificNotationTransitionBarrier.gameObject.SetActive(false);

		SceneSoundManager.Instance.PlaySFX("Door_Large_Open_01");
	}

	#endregion

	#region Variance Terminal Room
	private void SetVarianceRoomAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityOneManager.SetMissionObjectiveDisplay(!isActive);
		varianceRoomEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void ClearVarianceRoomEnvironmentState()
	{
		SetVarianceRoomAreaEnvironmentState(false);
		varianceControlPanel.SetInteractable(false);
		displayedContainerObject.SetActive(false);

		varianceTransitionDoor.GetComponent<Animator>().SetBool("door_closed", false);
		varianceTransitionBarrier.gameObject.SetActive(false);

		SceneSoundManager.Instance.PlaySFX("Door_Large_Open_01");
	}
	#endregion

	#region Accuracy & Precision Terminal Room
	private void SetAPRoomAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityOneManager.SetMissionObjectiveDisplay(!isActive);
		APRoomEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void ClearAPRoomEnvironmentState()
	{
		SetAPRoomAreaEnvironmentState(false);
		accuracyPrecisionControlPanel.SetInteractable(false);

		accuracyPrecisionTransitionDoor.GetComponent<Animator>().SetBool("door_closed", false);
		accuracyPrecisionTransitionBarrier.gameObject.SetActive(false);

		SceneSoundManager.Instance.PlaySFX("Door_Large_Open_01");
	}
	#endregion

	#region Errors Terminal Room
	private void SetErrorsRoomAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityOneManager.SetMissionObjectiveDisplay(!isActive);
		errorsRoomEnvironmentCamera.gameObject.SetActive(isActive);
	}
	#endregion
}