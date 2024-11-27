using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewLevelUnlockedScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTypeText;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private Button continueButton;

    public IEnumerator SetNewLevelUnlockedScreen(string levelType)
    {
        continueButton.enabled = false;
        continueText.gameObject.SetActive(false);

        levelTypeText.text = levelType;

        yield return new WaitForSeconds(2);

        continueButton.enabled = true;
        continueText.gameObject.SetActive(true);
    }

    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
    }
}
