using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HiScoreUI : MonoBehaviour
{
    //Hiscore
    //public Text text_scoreHiScore;

    //Scene
    public string scene_MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        DisplayHiScore();
    }

    public void DisplayHiScore()
    {
        //text_scoreHiScore.text = "Hiscore: " + HiScoreManager.instanceHiScore.score_highscore;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(scene_MainMenu);
    }
}
