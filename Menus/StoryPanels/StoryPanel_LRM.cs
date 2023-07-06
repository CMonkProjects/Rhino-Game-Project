using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel_LRM : StoryPanel_Basic
{
    protected override void DoStoryStuff()
    {
        int lrmsFired = ScoreManager.instanceScoreManager.lrmsFired;

        if (lrmsFired >= 4)
        {
            storyIndex = 0; //Bad
        }
        else if (lrmsFired <= 3 && lrmsFired >= 2)
        {
            storyIndex = 1; //OK
        }
        else if (lrmsFired <= 2 && lrmsFired == 1)
        {
            storyIndex = 2; //Good
        }
        else if (lrmsFired == 0)
        {
            storyIndex = 3; //Great
        }

        base.DoStoryStuff();
    }
}
