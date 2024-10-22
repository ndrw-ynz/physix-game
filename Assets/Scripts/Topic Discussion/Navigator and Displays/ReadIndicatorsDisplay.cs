using UnityEngine;

public class ReadIndicatorsDisplay : MonoBehaviour
{
    [Header("Read Indicator Buttons")]
    [SerializeField] private ReadIndicatorButton markAsReadButton;
    [SerializeField] private ReadIndicatorButton markAsNotReadButton;

    #region Read Indicator Buttons
    public void ChangeReadIndicatorButtonsState(int currentSectorIndex, int currentPageIndex, DiscussionPagesDisplay discNav)
    {
        if (!discNav.CurrentPageIsMarkedRead(currentSectorIndex, currentPageIndex))
        {
            // Activate the mark as read button
            markAsReadButton.gameObject.SetActive(true);
            markAsNotReadButton.gameObject.SetActive(false);
        }
        else
        {
            // Activate the mark as not yet read button
            markAsReadButton.gameObject.SetActive(false);
            markAsNotReadButton.gameObject.SetActive(true);
        }
    }
    #endregion
}