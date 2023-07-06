using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public GameObject playerPrefab;
    internal GameObject player;  //reference to the active player object

    public int checkpointID = 0;
    public Transform spawnpointParent;
    public List<GameObject> spawnpoints = new List<GameObject>();

    void SubscribeToEvents()
    {
        GameEvents.current.onRespawnPlayer += RespawnThePlayer;
        GameEvents.current.onCheckpointTrigger += NewCheckpoint;
    }

    private void OnDestroy()
    {
        GameEvents.current.onRespawnPlayer -= RespawnThePlayer;
        GameEvents.current.onCheckpointTrigger -= NewCheckpoint;
    }

    void GetSpawnpoints()
    {
        foreach (Transform child in spawnpointParent)
        {
            if (child.tag == "SpawnPoint")
            {
                spawnpoints.Add(child.gameObject);
                Debug.Log("Spawnpoint: " + spawnpoints.Count);
            }
        }
    }

    void OnEnable()
    {
        Debug.Log("OnEnable was called");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Called when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        SubscribeToEvents();

        if (spawnpointParent != null)
        {
            GetSpawnpoints();
        }

        if (player == null)
        {
            StartCoroutine(SpawnPlayer());
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Checkpoints--------------------------------------------------------
    public void NewCheckpoint(int id)
    {
        if (id > checkpointID)
        {
            checkpointID = id;
            Debug.Log("GameController New Checkpoint: " + checkpointID);
            GameEvents.current.PlaySound("Checkpoint");
        }
    }
    //Player Spawning and Respawning-------------------------------------
    IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        player = Instantiate(playerPrefab, spawnpoints[checkpointID].transform.position, Quaternion.identity);
    }
    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(3f);

        //Respawn the player at active spawnpoint
        player = Instantiate(playerPrefab, spawnpoints[checkpointID].transform.position, Quaternion.identity);
        GameEvents.current.PlayerRespawn(); //Call event in GameEvents
    }

    //Events-------------------------------
    public void RespawnThePlayer()
    {
        //Respawn the player
        StartCoroutine(RespawnPlayer());
    }
}