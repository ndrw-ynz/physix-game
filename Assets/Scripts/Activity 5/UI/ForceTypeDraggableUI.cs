using UnityEngine;
using TMPro;

public class ForceTypeDraggableUI : DraggableUIObject<ForceTypeDraggableUI>
{
    public ForceType contactForceType;
    [SerializeField] private TextMeshProUGUI subscriptText;

    public void SetSubscriptText(char subscript)
    {
        subscriptText.text = $"{subscript}";
    }
}