using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager highscoreInstance;
    const int leaderboardLength = 5;

    public List<Scores> defaultScores;

    public string playerName = "Player";

    //Constructor automatically fires at start of scene
    public static HighScoreManager Instance
    {
        get
        {
            //Create an instance of our highscoremanager if one doesn't already exist
            if (highscoreInstance == null)
            {
                highscoreInstance = new GameObject("HighScoreManager").AddComponent<HighScoreManager>();
            }

            return highscoreInstance;
        }
    }

    //Singleton
    void Awake()
    {
        if (highscoreInstance == null)
        {
            highscoreInstance = this;
        }
        else if (highscoreInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GetHighScore();

        SetDefaultScores(); //Testing purposes

        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        //GameEvents.current.onSetPlayerName += SetPlayerName;
    }

    void OnDisable()
    {
        //GameEvents.current.onSetPlayerName -= SetPlayerName;
    }

    void SetPlayerName(string _playerName)
    {
        playerName = _playerName;
    }

    public void SaveHighScore(string defaultName, int score, int deaths, int kills, int bonus)
    {
        List<Scores> highScores = new List<Scores>();

        int i = 1;

        while (i <= leaderboardLength && PlayerPrefs.HasKey("Highscore" + i + "score"))
        {
            Scores temp = new Scores();

            temp.score = PlayerPrefs.GetInt("Highscore" + i + "score");
            temp.name = PlayerPrefs.GetString("Highscore" + i + "name");
            temp.deaths = PlayerPrefs.GetInt("Highscore" + i + "deaths");
            temp.kills = PlayerPrefs.GetInt("Highscore" + i + "kills");
            temp.bonus = PlayerPrefs.GetInt("Highscore" + i + "bonus");
            highScores.Add(temp);
            i++;
        }
        if (highScores.Count == 0)
        {
            Scores temp = new Scores();
            temp.score = score;
            if (defaultName != null)
            {
                temp.name = defaultName;
            }
            else temp.name = playerName;
            temp.deaths = deaths;
            temp.kills = kills;
            temp.bonus = bonus;
            highScores.Add(temp);
        }
        else
        {
            for (i = 1; i <= highScores.Count && i <= leaderboardLength; i++)
            {
                if (score > highScores[i - 1].score)
                {
                    Scores _temp = new Scores();
                    _temp.score = score;
                    if (defaultName != null)
                    {
                        _temp.name = defaultName;
                    }
                    else _temp.name = playerName;
                    _temp.deaths = deaths;
                    _temp.kills = kills;
                    _temp.bonus = bonus;
                    highScores.Insert(i - 1, _temp);
                    break;
                }
                if (i == highScores.Count && i < leaderboardLength)
                {
                    Scores _temp = new Scores();
                    if (defaultName != null)
                    {
                        _temp.name = defaultName;
                    }
                    else _temp.name = playerName;
                    _temp.score = score;
                    _temp.deaths = deaths;
                    _temp.kills = kills;
                    _temp.bonus = bonus;
                    highScores.Add(_temp);
                    break;
                }
            }
        }

        i = 1;
        while(i <= leaderboardLength && i <= highScores.Count)
        {
            PlayerPrefs.SetString("Highscore" + i + "name", highScores[i - 1].name);
            PlayerPrefs.SetInt("Highscore" + i + "score", highScores[i - 1].score);
            PlayerPrefs.SetInt("Highscore" + i + "deaths", highScores[i - 1].deaths);
            PlayerPrefs.SetInt("Highscore" + i + "kills", highScores[i - 1].kills);
            PlayerPrefs.SetInt("Highscore" + i + "bonus", highScores[i - 1].bonus);
            i++;
        }

        PlayerPrefs.Save();
    }

    public List<Scores> GetHighScore()
    {
        List<Scores> highScores = new List<Scores>();

        int i = 1;
        while (i <= leaderboardLength && PlayerPrefs.HasKey("Highscore" + i + "score"))
        {
            Scores temp = new Scores();
            temp.score = PlayerPrefs.GetInt("Highscore" + i + "score");
            temp.name = PlayerPrefs.GetString("Highscore" + i + "name");
            temp.deaths = PlayerPrefs.GetInt("Highscore" + i + "deaths");
            temp.kills = PlayerPrefs.GetInt("Highscore" + i + "kills");
            temp.bonus = PlayerPrefs.GetInt("Highscore" + i + "bonus");
            highScores.Add(temp);
            i++;
        }

        return highScores;
    }

    //Testing purposes
    public void SetDefaultScores()
    {
        List<Scores> _defaultScores = new List<Scores>();

        List<Scores> actualHighScores = GetHighScore();

        foreach (Scores defaultScore in defaultScores)
        {
            _defaultScores.Add(defaultScore);
        }

        //Set these default scores only if our leaderboard is empty
        if (actualHighScores.Count == 0)
        {
            int i = 1;
            while (i <= leaderboardLength && i <= _defaultScores.Count)
            {
                SaveHighScore(_defaultScores[i - 1].name, _defaultScores[i - 1].score, _defaultScores[i - 1].deaths, _defaultScores[i - 1].kills, _defaultScores[i - 1].bonus);
                i++;
            }
        }
    }

    public void ClearLeaderBoard()
    {
        List<Scores> HighScores = GetHighScore();

        for (int i = 1; i <= HighScores.Count; i++)
        {
            PlayerPrefs.DeleteKey("Highscore" + i + "name");
            PlayerPrefs.DeleteKey("Highscore" + i + "score");
            PlayerPrefs.DeleteKey("Highscore" + i + "deaths");
            PlayerPrefs.DeleteKey("Highscore" + i + "kills");
            PlayerPrefs.DeleteKey("Highscore" + i + "bonus");
        }
    }

    //Called by GameEvent in the main menu
    public void EnterPlayerName(string _playerName)
    {
        playerName = _playerName;

        Debug.Log("Player Name is now: " + playerName);
    }
}

[System.Serializable]
public class Scores
{
    public int score;
    public string name;
    public int deaths;
    public int kills;
    public int bonus;
}