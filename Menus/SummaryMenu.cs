using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SummaryMenu : MonoBehaviour
{
    public GameObject Summary_UI;

    public string soundEffectText = "";
    public string victoryMusic = "VictoryMusic";

    public string mainMenuScene;
    public string hiScoreScene;

    public Transform texts;
    public Text deathsText, enemyKillsText, bonusesText;

    // Start is called before the first frame update
    void Start()
    {
        EndLevel();
    }

    void UpdateTexts()  //called by EndLevel
    {
        deathsText.text = "Deaths: " + ScoreManager.instanceScoreManager.playerDeaths;
        enemyKillsText.text = "Enemy Kills: " + ScoreManager.instanceScoreManager.enemyKills;
        //bonusesText.text = "Hidden Bonuses Found: " + ScoreManager.instanceScoreManager.dataBonusesFound;
    }

    public void EndLevel()
    {
        Summary_UI.SetActive(true);
        UpdateTexts();
        //Start Coroutine
        StartCoroutine(DisplaySummary());
    }
    //Coroutine displays, in succession, the player's stats for the level 
    IEnumerator DisplaySummary()
    {
        GameEvents.current.PlaySound(victoryMusic);
        //Foreach loop activates each player stat in the summary
        foreach(Transform child in texts)
        {
            yield return new WaitForSeconds(0.5f);
            GameEvents.current.PlaySound(soundEffectText);
            child.gameObject.SetActive(true);
        }
        
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(hiScoreScene);
    }
}
