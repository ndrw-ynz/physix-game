using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageCircleButtonsDisplay : MonoBehaviour
{
    public PageJumpButton pageCircleButtonPrefab;

    public List<PageJumpButton> pageCircleButtonList = new List<PageJumpButton>();
    private RectTransform pageCircleAreaParent;
    private int _numButtons;
    private float _buttonSpacing = 100.0f;

    public static event Action<PageCircleButtonsDisplay, int> PageCircleStateUpdate;
    private void OnEnable()
    {
        DiscussionNavigator.DiscussionPageStart += LoadPageCircleButtons;
        DiscussionNavigator.DiscussionPageStart += UpdatePageCircleButtonStates;
        DiscussionNavigator.SectorChangeEvent += LoadPageCircleButtons;
        DiscussionNavigator.SectorChangeEvent += UpdatePageCircleButtonStates;
        DiscussionNavigator.PageChangeEvent += UpdatePageCircleButtonStates;
        DiscussionNavigator.UnderstandMarkerChangeEvent += UpdatePageCircleButtonColors;
    }

    private void OnDisable()
    {
        DiscussionNavigator.DiscussionPageStart -= LoadPageCircleButtons;
        DiscussionNavigator.DiscussionPageStart -= UpdatePageCircleButtonStates;
        DiscussionNavigator.SectorChangeEvent -= LoadPageCircleButtons;
        DiscussionNavigator.SectorChangeEvent -= UpdatePageCircleButtonStates;
        DiscussionNavigator.PageChangeEvent -= UpdatePageCircleButtonStates;
        DiscussionNavigator.UnderstandMarkerChangeEvent -= UpdatePageCircleButtonColors;
    }

    private void LoadPageCircleButtons(DiscussionNavigator discNav)
    {
        if (pageCircleButtonList.Count > 0) { RemoveAllButtons(); }

        pageCircleAreaParent = GameObject.Find("BUTTONS").transform.Find("Page Circle Buttons").GetComponent<RectTransform>();
        _numButtons = discNav.GetCurrentSectorPagesCount();

        float totalWidth = (_numButtons - 1) * _buttonSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < _numButtons; i++)
        {
            Vector2 buttonPosition = new Vector2(startX + i * _buttonSpacing, 0f);
            GeneratePageCircleButton(buttonPosition, i);
        }
    }

    private void GeneratePageCircleButton(Vector2 buttonPosition, int i)
    {
        PageJumpButton newPageCircleButton = Instantiate(pageCircleButtonPrefab);
        newPageCircleButton.transform.SetParent(pageCircleAreaParent, false);
        newPageCircleButton.name = $"Page Circle Button {i + 1}";
        newPageCircleButton.transform.localPosition = buttonPosition;
        newPageCircleButton.Initialize(i);
        pageCircleButtonList.Add(newPageCircleButton);
    }

    private void UpdatePageCircleButtonStates(DiscussionNavigator discNav)
    {
        int currentPageIndex = discNav.GetCurrentPageIndex();
        for (int i =0; i < pageCircleButtonList.Count; i++)
        {
            if (i == currentPageIndex)
            {
                pageCircleButtonList[i].buttonOutline.gameObject.SetActive(true);
                PageCircleStateUpdate?.Invoke(this, i);
            }
            else
            {
                pageCircleButtonList[i].buttonOutline.gameObject.SetActive(false);
            }
        }
    }

    private void UpdatePageCircleButtonColors(DiscussionNavigator discNav)
    {
        for (int i = 0; i < pageCircleButtonList.Count; i++)
        {
            if (discNav.IsPageMarkedUnderstood(i))
            {
                pageCircleButtonList[i].buttonColor.color = new Color(0.51f, 1, 0.22f);
            }
            else
            {
                pageCircleButtonList[i].buttonColor.color = Color.white;
            }
        }
    }

    private void RemoveAllButtons()
    {
        for (int i = 0; i < pageCircleButtonList.Count; ++i)
        {
            Destroy(pageCircleButtonList[i].gameObject);
        }
        pageCircleButtonList.Clear();
    }
}
