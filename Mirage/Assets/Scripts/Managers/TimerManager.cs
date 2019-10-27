using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LevelManagement;

// manages timer mechanic, and all relevant components, spawnManager, TimerControllers etc...
public class TimerManager : MonoBehaviour
{
    // static referneces to colors for timer color changing 
    private static string GREEN_TEXT = "#60BF6B";
    private static Color32 GREEN_OUTLINE = new Color32(77, 156, 87, 255);

    private static string ORANGE_TEXT = "#DB990C";
    private static Color32 ORANGE_OUTLINE = new Color32(116, 116, 10, 255);

    private static string RED_TEXT = "#E54D22";
    private static Color32 RED_OUTLINE = new Color32(188, 50, 10, 255);

    //private static string BLUE_TEXT = "#2550FF";
    //private static Color32 BLUE_OUTLINE = new Color32(65, 70, 169, 255);

    private static string YELLOW_TEXT = "#F9FF00";
    private static Color32 YELLOW_OUTLINE = new Color32(52,100,73, 255);

    // reference to timer collection sfx 
    [SerializeField]
    private AudioController _audioControllerCollected;

    // reference to timer start sfx 
    [SerializeField]
    private AudioController _audioControllerStarted;

    // reference to whether or not the current level is the intro start tutorial level
    [SerializeField]
    private bool _isTutorialStart;

    // reference to status whether tutorial has started or not
    private bool _tutorialStarted = false;

    // reference to level timer limit
    [SerializeField]
    private float _timerLimit = 0;

    // reference to status of timer start
    private bool _timerStarted;

    // refereence to time at which timer starts
    private float _timerStart;

    // reference to distance between the two controlled characters
    private float _distance;

    // reference to top character game object
    private GameObject _topPlayer;

    // reference to bottom character game object
    private GameObject _bottomPlayer;

    // reference to spawn manager 
    private SpawnManager _spawnManager;

    // reference to timer UI text components
    private TextMeshProUGUI[] _timerTexts;

    // reference to timer UI canvas group components
    private CanvasGroup[] _timerGroups;

    // reference to status whether timer colour is currently active 
    private bool _colorGreen = false;
    private bool _colorOrange = false;
    private bool _colorRed = false;

    // reference to activity status of this object
    [SerializeField]
    private bool _isActive = true;

    // reference to teleporter intialisation status 
    private bool _teleporterIntialised = false; 
    public bool TeleporterInitialised { set {_teleporterIntialised = value; } }
    
    public delegate void OnLimitReached(); // declare new timer delegate type
    public event OnLimitReached notifyTimerObservers; // instantiate timer observer set

    public delegate void OnTimerStatus(bool status); // declare new timer status delegate type
    public event OnTimerStatus notifyTimerStatusObservers; // instantiate timer status observer set

