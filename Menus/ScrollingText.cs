using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//Text prints one character at a time
public class ScrollingText : MonoBehaviour
{
    public Text uiText;

    [Multiline]
    public string textEntered = ""; //default text, printed automatically on enable
    public char[] textEnteredSplit;
    public char[] textToPrintSplit;

    public bool isPrinting = false;
    public float charPrintDelay;

    public string soundEffect;

    string newText = "";    //text that's set manually, called from a delegate

    [SerializeField] StoryPanel_Basic storyTextManager;

    void Awake()
    {
        uiText = GetComponent<Text>();
    }

    void OnEnable()
    {
        //delegate
        if (storyTextManager != null)
        {
            storyTextManager.onWriteText += SetNewText;
        }

        ResetText();    //Reset the text so we can print it again

        textEnteredSplit = textEntered.ToCharArray();   //We're referencing all the letters (chars) in the textEntered string
        textToPrintSplit = new char[textEnteredSplit.Length];   //We split up the text into individual characters so we can print it out

        StartPrintingText();
    }

    void OnDisable()
    {
        //delegate
        if (storyTextManager != null)
        {
            storyTextManager.onWriteText -= SetNewText;
        }

        ResetText();
    }

    public void StartPrintingText()
    {
        StartCoroutine(PrintText());
    }

    IEnumerator PrintText()
    {
        isPrinting = true;

        for (int i = 0; i < textEnteredSplit.Length; i++)
        {
            textToPrintSplit[i] = textEnteredSplit[i];

            string s = new string(textToPrintSplit);
            uiText.text = s;

            GameEvents.current.PlaySound(soundEffect);

            yield return new WaitForSeconds(charPrintDelay);
        }

        isPrinting = false;
    }

    public void ResetText()
    {
        uiText.text = "";
        Array.Clear(textToPrintSplit, 0, textToPrintSplit.Length);
    }

    void SetNewText(string _newText)
    {
        newText = _newText;

        textEnteredSplit = newText.ToCharArray();
        textToPrintSplit = new char[textEnteredSplit.Length];

        StartPrintingText();
    }
}
