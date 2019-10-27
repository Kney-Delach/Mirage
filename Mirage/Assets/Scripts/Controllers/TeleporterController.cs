using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls teleporters 
[RequireComponent(typeof(Collider2D))]
public class TeleporterController : MonoBehaviour
{
    // reference to top player gameobject
    private GameObject _topPlayer;

    // reference to bottom player gameobject
    private GameObject _bottomPlayer;

    // referemce to top teleporter
    [SerializeField]
    private GameObject _topTeleporter;

    // reference to bottom teleporter
    [SerializeField]
    private GameObject _bottomTeleporter;

    // reference to blue teleporter effect
    [SerializeField]
    private GameObject _blueTeleportEffect;

    // reference to purple teleporter effect
    [SerializeField]
    private GameObject _purpleTeleportEffect;

    // reference to time manager
    private TimerManager _timerManager;

    // reference to activity status of KeyCode E
    private bool _triggered_e = false; 

    // reference to this objects audio controller 
    [SerializeField]
    AudioController _audioController;

    /// <summary>
    ///  Configuration variables
    /// </summary>

    // screen width variable
    private float screenWidth;
    
    private void Start()
    {
        screenWidth = Screen.width;

        _timerManager = Object.FindObjectOfType<TimerManager>();
        _topPlayer = GameObject.FindGameObjectWithTag("TopPlayer");
        _bottomPlayer = GameObject.FindGameObjectWithTag("BottomPlayer");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.touchCount >= 3 || Input.touchCount == 1)
        {
            if(Input.touchCount == 1)
            {
                if(Input.GetTouch(0).position.x > 0.3 * screenWidth && Input.GetTouch(0).position.x < 0.7*screenWidth)
                {
                    _triggered_e = true;
                }
            }
            else 
                _triggered_e = true;
        }

        else  if (Input.GetKeyUp(KeyCode.E) || Input.touchCount < 3 && Input.touchCount != 1)
        {
            _triggered_e = false;
        }
    }

    // character teleportation logic 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "TopPlayer" || collision.gameObject.tag == "BottomPlayer") && _triggered_e)
        {
            if (_blueTeleportEffect == null || _purpleTeleportEffect == null)
            {
                Debug.LogError("TeleporterController OnTriggerStay2D: Teleport effects not initialised");
                return;
            }
            StartCoroutine(PlayTeleportPlayers(collision.gameObject.tag));
            _triggered_e = false;
        }
    }

    // teleport players to new position
    private IEnumerator PlayTeleportPlayers(string tag)
    {
        _timerManager.TeleporterInitialised = true; // tell timer manager that player is about to align and thus stop the timer

        if (tag == "TopPlayer")
            Instantiate(_purpleTeleportEffect, _bottomTeleporter.transform.position, Quaternion.identity);

        if (tag == "BottomPlayer")
            Instantiate(_blueTeleportEffect, _topTeleporter.transform.position, Quaternion.identity);

        _audioController.PlaySfx();
        _topPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        _topPlayer.GetComponent<Animator>().enabled = false;
        _bottomPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        _bottomPlayer.GetComponent<Animator>().enabled = false;

        yield return new WaitForSeconds(0.3f);

        if (tag == "TopPlayer")
            _bottomPlayer.transform.position = new Vector3(_topPlayer.transform.position.x, _bottomTeleporter.transform.position.y, _bottomTeleporter.transform.position.z);
        
        if (tag == "BottomPlayer")
            _topPlayer.transform.position = new Vector3(_bottomPlayer.transform.position.x, _topTeleporter.transform.position.y, _topTeleporter.transform.position.z);
        
        _topPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _topPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        _topPlayer.GetComponent<Animator>().enabled = true;

        _bottomPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _bottomPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        _bottomPlayer.GetComponent<Animator>().enabled = true;

        _timerManager.TeleporterInitialised = false;        
    }
}
