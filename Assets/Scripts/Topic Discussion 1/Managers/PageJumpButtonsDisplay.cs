using UnityEngine;
using UnityEngine.UI;

public class PageJumpButtonsDisplay : MonoBehaviour
{
    [Header("Page Circle Buttons Properties")]
    [SerializeField] private PageJumpButton pageCircleButtonPrefab;
    [SerializeField] private HorizontalLayoutGroup pageCircleButtonGroup;

    // Number of buttons to create
    private int _numButtons;

    // Page circle outline animation properties
    private PageJumpButton _pageJumpButton;
    private Color _currentOutlineColor;
    private float _currentOutlineAlpha;
    private float _buttonOutlineAnimationDuration = 0.3f; // Duration of animation in seconds
    private bool _animateButton = false;
    private float _animationStartTime;

    private void OnEnable()
    {
        // Add Listeners
        DiscussionNavigator.DiscussionPageStart += LoadPageJumpButtons;
        DiscussionNavigator.SectorChangeEvent += LoadPageJumpButtons;
        DiscussionNavigator.PageChangeEvent += UpdatePageJumpButtonOutline;
        DiscussionNavigator.UnderstandMarkerChangeEvent += UpdatePageJumpButtonColors;
    }

    private void OnDisable()
    {
        // Remove Listeners
        DiscussionNavigator.DiscussionPageStart -= LoadPageJumpButtons;
        DiscussionNavigator.SectorChangeEvent -= LoadPageJumpButtons;
        DiscussionNavigator.PageChangeEvent -= UpdatePageJumpButtonOutline;
        DiscussionNavigator.UnderstandMarkerChangeEvent -= UpdatePageJumpButtonColors;
    }

    private void Update()
    {
        AnimatePageCircle();
    }

    #region Page Circle Creation and Outline/Color Updates
    private void LoadPageJumpButtons(DiscussionNavigator discNav)
    {
        PageJumpButton[] pageJumpButtons = pageCircleButtonGroup.GetComponentsInChildren<PageJumpButton>();

        // Remove all buttons if there are existing buttons
        if (pageJumpButtons.Length > 0) 
        {
            for (int i = 0; i < pageJumpButtons.Length; ++i)
            {
                DestroyImmediate(pageJumpButtons[i].gameObject);
            }
        }

        // Create buttons for the new sector
        _numButtons = discNav.GetCurrentSectorPagesCount();
        for (int i = 0; i < _numButtons; i++)
        {
            GeneratePageCircleButton(i);
        }

        // Update the page circle button outlines and properly set the active outline
        UpdatePageJumpButtonOutline(discNav);
    }

    private void UpdatePageJumpButtonOutline(DiscussionNavigator discNav)
    {
        PageJumpButton[] pageJumpButtons = pageCircleButtonGroup.GetComponentsInChildren<PageJumpButton>();

        // Loop through the button list and activate only the current page index's button outline
        for (int i =0; i < pageJumpButtons.Length; i++)
        {
            if (i == discNav.GetCurrentPageIndex())
            {
                pageJumpButtons[i].buttonOutline.gameObject.SetActive(true);
                ActivatePageCircleAnimation(pageJumpButtons[i]);
            }
            else
            {
                pageJumpButtons[i].buttonOutline.gameObject.SetActive(false);
            }
        }
    }

    private void UpdatePageJumpButtonColors(DiscussionNavigator discNav)
    {
        PageJumpButton[] pageJumpButtons = pageCircleButtonGroup.GetComponentsInChildren<PageJumpButton>();

        // Loop through the button list and change their colors to green if page is marked as understood
        for (int i = 0; i < pageJumpButtons.Length; i++)
        {
            if (discNav.IsPageMarkedUnderstood(i))
            {
                pageJumpButtons[i].buttonColor.color = new Color(0.51f, 1, 0.22f); // Darker green color
            }
            else
            {
                pageJumpButtons[i].buttonColor.color = Color.white;
            }
        }
    }

    private void GeneratePageCircleButton(int buttonIndex)
    {
        // Instantiate and set parent of new page circle button to the horizontal group layout
        PageJumpButton newPageCircleButton = Instantiate(pageCircleButtonPrefab);
        newPageCircleButton.transform.SetParent(pageCircleButtonGroup.transform, false);
        newPageCircleButton.name = $"Page Circle Button {buttonIndex + 1}";

        // Initialize index for jumping directly to its page upon button press
        newPageCircleButton.Initialize(buttonIndex);
    }
    #endregion

    #region Page Circle Outline Animation
    private void ActivatePageCircleAnimation(PageJumpButton currPageCircleButton)
    {
        _pageJumpButton = currPageCircleButton;
        _currentOutlineColor = Color.black;
        _currentOutlineAlpha = 0f; // Set back to zero
        _animateButton = true;
        _animationStartTime = Time.time;
    }

    private void AnimatePageCircle()
    {
        if (_animateButton)
        {
            // Calculate elapsed time
            float elapsedTime = Time.time - _animationStartTime;
            if (elapsedTime < _buttonOutlineAnimationDuration)
            {
                // Manually change alpha value of current outline color
                _currentOutlineAlpha = Mathf.Lerp(0f, 1.0f, elapsedTime / _buttonOutlineAnimationDuration);
                _currentOutlineColor.a = _currentOutlineAlpha;
                _pageJumpButton.buttonOutline.color = _currentOutlineColor;
            }
            else
            {
                _animateButton = false;
            }
        }
    }
    #endregion
}
