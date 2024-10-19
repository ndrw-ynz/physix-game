using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionObjectiveDisplayUI : MonoBehaviour
{
    [SerializeField] private List<Toggle> missionObjectiveList;

    // func for setting objective based on index
    public void ClearMissionObjective(int index)
    {
        Toggle missionObjectiveToggle = missionObjectiveList[index];

        missionObjectiveToggle.isOn = true;

		Image backgroundImage = missionObjectiveToggle.targetGraphic as Image;
        backgroundImage.color = Color.green;
    }

    public void UpdateMissionObjectiveText(int index, string updatedText)
    {
		Toggle missionObjectiveToggle = missionObjectiveList[index];

		TextMeshProUGUI toggleLabel = missionObjectiveToggle.GetComponentInChildren<TextMeshProUGUI>();
		toggleLabel.text = updatedText;
	}
}
