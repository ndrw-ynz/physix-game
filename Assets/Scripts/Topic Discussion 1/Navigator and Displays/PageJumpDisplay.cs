using UnityEngine;
using UnityEngine.UI;

public class PageJumpDisplay : MonoBehaviour
{
    [Header("Page Circle Buttons Properties")]
    [SerializeField] private PageJumpButton pageJumpButtonPrefab;
    [SerializeField] private HorizontalLayoutGroup pageJumpButtonGroup;

    // Number of buttons to create
    private int _numButtons;

    // Page circle outline animation properties
    private PageJumpButton _pageJumpButton;
    private Color _outlineColor;
    private float _buttonOutlineAnimationDuration = 0.3f; // Duration of animation in seconds
    private bool _animatePageJumpButton = false;
    private float _pageJumpButtonAnimationStartTime;

    private void OnEnable()
    {
        // Add Listeners
        DiscussionNavigator.DiscussionPageStart += LoadPageJumpButtons;
        DiscussionNavigator.SectorChangeEvent += LoadPageJumpButtons;
        DiscussionNavigator.PageChangeEvent += UpdatePageJumpButtonOutline;
        DiscussionNavigator.ReadMarkerChangeEvent += UpdatePageJumpButtonColors;
    }

    private void OnDisable()
    {
        // Remove Listeners
        DiscussionNavigator.DiscussionPageStart -= LoadPageJumpButtons;
        DiscussionNavigator.SectorChangeEvent -= LoadPageJumpButtons;
        DiscussionNavigator.PageChangeEvent -= UpdatePageJumpButtonOutline;
        DiscussionNavigator.ReadMarkerChangeEvent -= UpdatePageJumpButtonColors;
    }

    private void Update()
    {
        AnimatePageJumpButton();
    }

    #region Page Circle Creation and Outline/Color Updates
    private void LoadPageJumpButtons(DiscussionNavigator discNav)
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();

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
            GeneratePageJumpButton(i);
        }

        // Update the page circle button outlines and properly set the active outline
        UpdatePageJumpButtonOutline(discNav);
    }
    private void UpdatePageJumpButtonOutline(DiscussionNavigator discNav)
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();

        // Loop through the button list and activate only the current page index's button outline
        for (int i =0; i < pageJumpButtons.Length; i++)
        {
            if (i == discNav.GetCurrentPageIndex())
            {
                pageJumpButtons[i].buttonOutline.gameObject.SetActive(true);
                ActivatePageJumpAnimation(pageJumpButtons[i]);
            }
            else
            {
                pageJumpButtons[i].buttonOutline.gameObject.SetActive(false);
            }
        }
    }
    private void UpdatePageJumpButtonColors(DiscussionNavigator discNav)
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();

        // Loop through the button list and change their colors to green if page is marked as read
        for (int i = 0; i < pageJumpButtons.Length; i++)
        {
            if (discNav.IsPageMarkedRead(i))
            {
                pageJumpButtons[i].buttonColor.color = new Color(0.51f, 1, 0.22f); // Darker green color
            }
            else
            {
                pageJumpButtons[i].buttonColor.color = Color.white;
            }
        }
    }
    private void GeneratePageJumpButton(int buttonIndex)
    {
        // Instantiate and set parent of new page circle button to the horizontal group layout
        PageJumpButton newPageJumpButton = Instantiate(pageJumpButtonPrefab);
        newPageJumpButton.transform.SetParent(pageJumpButtonGroup.transform, false);
        newPageJumpButton.name = $"Page Circle Button {buttonIndex + 1}";

        // Initialize index for jumping directly to its page upon button press
        newPageJumpButton.Initialize(buttonIndex);
    }
    #endregion

    #region Page Circle Outline Animation
    private void ActivatePageJumpAnimation(PageJumpButton currPageJumpButton)
    {
        // Setup the page jump outline to be animated and activates animation sequence
        _pageJumpButton = currPageJumpButton;
        _outlineColor = Color.black;
        _animatePageJumpButton = true;
        _pageJumpButtonAnimationStartTime = Time.time;
    }
    private void AnimatePageJumpButton()
    {
        if (_animatePageJumpButton)
        {
            // Calculate elapsed time
            float elapsedTime = Time.time - _pageJumpButtonAnimationStartTime;
            if (elapsedTime < _buttonOutlineAnimationDuration)
            {
                // Manually change alpha value of current outline color
                float newOutlineAlpha = Mathf.Lerp(0f, 1.0f, elapsedTime / _buttonOutlineAnimationDuration);
                _outlineColor.a = newOutlineAlpha;
                _pageJumpButton.buttonOutline.color = _outlineColor;
            }
            else
            {
                // After animation, set animation mode to false
                _animatePageJumpButton = false;
            }
        }
    }
    #endregion
}
