using UnityEngine;
using UnityEngine.UI;

public class ProgressBarsDisplay : MonoBehaviour
{
    [Header("Progress Bar Button Properties")]
    [SerializeField] private ProgressBarButton progressBarButtonPrefab;
    [SerializeField] private VerticalLayoutGroup progressBarButtonGroup;

    // Progress bar animation properties
    private Image _temporaryImage;
    private Image _finalImage;
    private Color _oldColor;
    private Color _newColor;
    private float _buttonAnimationDuration = 0.2f;     // Animation speed in seconds
    private bool _animateProgressBarButton = false;
    private float _progressBarAnimationStartTime;       // Start time of progress bar animation

    // Indicator line animation properties
    private ProgressBarButton _indicatorLine;
    private float _targetWidth = 130f;                  // Constant target width
    private float _currentHeight;
    private float _indicatorAnimationDuration = 0.25f;  // Animation speed in seconds
    private bool _animateIndicatorLine = false;
    private float _indicatorAnimationStartTime;         // Start time of indicator line animation

    private void Update()
    {
        AnimateProgressBarButton();
        AnimateIndicatorLine();
    }

    #region Loading/Updating of Progress Bar Colors, Progress Count Text, and Indicator Lines
    public void GenerateProgressBarButton(int i, string sectorTitle, string progressCount)
    {
        // Generate progress bar button with proper title and progress count
        ProgressBarButton newButton = Instantiate(progressBarButtonPrefab);
        newButton.transform.SetParent(progressBarButtonGroup.transform, false);
        newButton.name = $"Progress Button {i + 1}";
        newButton.Initialize(sectorTitle, progressCount, i);
    }
    public void LoadProgressBarButtonsColors(double currentReadPagesCount, double currentTotalPages, int i)
    {
        ProgressBarButton[] progressBarButtons = progressBarButtonGroup.GetComponentsInChildren<ProgressBarButton>();

            // Deactivate the temporary background color
            progressBarButtons[i].progressBarTempColor.gameObject.SetActive(false);

            // Calculate progress percentage
            double currProgressBarPercentage = currentReadPagesCount / currentTotalPages * 100;
            if (currProgressBarPercentage == 100)
            {
                // Set progress bar color to light color green
                progressBarButtons[i].progressBarFinalColor.color = new Color(0.5890471f, 1f, 0.5264151f);
            }
            else if (currProgressBarPercentage > 50)
            {
                // Set progress bar color to light color yellow
                progressBarButtons[i].progressBarFinalColor.color = new Color(0.9546386f, 1f, 0.5254902f);
            }
            else if (currentReadPagesCount > 0)
            {
                // Set progress bar color to light color gray
                progressBarButtons[i].progressBarFinalColor.color = new Color(0.8339623f, 0.8339623f, 0.8339623f);
            }
            else
            {
                // Set progress bar color to light color gray
                progressBarButtons[i].progressBarFinalColor.color = new Color(0.764151f, 0.764151f, 0.764151f);
        }
    }
    public void UpdateProgressBarButtonTextAndColor(int currentSectorIndex, double currReadPagesCount, double currSectorPagesCount)
    {
        ProgressBarButton[] progressBarButtons = progressBarButtonGroup.GetComponentsInChildren<ProgressBarButton>();
        
        // Activate the temporary background color of the progress bar to give way for the color transition
        progressBarButtons[currentSectorIndex].progressBarTempColor.gameObject.SetActive(true);

        // Assign text value for read pages and total pages to the progress bar
        progressBarButtons[currentSectorIndex].progressCountText.text = $"{currReadPagesCount}/{currSectorPagesCount}";

        // Calculate the percentage of read pages
        double currProgressBarPercentage = currReadPagesCount / currSectorPagesCount * 100;

        if (currProgressBarPercentage == 100)
        {
            // Transition progress bar color to light color green
            Image temporaryImage = progressBarButtons[currentSectorIndex].progressBarTempColor;
            Image finalImage = progressBarButtons[currentSectorIndex].progressBarFinalColor;
            Color oldColor = progressBarButtons[currentSectorIndex].progressBarFinalColor.color;
            Color newColor = new Color(0.5890471f, 1f, 0.5264151f);
            ActivateProgressBarButtonAnimation(temporaryImage, finalImage, oldColor, newColor);
        }
        else if (currProgressBarPercentage > 50)
        {
            // Transition progress bar color to light color yellow
            Image temporaryImage = progressBarButtons[currentSectorIndex].progressBarTempColor;
            Image finalImage = progressBarButtons[currentSectorIndex].progressBarFinalColor;
            Color oldColor = progressBarButtons[currentSectorIndex].progressBarFinalColor.color;
            Color newColor = new Color(0.9546386f, 1f, 0.5254902f);
            ActivateProgressBarButtonAnimation(temporaryImage, finalImage, oldColor, newColor);
        }
        else if (currReadPagesCount > 0)
        {
            // Transition progress bar color to light color gray
            Image temporaryImage = progressBarButtons[currentSectorIndex].progressBarTempColor;
            Image finalImage = progressBarButtons[currentSectorIndex].progressBarFinalColor;
            Color oldColor = progressBarButtons[currentSectorIndex].progressBarFinalColor.color;
            Color newColor = Color.white;
            ActivateProgressBarButtonAnimation(temporaryImage, finalImage, oldColor, newColor);
        }
        else
        {
            // Transition progress bar color to gray
            Image temporaryImage = progressBarButtons[currentSectorIndex].progressBarTempColor;
            Image finalImage = progressBarButtons[currentSectorIndex].progressBarFinalColor;
            Color oldColor = progressBarButtons[currentSectorIndex].progressBarFinalColor.color;
            Color newColor = new Color(0.764151f, 0.764151f, 0.764151f);
            ActivateProgressBarButtonAnimation(temporaryImage, finalImage, oldColor, newColor);
        }
    }
    public void UpdateIndicatorLinePosition(int currentSectorIndex)
    {
        ProgressBarButton[] progressBarButtons = progressBarButtonGroup.GetComponentsInChildren<ProgressBarButton>();

        // Check all indicator lines
        for (int i = 0; i < progressBarButtons.Length; i++)
        {
            if (i == currentSectorIndex)
            {
                // Activate sector indicator for the current viewed sector
                progressBarButtons[i].progressBarIndicator.gameObject.SetActive(true);

                float currentHeight = progressBarButtons[i].progressBarIndicator.rectTransform.rect.height;
                ActivateIndicatorLineAnimation(progressBarButtons[i], currentHeight);
            }
            else 
            {
                // Ensure other sector indicators and deactivated
                progressBarButtons[i].progressBarIndicator.gameObject.SetActive(false);
            }
        }
    }
    public int GetProgressBarButtonsLength()
    {
        ProgressBarButton[] progressBarButtons = progressBarButtonGroup.GetComponentsInChildren<ProgressBarButton>();
        // Get the current progress bar buttons list's length
        return progressBarButtons.Length;
    }
    #endregion

