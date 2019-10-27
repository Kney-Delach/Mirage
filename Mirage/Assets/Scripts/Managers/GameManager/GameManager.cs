using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using LevelManagement;

// manages whether or not a level is complete
public class GameManager : MonoBehaviour
{
    // reference to player controllers
    private PlayerController[] _playerControllers;

    // reference to level objective
    private Objective _objective;

    // reference to status of game over 
    private bool _isGameOver;
    public bool IsGameOver { get { return _isGameOver; } }

    // game manager instance for each level
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    // transition prefab from game to menus
    [FormerlySerializedAs("_endTransitionPrefab")]
    [SerializeField]
    private TransitionFader _transitionPrefab;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        _objective = Object.FindObjectOfType<Objective>();
        _playerControllers = Object.FindObjectsOfType<PlayerController>();
    }

    // destroy instance on destroy
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    // end the level
    public void EndLevel()
    {
        if (_playerControllers != null)
        {
            // disable the player controls
            foreach(PlayerController playerController in _playerControllers)
            {
                playerController.enabled = false;
                playerController.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                playerController.GetComponent<Animator>().speed = 0;
            }
        }

        if (!_isGameOver)
        {
            _isGameOver = true;
            StartCoroutine(WinRoutine());
        }
    }

    // level complete coroutine
    private IEnumerator WinRoutine()
    {
        TransitionFader.PlayTransition(_transitionPrefab);

        float fadeDelay = (_transitionPrefab != null) ?
            _transitionPrefab.Delay + _transitionPrefab.FadeOnDuration : 0f;

        yield return new WaitForSeconds(fadeDelay);
#if UNITY_ADS
        UnityAdsPlayer.Instance.PlayAd();
#endif
        // TODO: ALTER THIS IF NOT IN TUTORIAL
        if (LevelLoader.CurrentSceneIndex == 6)
            FinishMenu.Open();
        else
            WinScreen.Open();


    }

    // check for the end game condition on each frame
    private void Update()
    {
        if (_objective != null && _objective.IsComplete)
        {
            EndLevel();
        }
    }
}
