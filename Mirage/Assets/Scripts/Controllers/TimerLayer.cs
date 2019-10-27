using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// timer layer UI of ontop of player
public class TimerLayer : MonoBehaviour
{
    // reference to timer text 
    [SerializeField]
    private TextMeshProUGUI _timerText;
    public TextMeshProUGUI TimerText { get { return _timerText; } }

    // reference to timer canvas group [used to set inactive in initial levels before timer mechanic is introduced]
    [SerializeField]
    private CanvasGroup _timerGroup;
    public CanvasGroup TimerGroup { get { return _timerGroup; } }
}
