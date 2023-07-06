using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI, gameMenuUI, controlsUI, highscoreUI, enterPlayerNameUI;
    public List<GameObject> briefingPage;
    int briefingPageIndex = 0;
    MouseCursor mouseCursor;

    public string firstLevelScene; //first level
    public string hiscoreScene;

    public string soundEffect_ButtonMouseOver, soundEffect_ButtonPress, soundEffect_IntroSound, soundEffect_StartNewGame, soundEffect_Quit, soundEffect_LoopingNoise;

    public string nameOfPlayer;
    public Text inputText;

    // Start is called before the first frame update
    void Start()
    {
        mouseCursor = GetComponent<MouseCursor>();
        mouseCursor.SetMenuCursor();
        mouseCursor.MouseVisibility(true);
    }

    //Main Page
    public void MainMenuPage(bool _bool)
    {
        mainMenuUI.SetActive(_bool);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    //Control Page
    public void OptionsPage(bool _bool)
    {
        controlsUI.SetActive(_bool);
    }

    //High Score Page
    /*public void HiScorePage(bool _bool)
    {
        highscoreUI.SetActive(_bool);
    }*/

    //Briefing Page
    public void BriefingPage(bool _bool)
    {
        gameMenuUI.SetActive(_bool);
    }
    public void StartNewGame()
    {
        PlaySound(soundEffect_StartNewGame);
        Invoke("LoadScene", 1f);
    }
    void LoadScene()
    {
        SceneManager.LoadScene(firstLevelScene);
    }
    public void LoadHiScoreScene()
    {
        SceneManager.LoadScene(hiscoreScene);
    }

    /*public void PlayerNamePage(bool _bool)
    {
        enterPlayerNameUI.SetActive(_bool);
    }*/

    public void NextBriefingPage()
    {
        briefingPage[briefingPageIndex].SetActive(false);

        //Count briefing index up by one
        briefingPageIndex++;

        if (briefingPageIndex > briefingPage.Count - 1)
            briefingPageIndex = briefingPage.Count - 1;

        briefingPage[briefingPageIndex].SetActive(true);
    }

    public void ResetBriefingPage()
    {
        briefingPage[briefingPageIndex].SetActive(false);

        briefingPageIndex = 0;

        briefingPage[briefingPageIndex].SetActive(true);
    }

    //Button Events
    public void ButtonMouseOver()
    {
        PlaySound(soundEffect_ButtonMouseOver);
    }

    public void ButtonPress()
    {
        PlaySound(soundEffect_ButtonPress);
    }

    public void PlaySound(string _soundEffectName)
    {
        GameEvents.current.PlaySound(_soundEffectName);
    }

    /*public void SetPlayerName()
    {
        nameOfPlayer = inputText.text;
        GameEvents.current.SetPlayerName(nameOfPlayer);
    }*/
}
