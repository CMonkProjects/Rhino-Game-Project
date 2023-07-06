using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel_HiddenData : StoryPanel_Basic
{
    protected override void DoStoryStuff()
    {
        int enemyDataFound = ScoreManager.instanceScoreManager.dataBonusesFound;

        storyIndex = enemyDataFound;

        base.DoStoryStuff();
    }
}
