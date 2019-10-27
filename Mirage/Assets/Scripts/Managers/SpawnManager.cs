using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// manages initial player spawn and continuous respawning at checkpoint locations 
public class SpawnManager : MonoBehaviour
{
    // reference to top player gameobject
    [SerializeField]
    private GameObject topPlayer;

    // reference to bottom player gameobject
    [SerializeField]
    private GameObject bottomPlayer;

    // reference to top side checkpoint transforms
    [SerializeField]
    private Transform[] topCheckpoints;

    // reference to bottom side checkpoint transforms
    [SerializeField]
    private Transform[] bottomCheckpoints;
    
    // reference to the player's current checkpoint
    private int checkpointIndex;

    // reference to active top player gameobject
    private GameObject topPlayerActive;

    // reference to active bottom player gameobject
    private GameObject bottomPlayerActive;

    // reference to dynamic root gameobject for instsantiating players to
    private GameObject dynamicRoot;

    // reference to death sfx
    [SerializeField]
    private AudioController _deathSfxAC;

    public delegate void OnRespawn(int respawnIndex); // declare new resapwn delegate
    public event OnRespawn notifyRespawnObservers; // instantiate respawn observer set

    // spawn manager instance
    private static SpawnManager _instance;
    public static SpawnManager Instance { get { return _instance; } }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        dynamicRoot = GameObject.FindGameObjectWithTag("DynamicRoot");

        checkpointIndex = 0;
                
        InstantiatePlayer(); // instantiate both top and bottom players 
    }

    void Start()
    {      
        topPlayerActive = GameObject.FindGameObjectWithTag("TopPlayer");
        bottomPlayerActive = GameObject.FindGameObjectWithTag("BottomPlayer");

        // initialize cinemachine camera to follow the top player
        GameObject playerCamera = GameObject.FindGameObjectWithTag("CM-1");

        playerCamera.GetComponent<CinemachineVirtualCamera>().LookAt = topPlayerActive.transform;
        playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = topPlayerActive.transform;

        LifeManager lifeManager = FindObjectOfType<LifeManager>();
        lifeManager.notifyLifeObservers += OnLifeLost; // observation handler registration

        TimerManager timerManager = FindObjectOfType<TimerManager>();
        timerManager.notifyTimerObservers += OnLimitReached; // observation handler registration
    }

    // destroy instance on destroy
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    // respawns player when timer limit reached
    void OnLimitReached()
    {
        RespawnPlayer();
    }

    // respawns player when dies to a trap
    void OnLifeLost(int playerLives)
    {
        RespawnPlayer();
    }

    // helper function to instantiate players 
    private void InstantiatePlayer()
    {
        Instantiate(topPlayer, topCheckpoints[checkpointIndex].position, Quaternion.identity, dynamicRoot.transform);
        Instantiate(bottomPlayer, bottomCheckpoints[checkpointIndex].position, Quaternion.identity, dynamicRoot.transform);
    }

    // function to respawn player at most recent checkpoint
    private void RespawnPlayer()
    {
        if (_deathSfxAC != null)
            _deathSfxAC.PlaySfx();
        topPlayerActive.transform.position = topCheckpoints[checkpointIndex].position;
        bottomPlayerActive.transform.position = bottomCheckpoints[checkpointIndex].position;
        notifyRespawnObservers(checkpointIndex - 1);

    }

    // function to increment checkpoint index 
    public void IncrementCheckpointIndex(string tag)
    {
        checkpointIndex ++;
    }
}

