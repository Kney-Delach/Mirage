using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls checkpoints
[RequireComponent(typeof(Collider2D))]
public class CheckpointController : MonoBehaviour
{
    // reference to the Spawn Manager
    private SpawnManager spawnManager;

    // turn triggered to true if checkpoint has been reached
    private bool _triggered = false;
    private bool Triggered { get { return _triggered; } }

    // reference to top checkpoint animator
    [SerializeField]
    private Animator topAnimator;

    // reference to bottom checkpoint animator
    [SerializeField]
    private Animator bottomAnimator;

    // reference to this objects audio controller 
    [SerializeField]
    AudioController _audioController;

    private void Awake()
    {
        spawnManager = Object.FindObjectOfType<SpawnManager>();
    }

    // increments spawn manager checkpoint index upon collision and initialises animation
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "TopPlayer" || collision.gameObject.tag == "BottomPlayer") && !_triggered)
        {
            _triggered = true;
            _audioController.PlaySfx();
            topAnimator.SetBool("CheckpointReached", true);
            bottomAnimator.SetBool("CheckpointReached", true);
            spawnManager.IncrementCheckpointIndex(collision.gameObject.tag);

        }
    }
}
