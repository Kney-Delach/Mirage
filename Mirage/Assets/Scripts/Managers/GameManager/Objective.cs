using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// signifies the completion of the main objective of a level, getting to the end
[RequireComponent(typeof(Collider2D))]
public class Objective : MonoBehaviour
{
    // tag to identify the top character
    [SerializeField]
    private string _topPlayerTag = "TopPlayer";

    // tag to identify the bottom character
    [SerializeField]
    private string _bottomPlayerTag = "BottomPlayer";

    // reference to status of top character trigger 
    private bool _topTriggering = false;

    // reference to status of bottom character trigger
    private bool _bottomTriggering = false;
    
    // reference to status of objective completion
    private bool _isComplete;
    public bool IsComplete { get { return _isComplete; } }

    // sets the objective to complete and re-aligns characters
    public void CompleteObjective()
    {
        _isComplete = true;
        GameObject topPlayer = GameObject.FindGameObjectWithTag(_topPlayerTag);
        GameObject bottomPlayer = GameObject.FindGameObjectWithTag(_bottomPlayerTag);

        if (topPlayer.transform.position.x >= bottomPlayer.transform.position.x)
            bottomPlayer.transform.position = new Vector3(topPlayer.transform.position.x, bottomPlayer.transform.position.y,
                bottomPlayer.transform.position.z);
        else
            topPlayer.transform.position = new Vector3(bottomPlayer.transform.position.x, topPlayer.transform.position.y,
                topPlayer.transform.position.z);
    }

    // if both characters triggering, completes objective
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _topPlayerTag)
            _topTriggering = true;
        if (collision.tag == _bottomPlayerTag)
            _bottomTriggering = true;

        if (_topTriggering && _bottomTriggering)
            CompleteObjective();                  
    }

    // if character exits trigger before other character enters it, removes trigger. Both characters must trigger simulteneously
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == _topPlayerTag)
            _topTriggering = false;
        if (collision.tag == _bottomPlayerTag)
            _bottomTriggering = false;
    }

}
