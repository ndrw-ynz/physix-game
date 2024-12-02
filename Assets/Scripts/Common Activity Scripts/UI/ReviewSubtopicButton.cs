using UnityEngine;
using UnityEngine.SceneManagement;

public class ReviewSubtopicButton : MonoBehaviour
{
    [Header("Indexes Properties to Load")]
    [SerializeField] private int sectorIndexToOpen;
    [SerializeField] private int pageIndexToOpen;

    [Header("Lesson Number to Load")]
    [SerializeField] private int discussionNumber;

    public void LoadSpecifiedDiscussionSubtopic()
    {
        TopicDiscussionManager.currentSectorIndex = sectorIndexToOpen;
        TopicDiscussionManager.currentPageIndex = pageIndexToOpen;

        SceneManager.LoadScene($"Topic Discussion {discussionNumber}");
    }
}