    // spawn manager instance for each level
    private static TimerManager _instance;
    public static TimerManager Instance { get { return _instance; } }

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
    }

    void Start()
    {
        _timerTexts = new TextMeshProUGUI[2];
        _timerGroups = new CanvasGroup[2];

        _topPlayer = GameObject.FindGameObjectWithTag("TopPlayer");
        _bottomPlayer = GameObject.FindGameObjectWithTag("BottomPlayer");

        _spawnManager = Object.FindObjectOfType<SpawnManager>();

        TimerLayer[] timerLayer = FindObjectsOfType<TimerLayer>();
        
        for ( int i = 0; i < 2; i++ )
        {
            _timerTexts[i] = timerLayer[i].TimerText;
            _timerGroups[i] = timerLayer[i].TimerGroup;
        }

        if (_isActive)
        {
            string tempText = FormatTime(_timerLimit);

            for (int i = 0; i < 2; i++)
            {
                _timerGroups[i].alpha = 1;
                _timerTexts[i].text = tempText;
            }

            SetTextColour(GREEN_TEXT, GREEN_OUTLINE);
            _colorGreen = true;
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                _timerGroups[i].alpha = 0;
            }
        }
    }

    // destroy instance on destroy
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    void Update()
    {
        if (!_isActive)
        {
            _distance = System.Math.Abs(_topPlayer.transform.position.x - _bottomPlayer.transform.position.x);
            if (_distance > 0.2)
            {
                _bottomPlayer.transform.position = new Vector3(_topPlayer.transform.position.x, -_topPlayer.transform.position.y, _bottomPlayer.transform.position.z);
            }
            return;
        }
        _distance = System.Math.Abs(_topPlayer.transform.position.x - _bottomPlayer.transform.position.x);
        if (_distance > 0.3 && !_timerStarted && !_teleporterIntialised)
        {
            if(_audioControllerStarted != null)
                _audioControllerStarted.PlaySfx(); 

            if (_isTutorialStart && !_tutorialStarted)
            {
                _tutorialStarted = true;
                notifyTimerStatusObservers(true); // notify status observers

            }
            notifyTimerStatusObservers(true); // notify status observers
            _timerStarted = true;
            _timerStart = Time.time;
            _colorGreen = true;
            _colorOrange = true;
            _colorRed = true;
            SetTextColour(YELLOW_TEXT, YELLOW_OUTLINE);
            StartCoroutine(PlayColorRoutine(1));


        }
        else if (_distance < 0.3 && _timerStarted || _teleporterIntialised)
        {
            _timerStarted = false;
            notifyTimerStatusObservers(false); // notify status observers

            string tempTime = FormatTime(_timerLimit);

            for (int i = 0; i < 2; i++)
            {
                _timerTexts[i].text = tempTime;
            }

            SetTextColour(GREEN_TEXT, GREEN_OUTLINE);
            _colorGreen = true;
            _colorOrange = false;
            _colorRed = false;
        }

        float timeDifference = Time.time - _timerStart;
        if (_timerStarted && (timeDifference >= _timerLimit))
        {
            notifyTimerObservers(); // notify timer observers
            notifyTimerStatusObservers(false); // notify status observers

            _timerStarted = false;
            string tempTime = FormatTime(_timerLimit);

            for (int i = 0; i < 2; i++)
            {
                _timerTexts[i].text = tempTime;
            }

            SetTextColour(GREEN_TEXT, GREEN_OUTLINE);

            _colorGreen = true;
            _colorOrange = false;
            _colorRed = false;
        }
        else if (_timerStarted)
        { 
            float tempTime = _timerLimit - timeDifference;

            _colorGreen = UpdateTimerColour(tempTime, 1.0f, _colorGreen, GREEN_TEXT,GREEN_OUTLINE);
            _colorOrange = UpdateTimerColour(tempTime, 0.75f ,_colorOrange, ORANGE_TEXT, ORANGE_OUTLINE);
            _colorRed = UpdateTimerColour(tempTime, 0.25f, _colorRed, RED_TEXT, RED_OUTLINE);
            for (int i = 0; i < 2; i++)
            {
                _timerTexts[i].text = tempTime.ToString("0.0");
            }
        }

    }

    // function setting the text color of the timer UI
    private void SetTextColour(string hexColor, Color32 outlineColor)
    {
        Color color;
        
        if (ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            for (int i = 0; i < 2; i++)
            {
                _timerTexts[i].color = color;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            _timerTexts[i].outlineColor = outlineColor;
            _timerTexts[i].enabled = false;
            _timerTexts[i].enabled = true;
        }
    }

    // function updating the timer color
    private bool UpdateTimerColour(float tempTime, float percentile, bool isColorSet, string hexColor, Color32 outlineColor)
    {        
        if(((tempTime / _timerLimit) <= percentile) && !isColorSet )
        {

            SetTextColour(hexColor, outlineColor);
            return true;

        }

        if((tempTime >= _timerLimit) && !_colorGreen)
        {
            SetTextColour(GREEN_TEXT, GREEN_OUTLINE);

            return isColorSet;

        }
        return isColorSet;
    }

    // function to format the time 
    private string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:0}.{1:0}", seconds, fraction);
        return timeText;
    }

    // function to add more time to the timer
    public void AddTime(float extraTimeValue)
    {
        if(_audioControllerCollected != null)
           _audioControllerCollected.PlaySfx();

        _timerStart = _timerStart + extraTimeValue;

        _colorGreen = true;
        _colorOrange = true;
        _colorRed = true;

        SetTextColour(YELLOW_TEXT, YELLOW_OUTLINE);
        StartCoroutine(PlayColorRoutine(1));
    }

    // coroutine to reset colours after time addition
    private IEnumerator PlayColorRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        _colorGreen = false;
        _colorOrange = false;
        _colorRed = false;
    }
}
