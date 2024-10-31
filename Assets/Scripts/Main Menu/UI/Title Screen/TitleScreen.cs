using TMPro;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fullName;
    [SerializeField] TextMeshProUGUI section;

    public void SetUserProfile(string studFirstName, string studLastName, string studSection)
    {
        fullName.text = studFirstName + " " + studLastName;
        section.text = "Section " + studSection;
    }
}
