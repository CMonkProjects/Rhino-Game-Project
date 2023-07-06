using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel_CommSignal : StoryPanel_Basic
{
    /*void Start()
    {
        DoStoryStuff();
    }*/

    protected override void DoStoryStuff()
    {
        bool enemySignalSent = ScoreManager.instanceScoreManager.enemyCommCenterSignal;

        switch (enemySignalSent)
        {
            case true:
                storyIndex = 0; //bad
                break;

            case false:
                storyIndex = 1; //good
                break;
        }

        base.DoStoryStuff();
    }
}
