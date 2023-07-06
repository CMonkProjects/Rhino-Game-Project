using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel_SiegeBattle : StoryPanel_Basic
{
    protected override void DoStoryStuff()
    {
        int enemyLRMs = ScoreManager.instanceScoreManager.lrmsFired;

        if (enemyLRMs >= 4)
            enemyLRMs = 4;

        int enemyAircraft = ScoreManager.instanceScoreManager.aircraftTakenOff;

        if (enemyAircraft >= 4)
            enemyAircraft = 4;

        int enemyBattleSupport = enemyLRMs + enemyAircraft;

        bool enemyGroundCoordination = ScoreManager.instanceScoreManager.enemyCommCenterSignal;

        //story index 0 - 5

        switch (enemyGroundCoordination)
        {
            case true:
                //Enemy coordinated

                //bad 0
                if (enemyBattleSupport >= 7)
                {
                    storyIndex = 0;
                }

                //ok 1
                else if (enemyBattleSupport >= 4 && enemyBattleSupport <= 6)
                {
                    storyIndex = 1;
                }

                //good 2
                else if (enemyBattleSupport <= 3)
                {
                    storyIndex = 2;
                }

                break;

            case false:
                //Enemy is uncoordinated

                //ok 3
                if (enemyBattleSupport >= 7)
                {
                    storyIndex = 3;
                }

                //great 4
                else if (enemyBattleSupport >= 4 && enemyBattleSupport <= 6)
                {
                    storyIndex = 4;
                }

                //best 5
                else if (enemyBattleSupport <= 3)
                {
                    storyIndex = 5;
                }

                break;
        }

        ScoreManager.instanceScoreManager.battleScore = storyIndex;

        Debug.Log("BATTLE SCORE IS NOW - " + ScoreManager.instanceScoreManager.battleScore);

        base.DoStoryStuff();
    }
}
