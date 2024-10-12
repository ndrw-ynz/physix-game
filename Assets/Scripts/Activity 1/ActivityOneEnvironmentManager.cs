using System.Xml.Linq;
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

	[Header("Environment Cameras")]
	[SerializeField] private Camera containerEnvironmentCamera;
	[SerializeField] private Camera SNRoomEnvironmentCamera;

	[Header("Scientific Notation Terminal Area Game Objects")]
	[SerializeField] private InteractableViewOpenerObject containerPickerControlPanel;
	[SerializeField] private InteractableViewOpenerObject scientificNotationControlPanel;
	[SerializeField] private GameObject displayedContainerObject;
	[SerializeField] private GameObject scientificNotationTransitionDoor;
	[SerializeField] private GameObject scientificNotationTransitionBarrier;

	private void Start()
	{
		containerPickerView.OpenViewEvent += () => SetContainerAreaEnvironmentState(true);
		containerPickerView.QuitViewEvent += () => SetContainerAreaEnvironmentState(false);

		scientificNotationView.OpenViewEvent += () => SetSNRoomAreaEnvironmentState(true);
		scientificNotationView.QuitViewEvent += () => SetSNRoomAreaEnvironmentState(false);

		containerSelectionHandler.UpdateSelectedContainerEvent += (boxContainer) => displayedContainerObject.SetActive(boxContainer != null);

		activityOneManager.SNRoomClearEvent += ClearSNRoomEnvironmentState;
		activityOneManager.SNCorrectAnswerEvent += ResetSNRoomAreaEnvironmentState;
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
		containerSelectionHandler.DestroySelectedContainer();
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
}