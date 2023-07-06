using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel_Aircraft : StoryPanel_Basic
{
    protected override void DoStoryStuff()
    {
        int aircraftTakenOff = ScoreManager.instanceScoreManager.aircraftTakenOff;

        if (aircraftTakenOff >= 5)
        {
            storyIndex = 0; //Bad
        }
        else if (aircraftTakenOff <= 4 && aircraftTakenOff >= 3)
        {
            storyIndex = 1; //OK
        }
        else if (aircraftTakenOff <= 2 && aircraftTakenOff >= 1)
        {
            storyIndex = 2; //Good
        }
        else if (aircraftTakenOff == 0)
        {
            storyIndex = 3; //Great
        }

        base.DoStoryStuff();
    }
}
