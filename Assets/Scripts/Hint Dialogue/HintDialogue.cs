using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private GameObject downArrowButton;
    public List<string> sentences = new List<string>();
    private int index;

    void OnEnable()
    {
        // Reset index to 0
        index = 0;
        downArrowButton.SetActive(false);
        backgroundButton.enabled = false;

        sentences = new List<string>()
        {
            "Hello Charlie Chaplin DSPKAJDIOPJASIOD ASJDIOJASIP DOPIASJDP ASPDASPJDJSAPDJAPSO DJPOASJP",
            "Hi Dyan Morena  Charlie Chaplin DSPKAJDIOPJASIOD ASJDIOJASIP DOPIASJDP ASPDASPJDJSAPDJAPSO DJPOASJP",
            "Hi Dyan Morena  Charlie Chaplin DSPKAJDIOPJASIOD ASJDIOJASIP DOPIASJDP ASPDASPJDJSAPDJAPSO DJPOASJP",
            "Hi Dyan Morena  Charlie Chaplin DSPKAJDIOPJASIOD ASJDIOJASIP DOPIASJDP ASPDASPJDJSAPDJAPSO DJPOASJP",
            "Hi Dyan Morena  Charlie Chaplin DSPKAJDIOPJASIOD ASJDIOJASIP DOPIASJDP ASPDASPJDJSAPDJAPSO DJPOASJP",
            "Hi Dyan Morena  Charlie Chaplin DSPKAJDIOPJASIOD ASJDIOJASIP DOPIASJDP ASPDASPJDJSAPDJAPSO DJPOASJP"
        };
        StartCoroutine(Type());
    }

    void Update()
    {
        if (dialogueText.text == sentences[index])
        {
            downArrowButton.SetActive(true);
            backgroundButton.enabled = true;
        }
    }

    public IEnumerator Type()
    {
        dialogueText.text = "";

        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void NextSentence()
    {
        downArrowButton.SetActive(false);
        backgroundButton.enabled = false;

        if (index < sentences.Count - 1)
        {
            index++;
            StartCoroutine(Type());
        }
        else
        {
            Debug.Log("Dialogue is done");
            // Invoke dialogue done here
        }
    }
}
