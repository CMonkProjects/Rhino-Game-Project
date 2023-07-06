using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel_Basic : MonoBehaviour
{
    //list the possible outcome strings
    [SerializeField] protected List<string> storyText = new List<string>();

    //list the gameobjects to activate for each outcome
    [SerializeField] protected List<GameObject> storyObjects = new List<GameObject>();

    [SerializeField] protected int storyIndex = 0;

    public delegate void OnWriteText(string textToWrite);
    public OnWriteText onWriteText;

    public delegate void OnActivateStoryObject();
    public OnActivateStoryObject onActivateStoryObject;

    protected virtual void OnEnable()
    {
        DoStoryStuff();
    }

    //function called when enabled
    protected virtual void DoStoryStuff()
    {
        Invoke("WriteText", 0.1f);
        Invoke("ActivateStoryObject", 0.2f);
    }

    //calls a delegate
    protected virtual void WriteText()
    {
        if (onWriteText != null)
        {
            onWriteText(storyText[storyIndex]);
        }
    }

    protected virtual void ActivateStoryObject()
    {
        if (onActivateStoryObject != null)
        {
            onActivateStoryObject();
        }

        //test
        storyObjects[storyIndex].SetActive(true);
    }
}
