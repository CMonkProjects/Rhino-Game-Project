using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel_WarOutcome : StoryPanel_Basic
{
    protected override void DoStoryStuff()
    {
        int battleScore = ScoreManager.instanceScoreManager.battleScore;

        int superweaponFired = ScoreManager.instanceScoreManager.superweaponFired;

        if (superweaponFired >= 2)
            superweaponFired = 1;

        //story index 0 - 5

        switch (superweaponFired)
        {
            //Superweapon not fired
            case 0:

                //pretty bad - 0 stalemate / white peace
                if (battleScore == 0 || battleScore == 3)
                {
                    storyIndex = 0;
                }

                //good 1 enemy city taken after some heavy fighting
                else if (battleScore == 1 || battleScore == 4)
                {
                    storyIndex = 1;
                }

                //best 2 enemy city surrendered and taken
                else if (battleScore == 2 || battleScore == 5)
                {
                    storyIndex = 2;
                }

                break;

            //Superweapon fired
            case 1:

                //worst 0 lost war, friendly forces retreat
                if (battleScore == 0 || battleScore == 3)
                {
                    storyIndex = 3;
                }

                //bad 1 stalemate / white peace, enemy city bombed and shelled but not taken
                else if (battleScore == 1 || battleScore == 4)
                {
                    storyIndex = 4;
                }

                //not very good 2 enemy city taken, enemy city heavily damaged in revenge
                else if (battleScore == 2 || battleScore == 5)
                {
                    storyIndex = 5;
                }

                break;
        }

        base.DoStoryStuff();
    }
}