    #region Progress Bar and Indicator Line Animations
    private void ActivateProgressBarButtonAnimation(Image temporaryImage, Image finalImage, Color oldColor, Color newColor)
    {
        /* Sets the right temporary image, right final image, old color, new color, activate the progress bar button animation, 
         * and records the start time of the progress bar button animation*/
        _temporaryImage = temporaryImage;
        _finalImage = finalImage;
        _oldColor = oldColor;
        _newColor = newColor;
        _animateProgressBarButton = true;
        _progressBarAnimationStartTime = Time.time;
    }
    private void ActivateIndicatorLineAnimation(ProgressBarButton indicatorLine, float currentHeight)
    {
        /* Sets the right indicator line, current height, activate the progress bar button animation, 
         * and record the start time of the indicator line animation*/
        _indicatorLine = indicatorLine;
        _currentHeight = currentHeight;
        _animateIndicatorLine = true;
        _indicatorAnimationStartTime = Time.time;
    }
    private void AnimateProgressBarButton()
    {
        if (_animateProgressBarButton)
        {
            // Calculate elapsed time
            float elapsedTime = Time.time - _progressBarAnimationStartTime;
            if (elapsedTime < _buttonAnimationDuration)
            {
                // If the elapsed time is less than the set progress bar button animation duration, keep animating the fade in effect
                float newButtonAlpha = Mathf.Lerp(0f, 1.0f, elapsedTime / _buttonAnimationDuration);
                _newColor.a = newButtonAlpha;

                _temporaryImage.color = _oldColor;
                _finalImage.color = _newColor;
            }
            else
            {
                // If the elapsed time has reached the set progress bar button jump button animation duration, ensure progress bar button alpha is 1 and stop animation
                _newColor.a = 1;
                _temporaryImage.color = _oldColor;
                _finalImage.color = _newColor;
                _temporaryImage.gameObject.SetActive(false);
                _animateProgressBarButton = false;
            }
        }
    }
    private void AnimateIndicatorLine()
    {
        // Calculate elapsed time
        float elapsedTime = Time.time - _indicatorAnimationStartTime;
        if (_animateIndicatorLine)
        {
            if (elapsedTime < _indicatorAnimationDuration)
            {
                // If the elapsed time is less than the set indicator line animation duration, keep animating the fade in effect
                float currentWidth = Mathf.Lerp(0f, _targetWidth, elapsedTime / _indicatorAnimationDuration);
                _indicatorLine.progressBarRectTransform.sizeDelta = new Vector2(currentWidth, _currentHeight);
            }
            else
            {
                /* If the elapsed time has reached the set indicator line jump button animation duration, ensure indicator line width is is the target width
                 * and stop animation*/
                _indicatorLine.progressBarRectTransform.sizeDelta = new Vector2(_targetWidth, _currentHeight);
                _animateIndicatorLine = false;
            }
        }
    }
    #endregion
}
