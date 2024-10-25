using UnityEngine;

public class ReadIndicatorsDisplay : MonoBehaviour
{
    [Header("Read Indicator Buttons")]
    [SerializeField] private ReadIndicatorButton markAsReadButton;
    [SerializeField] private ReadIndicatorButton markAsNotReadButton;

    #region Read Indicator Buttons
    public void ChangeReadIndicatorButtonsState(int currentSectorIndex, int currentPageIndex, bool isPageMarkedRead)
    {
        if (!isPageMarkedRead)
        {
            // Activate the mark as read button if page is not yet marked as read
            markAsReadButton.gameObject.SetActive(true);
            markAsNotReadButton.gameObject.SetActive(false);
        }
        else
        {
            // Activate the mark as not yet read button if page has been marked as read
            markAsReadButton.gameObject.SetActive(false);
            markAsNotReadButton.gameObject.SetActive(true);
        }
    }
    #endregion
}