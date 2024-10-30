using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image titleScreenButtonImage;
    [SerializeField] private TextMeshProUGUI titleScreenButtonText;

    private void OnDisable()
    {
        titleScreenButtonImage.gameObject.SetActive(false);
        titleScreenButtonText.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        titleScreenButtonImage.gameObject.SetActive(true);
        titleScreenButtonText.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        titleScreenButtonImage.gameObject.SetActive(false);
        titleScreenButtonText.color = Color.white;
    }
}
