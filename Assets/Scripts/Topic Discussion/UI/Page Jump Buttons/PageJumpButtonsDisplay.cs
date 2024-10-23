using UnityEngine;
using UnityEngine.UI;

public class PageJumpButtonsDisplay : MonoBehaviour
{
    [Header("Page Circle Buttons Properties")]
    [SerializeField] private PageJumpButton pageJumpButtonPrefab;
    [SerializeField] private HorizontalLayoutGroup pageJumpButtonGroup;

    // Page jump button outline animation properties
    private PageJumpButton _pageJumpButton;
    private Color _outlineColor;
    private float _buttonOutlineAnimationDuration = 0.3f; // Duration of animation in seconds
    private bool _animatePageJumpButton = false;
    private float _pageJumpButtonAnimationStartTime;

    private void Update()
    {
        AnimatePageJumpButton();
    }

    #region Page Circle Creation and Outline/Color Updates
    public void GeneratePageJumpButton(int buttonIndex)
    {
        // Instantiate and set parent of new page circle button to the horizontal group layout
        PageJumpButton newPageJumpButton = Instantiate(pageJumpButtonPrefab);
        newPageJumpButton.transform.SetParent(pageJumpButtonGroup.transform, false);
        newPageJumpButton.name = $"Page Circle Button {buttonIndex + 1}";

        // Initialize index for jumping directly to its page upon button press
        newPageJumpButton.Initialize(buttonIndex);
    }
    public void UpdatePageJumpButtonsOutline(int currentPageIndex)
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();

        // Loop through the button list
        for (int i =0; i < pageJumpButtons.Length; i++)
        {
            if (i == currentPageIndex)
            {
                // Activate the current page index's button outline and animate with a fade in style animation
                pageJumpButtons[i].buttonOutline.gameObject.SetActive(true);
                ActivatePageJumpAnimation(pageJumpButtons[i]);
            }
            else
            {
                // Ensure all other outlines are deactivated
                pageJumpButtons[i].buttonOutline.gameObject.SetActive(false);
            }
        }
    }
    public void UpdatePageJumpButtonColor(int currentSectorIndex, bool isPageMarkedRead, int i)
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();

        if (isPageMarkedRead)
        {
            // Set the button's color to dark green if page is marked as read
            pageJumpButtons[i].buttonColor.color = new Color(0.51f, 1, 0.22f); // Darker green color
        }
        else
        {
            // Set the button's color to white if the page is not yet marked as read
            pageJumpButtons[i].buttonColor.color = Color.white;
        }
    }
    public void DestroyImmediateAllPageJumpButtons()
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();

        // Loop through the current page jump button list
        for (int i = 0; i < pageJumpButtons.Length; ++i)
        {
            // Destroy the buttons immediately to avoid concurrency issues
            DestroyImmediate(pageJumpButtons[i].gameObject);
        }
    }
    public int GetPageJumpButtonsLength()
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();
        // Get the current page jump button list's length
        return pageJumpButtons.Length;
    }
    #endregion

    #region Page Jump Button Outline Animation
    private void ActivatePageJumpAnimation(PageJumpButton currPageJumpButton)
    {
        /* Sets the right page jump button, right outline color, activate the page jump button, 
         * and records the start time of the page jump button animation*/
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
                // If the elapsed time is less than the set page jump button animation duration, keep animating the fade in effect
                float newOutlineAlpha = Mathf.Lerp(0f, 1.0f, elapsedTime / _buttonOutlineAnimationDuration);
                _outlineColor.a = newOutlineAlpha;
                _pageJumpButton.buttonOutline.color = _outlineColor;
            }
            else
            {
                // If the elapsed time has reached the set page jump button animation duration, ensure page alpha is 1 and stop animation
                _outlineColor.a = 1;
                _pageJumpButton.buttonOutline.color = _outlineColor;
                _animatePageJumpButton = false;
            }
        }
    }
    #endregion
}
