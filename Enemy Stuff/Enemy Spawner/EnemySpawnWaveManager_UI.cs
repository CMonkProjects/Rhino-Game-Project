using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Self contained UI for enemy waves; has a timer and message display
public class EnemySpawnWaveManager_UI : MonoBehaviour
{
    public GameObject ui_Panel;
    public Text waveMessageText;
    public Text currentWaveText;
    public Text waveTimerText;
    public Text enemyCountText;

    public string currentWaveString;

    [SerializeField] float messageTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        waveMessageText.text = "";
        currentWaveText.text = currentWaveString + EnemySpawnWaveManager.currentWave;
        waveTimerText.text = "";
        enemyCountText.text = "Enemies Left - " + EnemySpawnWaveManager.enemyCount;
    }

    public void DisplayWaveMessage(string _message)
    {
        waveMessageText.text = "";

        waveMessageText.text = _message;

        StartCoroutine(EndWaveMessage(waveMessageText));
    }

    IEnumerator EndWaveMessage(Text _text)
    {
        yield return new WaitForSeconds(messageTime);

        _text.text = "";
    }

    public void UpdateEnemyCount()
    {
        enemyCountText.text = "Enemies Left - " + EnemySpawnWaveManager.enemyCount;
    }

    public void EndEnemyCount()
    {
        StartCoroutine(EndWaveMessage(enemyCountText));
    }

    public void UpdateCurrentWave()
    {
        //display current wave
        currentWaveText.text = currentWaveString + (EnemySpawnWaveManager.currentWave + 1);

        StartCoroutine(EndWaveMessage(currentWaveText));
    }
}
