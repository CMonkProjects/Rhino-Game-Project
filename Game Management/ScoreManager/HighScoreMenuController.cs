using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighScoreMenuController : MonoBehaviour
{
    public List<Scores> highscore;

    public Text nameText, scoreText, deathText, killText, bonusText;
    public List<string> activeMessages = new List<string>();

    public GameObject clearLeaderboardPanel;
    public GameObject scoreboardButtons;

    // Start is called before the first frame update
    void Start()
    {
        highscore = new List<Scores>();
        GetHighScores();
    }

    void GetHighScores()
    {
        foreach (Scores _highscore in HighScoreManager.highscoreInstance.GetHighScore())
        {
            highscore.Add(_highscore);
            NewMessage(_highscore.name, _highscore.score);

        }
    }

    void NewMessage(string _name, int _score)
    {
        Debug.Log("Highscore: " + _name + " " + _score);
        UpdateMessagesUI();
    }

    void UpdateMessagesUI()
    {
        nameText.text = "Name";
        scoreText.text = "Score";
        deathText.text = "Deaths";
        killText.text = "Kills";
        bonusText.text = "Bonus";

        if (highscore == null || highscore.Count == 0)
        {
            return;
        }

        foreach (Scores _highscore in highscore)
        {
            int rankPosition = highscore.IndexOf(_highscore) + 1;

            nameText.text += "\n" + rankPosition + ") " + _highscore.name;
            scoreText.text += "\n" + _highscore.score;
            deathText.text += "\n" + _highscore.deaths;
            killText.text += "\n" + _highscore.kills;
            bonusText.text += "\n" + _highscore.bonus;
        }
    }
    //Buttons------------------------------
    public void Button_ClearLeaderboard()
    {
        HighScoreManager.highscoreInstance.ClearLeaderBoard();
        HighScoreManager.highscoreInstance.SetDefaultScores();
        HighScoreManager.highscoreInstance.GetHighScore();
        UpdateMessagesUI();
    }

    public void Button_ClearLeaderboardPanel(bool _bool)
    {
        clearLeaderboardPanel.SetActive(_bool);
    }

    //Enables or Disables the buttons in the main highscore screen
    public void Scoreboard_Buttons(bool _bool)
    {
        scoreboardButtons.SetActive(_bool);
    }
}
