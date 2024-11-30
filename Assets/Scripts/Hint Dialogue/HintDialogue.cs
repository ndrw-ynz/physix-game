using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintDialogue : MonoBehaviour
{
    public static event Action HintDialogueFinished;

    [Header("Dialogue Properties")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private GameObject downArrow;
    public List<string> sentences = new List<string>();

    // Current index of the sentences list
    private int index;

    void OnEnable()
    {
        // Reset index to 0
        index = 0;

        // Deactivate down arrow
        downArrow.SetActive(false);

        // Disable the background button
        backgroundButton.enabled = false;

        // Start typing the sentence
        StartCoroutine(Type());
    }

    void Update()
    {
        // If the sentence is complete, activate down arrow and enable background button to be clicked
        if (dialogueText.text == sentences[index])
        {
            downArrow.SetActive(true);
            backgroundButton.enabled = true;
        }
    }

    public IEnumerator Type()
    {
        // Clear dialogue text
        dialogueText.text = "";

        // Create a typing effect for the current sentence
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void NextSentence()
    {
        // Deactivate down arrow
        downArrow.SetActive(false);

        // Disable button background
        backgroundButton.enabled = false;

        // Check if there's still more sentences to show in dialogue or not
        if (index < sentences.Count - 1)
        {
            index++;
            StartCoroutine(Type());
        }
        else
        {
            Debug.Log("Dialogue is done");
            HintDialogueFinished?.Invoke();
        }
    }
}
