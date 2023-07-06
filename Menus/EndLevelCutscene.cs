using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelCutscene : MonoBehaviour
{
    public string summaryScene;

    public GameObject endLevel_Panel;

    private void Start()
    {
        StartCoroutine(DisplayEndLevelPanel());
    }

    IEnumerator DisplayEndLevelPanel()
    {
        yield return new WaitForSeconds(0.2f);

        endLevel_Panel.SetActive(true);
    }
}
