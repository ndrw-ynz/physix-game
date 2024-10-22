using UnityEngine;
using UnityEngine.UI;

public class PageJumpButtonsDisplay : MonoBehaviour
{
    [Header("Page Circle Buttons Properties")]
    [SerializeField] private PageJumpButton pageJumpButtonPrefab;
    [SerializeField] private HorizontalLayoutGroup pageJumpButtonGroup;

    // Page circle outline animation properties
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

    public void DestroyImmediateAllPageJumpButtons()
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();
        for (int i = 0; i < pageJumpButtons.Length; ++i)
        {
            DestroyImmediate(pageJumpButtons[i].gameObject);
        }
    }

    public void UpdatePageJumpButtonOutline(int currentPageIndex)
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();

        // Loop through the button list and activate only the current page index's button outline
        for (int i =0; i < pageJumpButtons.Length; i++)
        {
            if (i == currentPageIndex)
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
    public void UpdatePageJumpButtonColors(int currentSectorIndex, bool isPageMarkedRead, int i)
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();

        if (isPageMarkedRead)
        {
            pageJumpButtons[i].buttonColor.color = new Color(0.51f, 1, 0.22f); // Darker green color
        }
        else
        {
            pageJumpButtons[i].buttonColor.color = Color.white;
        }
    }

    public int GetPageJumpButtonsLength()
    {
        PageJumpButton[] pageJumpButtons = pageJumpButtonGroup.GetComponentsInChildren<PageJumpButton>();
        return pageJumpButtons.Length;
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
