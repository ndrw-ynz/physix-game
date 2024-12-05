using UnityEngine;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour
{
    public ScrollRect scrollRect; // Assign your ScrollRect in the Inspector

    private void OnEnable()
    {
        ResetScroll();
    }

    private void ResetScroll()
    {
        if (scrollRect != null)
        {
            // Reset scroll position (to top/left)
            scrollRect.verticalNormalizedPosition = 1; // Top
        }
    }
}
