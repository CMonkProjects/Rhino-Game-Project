using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOver_UI;

    public string mainMenuScene = "Scene_MainMenu";
    public string hiscoreScene = "Scene_Hiscores";

    public string soundEffectText = "";

    public Transform texts;
    public Text enemyKillsText, buildingsDestroyedText, bonusesText;

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onGameOver += GameOver;
    }

    void OnDestroy()
    {
        GameEvents.current.onGameOver -= GameOver;
    }

    public void GameOver()
    {
        GameOver_UI.SetActive(true);
        GameController.gameIsActive = false;
        EndLevel();
    }

    public void QuitToMenu()
    {
        GameOver_UI.SetActive(false);
        GameController.gameIsActive = true;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Retry()
    {
        GameOver_UI.SetActive(false);
        GameController.gameIsActive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Borrowed from SummaryMenu script
    public void EndLevel()
    {
        UpdateTexts();
        //Start Coroutine
        StartCoroutine(DisplaySummary());
        GameController.gameIsActive = false;
    }

    void UpdateTexts()  //called by EndLevel
    {
        enemyKillsText.text = "Enemy Kills: " + ScoreManager.instanceScoreManager.enemyKills;
        buildingsDestroyedText.text = "Buildings Destroyed: " + ScoreManager.instanceScoreManager.buildingsDestroyed;
        //bonusesText.text = "Hidden Bonuses Found: " + ScoreManager.instanceScoreManager.dataBonusesFound;
    }

    IEnumerator DisplaySummary()
    {
        //Foreach loop activates each player stat in the summary
        foreach (Transform child in texts)
        {
            yield return new WaitForSeconds(0.5f);
            GameEvents.current.PlaySound(soundEffectText);
            child.gameObject.SetActive(true);
        }
    }
}
