using System.Collections.Generic;
using UnityEngine;

public class LessonComponentsScreens : MonoBehaviour
{
    [Header("Lesson Components Screens")]
    [SerializeField] private GameObject lessonOneComponentsScreen;
    [SerializeField] private GameObject lessonTwoComponentsScreen;
    [SerializeField] private GameObject lessonThreeComponentsScreen;
    [SerializeField] private GameObject lessonFourComponentsScreen;
    [SerializeField] private GameObject lessonFiveComponentsScreen;
    [SerializeField] private GameObject lessonSixComponentsScreen;
    [SerializeField] private GameObject lessonSevenComponentsScreen;
    [SerializeField] private GameObject lessonEightComponentsScreen;
    [SerializeField] private GameObject lessonNineComponentsScreen;

    // Lesson Components Dictionary
    private Dictionary<int, GameObject> lessonComponentKeyValuePairs;

    private void Start()
    {
        // Initialize Lesson Components Screens Key Value Pairs
        lessonComponentKeyValuePairs = new Dictionary<int, GameObject>()
        {
            {1, lessonOneComponentsScreen},
            {2, lessonTwoComponentsScreen},
            {3, lessonThreeComponentsScreen},
            {4, lessonFourComponentsScreen},
            {5, lessonFiveComponentsScreen},
            {6, lessonSixComponentsScreen},
            {7, lessonSevenComponentsScreen},
            {8, lessonEightComponentsScreen},
            {9, lessonNineComponentsScreen}
        };
    }

    public void LoadLessonComponentsScreen(int keyValue)
    {
        // Open the specified lesson components screen
        lessonComponentKeyValuePairs[keyValue].SetActive(true);
    }
}
