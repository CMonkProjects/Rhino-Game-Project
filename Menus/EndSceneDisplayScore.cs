using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneDisplayScore : MonoBehaviour
{
    public Text enemyKillsText, buildingsDestroyedText, hiddenBonusesText, treesTerminatedText;
    public Text enemyKillsTextScore, buildingsDestroyedTextScore, hiddenBonusesTextScore, treesTerminatedTextScore;

    [SerializeField] string enemyKillsString;
    [SerializeField] string buildingsDestroyedString;
    [SerializeField] string hiddenBonusesString;
    [SerializeField] string treesTerminatedString;

    public string mainMenuScene;

    // Start is called before the first frame update
    void Start()
    {
        InitializeTexts();
    }

    void InitializeTexts()
    {
        int enemyKills = 0;
        int buildingsDestroyed = 0;
        int hiddenBonuses = 0;
        int treesTerminated = 0;

        if (ScoreManager.instanceScoreManager)
        {
            enemyKills = ScoreManager.instanceScoreManager.enemyKills;
            buildingsDestroyed = ScoreManager.instanceScoreManager.buildingsDestroyed;
            hiddenBonuses = ScoreManager.instanceScoreManager.dataBonusesFound;
            treesTerminated = ScoreManager.instanceScoreManager.treesTerminated;

            Debug.LogError("FOUND SCORE MANAGER");
        }
        else Debug.LogError("SCORE MANAGER NOT FOUND");

        enemyKillsText.text = enemyKillsString;
        buildingsDestroyedText.text = buildingsDestroyedString;
        hiddenBonusesText.text = hiddenBonusesString;
        treesTerminatedText.text = treesTerminatedString;

        enemyKillsTextScore.text = "" + enemyKills;
        buildingsDestroyedTextScore.text = "" + buildingsDestroyed;
        hiddenBonusesTextScore.text = "" + hiddenBonuses;
        treesTerminatedTextScore.text = "" + treesTerminated;
    }

    public void ReturnToMainMenu()
    {
        GameEvents.current.ResetScore();

        SceneManager.LoadScene(mainMenuScene);
    }
}
