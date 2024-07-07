using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [System.Serializable]
    public class ProgressBar
    {
        public GameObject progressBar;
        public TextMeshProUGUI sectorTitle;
        public TextMeshProUGUI progressCounter;
    }

    public DiscussionNavigator discussionNavigator;
    public List<ProgressBar> sectorProgressBars;

    private void Start()
    {
        loadProgressBars();
    }

    public void loadProgressBars()
    {
        for (int i = 0; i < discussionNavigator.subTopicsList.Count; i++)
        {
            string currentSectorTitle = discussionNavigator.subTopicsList[i].sectorTitle;
            int understoodPages = 0;
            int currentSectorTotalPages = discussionNavigator.subTopicsList[i].pages.Count;
            Debug.Log("Num of Pages:" + discussionNavigator.subTopicsList[i].pages.Count);

            for (int j = 0; j < discussionNavigator.subTopicsList[i].pages.Count; j++)
            {
                Debug.Log("Current Sector Index:" + i);
                Debug.Log("Current Page Index:" + j);
                if (currentPageIsMarkedUnderstood(i, j))
                {
                    understoodPages++;
                }
            }

            sectorProgressBars[i].sectorTitle.SetText(currentSectorTitle);
            sectorProgressBars[i].progressCounter.SetText($"{understoodPages}/{currentSectorTotalPages}");
            
        }
    }

    private void changeMarkButtonState()
    {

    }


    private bool currentPageIsMarkedUnderstood(int currentSectorIndex, int currentPageIndex)
    {
        return discussionNavigator.subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedUnderstood;
    }
}
