using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instanceScoreManager;

    [Header("Misc Stats")]

    public int playerDeaths;
    public int enemyKills, buildingsDestroyed, treesTerminated;

    [Header("Story Based Stats")]

    public bool enemyCommCenterSignal = false;
    public int lrmsFired, aircraftTakenOff, superweaponFired;
    public int dataBonusesFound;
    public int battleScore;

    public int priority = 0;

    void Awake()
    {
        //Singleton
        if (instanceScoreManager == null)
        {
            instanceScoreManager = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onResetScore += ResetScore;

        //Generic Stuff
        GameEvents.current.onEnemyDestroyed += EnemyDestroyed;
        GameEvents.current.onBuildingDestroyed += BuildingDestroyed;
        GameEvents.current.onTreeTerminated += TreeTerminated;

        //Story Stuff
        GameEvents.current.onHiddenBonus += HiddenBonus;
        GameEvents.current.onEnemyCommSignal += EnemyCommCenterSignal;
        GameEvents.current.onLRMNotDestroyed += EnemyLRMFired;
        GameEvents.current.onAircraftNotDestroyed += EnemyAircraftTakenOff;
        GameEvents.current.onSuperweaponFiring += SuperWeaponFired;
        GameEvents.current.onSetBattleScore += GetBattleScore;
    }

    void OnDisable()
    {
        GameEvents.current.onResetScore -= ResetScore;

        //Generic Stuff
        GameEvents.current.onEnemyDestroyed -= EnemyDestroyed;
        GameEvents.current.onBuildingDestroyed -= BuildingDestroyed;
        GameEvents.current.onTreeTerminated -= TreeTerminated;

        //Story Stuff
        GameEvents.current.onHiddenBonus -= HiddenBonus;
        GameEvents.current.onEnemyCommSignal -= EnemyCommCenterSignal;
        GameEvents.current.onLRMNotDestroyed -= EnemyLRMFired;
        GameEvents.current.onAircraftNotDestroyed -= EnemyAircraftTakenOff;
        GameEvents.current.onSuperweaponFiring -= SuperWeaponFired;
        GameEvents.current.onSetBattleScore -= GetBattleScore;
    }

    //Generic stuff

    void EnemyDestroyed()
    {
        enemyKills++;
    }

    void BuildingDestroyed()
    {
        buildingsDestroyed++;
    }

    void TreeTerminated()
    {
        treesTerminated++;
    }

    //Story Stuff

    void HiddenBonus()
    {
        dataBonusesFound++;
    }

    void EnemyCommCenterSignal()
    {
        enemyCommCenterSignal = true;

        GameEvents.current.NewMessage("Enemy sent transmission");
    }

    void EnemyLRMFired()
    {
        lrmsFired++;

        GameEvents.current.NewMessage("Enemy LRM launched");
    }

    void EnemyAircraftTakenOff()
    {
        aircraftTakenOff++;

        GameEvents.current.NewMessage("Enemy aircraft taken off");
    }

    void SuperWeaponFired()
    {
        superweaponFired++;

        GameEvents.current.NewMessage("Enemy Superweapon fired");
    }

    void GetBattleScore(int _battleScore)
    {
        battleScore = _battleScore;

        Debug.Log("BATTLE SCORE IS NOW - " + battleScore);
    }

    //Reset Score
    public void ResetScore()
    {
        Destroy(gameObject);
        /*playerDeaths = 0;
        enemyKills = 0;
        buildingsDestroyed = 0;
        treesTerminated = 0;
        dataBonusesFound = 0;
        enemyCommCenterSignal = false;
        lrmsFired = 0;
        aircraftTakenOff = 0;
        battleScore = 0;*/
    }
}
