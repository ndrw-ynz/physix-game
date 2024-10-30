using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LessonSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<int> LessonSelectClick;
    public static event Action LessonLockedClick;

    [Header("Availability of Lesson")]
    public bool isUnlocked;

    [Header("Lesson Select Button Properties")]
    [SerializeField] private Button lessonSelectButton;
    [SerializeField] private Image panelImage;
    [SerializeField] private Image grayLockImage;

    [Header("Lesson Select Button Number")]
    [SerializeField] private int lessonNumber;
    [SerializeField] private int increaseSizeValue;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        lessonSelectButton.onClick.AddListener(() => CheckLessonAvailability());

        // Load proper lock image state for current button
        LoadLockImage();
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        lessonSelectButton.onClick.RemoveAllListeners();

        if (isHovered)
        {
            // Change panel color to white on disable for current hovered button
            panelImage.color = Color.white;
            isHovered = false;
        }
    }

    private void LoadLockImage()
    {
        // Load gray lock image
        if (isUnlocked)
        {
            grayLockImage.gameObject.SetActive(false);
        }
        else
        {
            grayLockImage.gameObject.SetActive(true);
        }
    }
    private void CheckLessonAvailability()
    {
        // Check if current lesson is unlocked or not
        if (isUnlocked)
        {
            LessonSelectClick?.Invoke(lessonNumber);
        }
        else
        {
            LessonLockedClick?.Invoke();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change panel color to light blue on hover if lesson is unlocked
        if (isUnlocked)
        {
            panelImage.color = new Color(0.5037736f, 0.6785805f, 1f);
            isHovered = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Change panel color to white on unhover for the current hovered button
        if (isHovered)
        {
            panelImage.color = Color.white;
            isHovered = false;
        }
    }

}
