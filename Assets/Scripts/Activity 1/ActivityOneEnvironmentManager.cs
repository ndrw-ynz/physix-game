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

	[Header("Environment Cameras")]
	[SerializeField] private Camera containerEnvironmentCamera;
	[SerializeField] private Camera SNRoomEnvironmentCamera;
	[SerializeField] private Camera varianceRoomEnvironmentCamera;
	[SerializeField] private Camera APRoomEnvironmentCamera;

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

		containerSelectionHandler.UpdateSelectedContainerEvent += (boxContainer) => displayedContainerObject.SetActive(boxContainer != null);

		activityOneManager.SNRoomClearEvent += ClearSNRoomEnvironmentState;
		activityOneManager.SNCorrectAnswerEvent += ResetSNRoomAreaEnvironmentState;
		activityOneManager.VarianceRoomClearEvent += ClearVarianceRoomEnvironmentState;
	}

	#region Scientific Notation Terminal Room
	private void SetContainerAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		containerEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void SetSNRoomAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
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
		displayedContainerObject.SetActive(false);

		scientificNotationTransitionDoor.GetComponent<Animator>().SetBool("door_closed", false); ;
		scientificNotationTransitionBarrier.gameObject.SetActive(false);
	}

	#endregion

	#region Variance Terminal Room
	private void SetVarianceRoomAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		varianceRoomEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void ClearVarianceRoomEnvironmentState()
	{
		SetVarianceRoomAreaEnvironmentState(false);
		varianceControlPanel.SetInteractable(false);

		varianceTransitionDoor.GetComponent<Animator>().SetBool("door_closed", false); ;
		varianceTransitionBarrier.gameObject.SetActive(false);
	}
	#endregion

	#region Accuracy & Precision Terminal Room
	private void SetAPRoomAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		APRoomEnvironmentCamera.gameObject.SetActive(isActive);
	}
	#endregion
}