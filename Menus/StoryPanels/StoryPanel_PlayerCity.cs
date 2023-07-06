using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel_PlayerCity : StoryPanel_Basic
{
    protected override void DoStoryStuff()
    {
        int superweaponFired = ScoreManager.instanceScoreManager.superweaponFired;

        if (superweaponFired >= 2)
        {
            storyIndex = 0; //Bad
        }
        else if (superweaponFired == 1)
        {
            storyIndex = 1; //OK
        }
        else if (superweaponFired == 0)
        {
            storyIndex = 2; //Good
        }

        base.DoStoryStuff();
    }
}
