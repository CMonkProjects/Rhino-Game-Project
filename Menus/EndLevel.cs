using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    public GameObject EndLevel_UI;
    public string endScene;
    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        GameEvents.current.onLevelEnd += FinishLevel;
    }

    private void OnDestroy()
    {
        GameEvents.current.onLevelEnd -= FinishLevel;
    }

    public void FinishLevel()
    {
        EndLevel_UI.SetActive(true);
        GameController.gameIsActive = false;
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(endScene);
    }
}
