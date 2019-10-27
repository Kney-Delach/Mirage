using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// controls the timer collection objects 
[RequireComponent(typeof(Collider2D))]
public class TimerController : MonoBehaviour
{
    // reference to the TimerManager
    private TimerManager _timerManager;

    // reference to timer text object
    [SerializeField]
    private TextMeshProUGUI _timerAmountText;

    // reference to the collection effect
    [SerializeField]
    private GameObject __collectionEffect;

    // reference to extra time addition of collection object
    [SerializeField]
    private float _extraTimeValue = 5;

    // reference to check if timer has been collected
    private bool _isCollected = false;

    // reference to status of collectability of timer, only collectable if timer has started
    private bool _isCollectable = false; 

    private void Awake()
    {
        _timerManager = Object.FindObjectOfType<TimerManager>();
        _timerAmountText.text = "+" + _extraTimeValue.ToString();
    }

    private void Start()
    {
        LifeManager lifeManager = FindObjectOfType<LifeManager>();
        lifeManager.notifyLifeObservers += OnLifeLost; // observation handler registration

        TimerManager timerManager = FindObjectOfType<TimerManager>();
        timerManager.notifyTimerObservers += OnLimitReached; // observation handler registration

        timerManager.notifyTimerStatusObservers += OnTimerStatus;
    }

    // returns collectable status of timer 
    private void OnTimerStatus(bool status)
    {
        _isCollectable = status;
    }

    // when player dies, reset collectability status of object, as timer stops
    private void OnLifeLost(int livesLost)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            _isCollected = false;
        }
    }

    // whether or not timer limit has been reached by player
    private void OnLimitReached()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            _isCollected = false;
        }
    }

    // trigger life collection 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "BottomPlayer" || collision.tag == "TopPlayer") && !_isCollected && _isCollectable)
        {
            _isCollected = true;
            _timerManager.AddTime(_extraTimeValue);

            Instantiate(__collectionEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
