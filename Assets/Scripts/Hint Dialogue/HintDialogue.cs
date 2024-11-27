using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class HintDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject downArrowButton;

    private int index;



    // Start is called before the first frame update
    void Start()
    {
        List<string> test = new List<string>()
        {
            "Hello Charlie Chaplin",
            "Hi Dyan Morena"
        };
        StartCoroutine(Type(test));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Type(List<string> sentences)
    {
        dialogueText.text = "";

        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
