using UnityEngine;

public class HelpfulToolsDisplay : MonoBehaviour
{
    [Header("Locked Background Game Object")]
    [SerializeField] private GameObject lockedBackground;

    public void LockHelpfulTools(Difficulty difficulty)
    {
        if (difficulty == Difficulty.Hard)
        {
            lockedBackground.SetActive(true);
        }
        else
        {
            lockedBackground.SetActive(false);
        }
    }
}
