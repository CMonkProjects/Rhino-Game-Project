using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        //Singleton
        if (current == null)
        {
            current = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("GameEvent object destroyed due to singleton");
            return;
        }
    }

    //Player Event Stuff---------------------------
    public event Action onStartNewGame;
    public void StartNewGame()
    {
        onStartNewGame();
    }

    public event Action onPlayerDied;
    public void PlayerDied()
    {
        onPlayerDied();
    }

    public event Action onPlayerRespawn;
    public void PlayerRespawn()
    {
        onPlayerRespawn();
    }

    public event Action<int> onUpdateScore;
    public void UpdateScore(int _scoreAdded)
    {
        onUpdateScore(_scoreAdded);
    }

    public event Action<int> onUpdateLives;
    public void ChangeLives(int _livesChanged)
    {
        onUpdateLives(_livesChanged);
    }

    public event Action onPlayerHealthChanged;
    public void PlayerHealthChanged()
    {
        if (onPlayerHealthChanged != null)
        {
            onPlayerHealthChanged();
        }
    }

    public event Action onEnemyDestroyed;
    public void EnemyDestroyed()
    {
        onEnemyDestroyed();
    }

    public event Action onBuildingDestroyed;
    public void BuildingDestroyed()
    {
        onBuildingDestroyed();
    }

    public event Action onTreeTerminated;
    public void TreeTerminated()
    {
        if (onTreeTerminated != null)
        {
            onTreeTerminated();
        }
    }

    public event Action<float> onPlayerDamaged;
    public void PlayerDamaged(float _damage)
    {
        onPlayerDamaged(_damage);
    }

    public event Action onPlayerLowHealth;
    public void PlayerLowHealth()
    {
        onPlayerLowHealth();
    }

    /*//Weapon Selected Event
    public event Action<Texture2D> onWeaponSelected;
    public void WeaponSelected(Texture2D _newCrosshair)
    {
        onWeaponSelected(_newCrosshair);
    }*/

    //Screen Shake
    public event Action<string> onShakeScreen;
    public void ShakeScreen(string shakeTrigger)
    {
        onShakeScreen(shakeTrigger);
    }

    //Game Management Events
    public event Action onLevelEnd;
    public void LevelEnd()
    {
        onLevelEnd();
    }

    public event Action<bool> onPauseMenu;
    public void GamePauseMenu(bool _paused)
    {
        onPauseMenu(_paused);
    }

    public event Action onGameOver;
    public void GameOver()
    {
        onGameOver();
    }

    public event Action onRespawnPlayer;
    public void RespawnPlayer()
    {
        onRespawnPlayer();
    }

    public event Action onPlayerSpawned;
    public void PlayerSpawned()
    {
        onPlayerSpawned();
    }

    public event Action<int> onCheckpointTrigger;
    public void CheckpointTrigger(int id)
    {
        if (onCheckpointTrigger != null)
        {
            onCheckpointTrigger(id);    //calls a specific checkpoint by its id number
        }
    }

    public event Action<string> onMessage;
    public void NewMessage(string _newMessage)
    {
        if (onMessage != null)
        {
            onMessage(_newMessage);
        }
    }

    public event Action onHiddenBonus;
    public void HiddenBonus()
    {
        if (onHiddenBonus != null)
        {
            onHiddenBonus();
        }
    }

    public event Action onEnemyRhinoNear;
    public void EnemyRhinoNear()
    {
        onEnemyRhinoNear();
    }

    public event Action onEnemySuicideNear;
    public void EnemySuicideNear()
    {
        onEnemySuicideNear();
    }

    //Generic Stuff--------------------------------
    public event Action<int> onDoorwayTriggerEnter;
    public void DoorwayTriggerEnter(int id) //calls a specific door by its id number
    {
        if (onDoorwayTriggerEnter != null)
        {
            onDoorwayTriggerEnter(id);
        }
    }

    public event Action<int> onDoorwayTriggerExit;
    public void DoorwayTriggerExit(int id)
    {
        if (onDoorwayTriggerExit != null)
        {
            onDoorwayTriggerExit(id);
        }
    }

    public event Action<string> onCeilingTriggerEnter;
    public void CeilingTriggerEnter(string id)
    {
        if (onCeilingTriggerEnter != null)
        {
            onCeilingTriggerEnter(id);
        }
    }

    public event Action<string> onCeilingTriggerExit;
    public void CeilingTriggerExit(string id)
    {
        if (onCeilingTriggerExit != null)
        {
            onCeilingTriggerExit(id);
        }
    }

    public event Action<string> onPlayerLeavingArea;

    public void PlayerLeavingArea(string id)
    {
        if (onPlayerLeavingArea != null)
        {
            onPlayerLeavingArea(id);
        }
    }

    //Objective Stuff-------------------------------

    //For objectives like destroying certain objectives, collecting items, reaching destinations, etc.

    //This is for testing purposes
    public event Action onCollectItem;
    public void CollectedItem()
    {
        if (onCollectItem != null)
        {
            onCollectItem();
        }
    }

    public event Action<string> onCollectObjectiveItem;
    public void CollectedObjectiveItem(string _objItem)
    {
        if (onCollectObjectiveItem != null)
        {
            onCollectObjectiveItem(_objItem);
        }
    }

    public event Action<string> onTargetObjectiveDestroyed;
    public void TargetObjectiveDestroyed(string _objTarget)
    {
        if (onTargetObjectiveDestroyed != null)
        {
            onTargetObjectiveDestroyed(_objTarget);
        }
    }

    public event Action<string> onTargetNotDestroyed;    //generic event for failing objectives
    public void TargetNotDestroyed(string _objTargetFail)
    {
        if (onTargetNotDestroyed != null)
        {
            onTargetNotDestroyed(_objTargetFail);
        }
    }

    public event Action<string> onDestinationReached;
    public void DestinationReached(string _objTrigger)
    {
        if (onDestinationReached != null)
        {
            onDestinationReached(_objTrigger);
        }
    }

    //Events such as new objectives, all objectives completed, etc.

    public event Action<string> onObjectiveCompleted;
    public void ObjectiveCompleted(string _message)
    {
        if (onObjectiveCompleted != null)
        {
            onObjectiveCompleted(_message);
        }
    }

    public event Action onNewObjective;
    public void NewObjective()
    {
        if (onNewObjective != null)
        {
            onNewObjective();
        }
    }

    public event Action<int> onNewObjectiveGroup;
    public void NewObjectiveGroup(int _objectiveGroup)
    {
        if (onNewObjectiveGroup != null)
        {
            onNewObjectiveGroup(_objectiveGroup);
        }
    }

    public event Action<int> onAllObjectivesCompleted;
    public void AllObjectivesCompleted(int _objectiveGroup)
    {
        if (onAllObjectivesCompleted != null)
        {
            onAllObjectivesCompleted(_objectiveGroup);
        }
    }

    public event Action onObjectiveUpdated;
    public void ObjectiveUpdated()
    {
        if (onObjectiveUpdated != null)
        {
            onObjectiveUpdated();
        }
    }

    //SoundManager----------------------------------
    public event Action<string> onPlaySound;
    public void PlaySound(string sound)
    {
        if (onPlaySound != null)
        {
            onPlaySound(sound);
        }
    }

    public event Action<string> onStopSound;
    public void StopSound(string sound)
    {
        if (onStopSound != null)
        {
            onStopSound(sound);
        }
    }

    //Called by the volume slider
    public event Action<float> onSetVolume;
    public void SetVolume(float _newVolume)
    {
        if (onSetVolume != null)
        {
            onSetVolume(_newVolume);
        }
    }

    public event Action<string, int> onPlaySoundMultipleTimes;
    public void PlaySoundMultipleTimes(string sound, int numberOfTimes)
    {
        onPlaySoundMultipleTimes(sound, numberOfTimes);
    }

    //ScoreManager----------------------------------
    public event Action<int, int, int, int> onSetGameScore;
    public void SetGameScore(int _deaths, int _enemyKills, int _buildingsDestroyed, int _bonusesFound)
    {
        if (onSetGameScore != null)
        {
            onSetGameScore(_deaths, _enemyKills, _buildingsDestroyed, _bonusesFound);
        }
    }

    public event Action onResetScore;
    public void ResetScore()
    {
        if (onResetScore != null)
        {
            onResetScore();
        }
    }

    public event Action<int> onSetBattleScore;
    public void SetBattleScore(int _battleScore)
    {
        if (onSetBattleScore != null)
        {
            onSetBattleScore(_battleScore);
        }
    }

    //ScoreManager - Story Based Stuff----------------------------------
    public event Action onEnemyCommSignal;
    public void EnemyCommSignal()
    {
        if (onEnemyCommSignal != null)
        {
            onEnemyCommSignal();
        }
    }

    public event Action onLRMLaunched;
    public void LRMLaunched()
    {
        if (onLRMLaunched != null)
        {
            onLRMLaunched();
        }
    }

    public event Action onLRMNotDestroyed;  //Player failed to destroy LRM
    public void LRMNotDestroyed()
    {
        if (onLRMNotDestroyed != null)
        {
            onLRMNotDestroyed();
        }
    }

    public event Action onAircraftNotDestroyed;
    public void AircraftNotDestroyed()
    {
        if (onAircraftNotDestroyed != null)
        {
            onAircraftNotDestroyed();
        }
    }

    //Superweapon Stuff----------------------------------
    public event Action onGeneratorDestroyed;
    public void GeneratorDestroyed()
    {
        if (onGeneratorDestroyed != null)
        {
            onGeneratorDestroyed();
        }
    }

    public event Action onSuperweaponFiring;
    public void SuperweaponFiring()
    {
        if (onSuperweaponFiring != null)
        {
            onSuperweaponFiring();
        }
    }

    public event Action onAllGeneratorsDestroyed;
    public void AllGeneratorsDestroyed()
    {
        if (onAllGeneratorsDestroyed != null)
        {
            onAllGeneratorsDestroyed();
        }
    }

    public event Action onPrimaryGeneratorOnline;
    public void PrimaryGeneratorOnline()
    {
        if (onPrimaryGeneratorOnline != null)
        {
            onPrimaryGeneratorOnline();
        }
    }

    public event Action onPrimaryGeneratorDestroyed;
    public void PrimaryGeneratorDestroyed()
    {
        if (onPrimaryGeneratorDestroyed != null)
        {
            onPrimaryGeneratorDestroyed();
        }
    }

    /*public event Action<int> onSetHiScore;
    public void SetHiScore(int _hiScore)
    {
        if (onSetHiScore != null)
        {
            onSetHiScore(_hiScore);
        }
    }*/

    /*public event Action<string> onSetPlayerName;
    public void SetPlayerName(string _playerName)
    {
        if (onSetPlayerName != null)
        {
            onSetPlayerName(_playerName);
        }
    }*/
}