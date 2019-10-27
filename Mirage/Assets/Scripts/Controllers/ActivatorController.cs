using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// controlls pressure plate activation and all the relevant connections (barriers, animations, etc)
[RequireComponent(typeof(Collider2D))]
public class ActivatorController : MonoBehaviour
{
    // reference to check if activator has been activated
    private bool _activated = false;

    // reference to check if activator is being triggered
    private bool _triggering = false;
    private bool Triggering { get { return _triggering; } }

    // reference to the opposite direction activator 
    [FormerlySerializedAs("oppositeActivator")]
    [SerializeField]
    private ActivatorController _oppositeActivator;

    // reference to the barrier animator 
    [FormerlySerializedAs("barrierAnimator")]
    [SerializeField]
    private Animator _barrierAnimator;

    // reference to this activator animator 
    [SerializeField]
    private Animator _activatorAnimator;

    // reference to the audio controller of the barriers
    [SerializeField]
    private AudioController _barrierAudioController;

    // reference to the audio controller of the triggering effect
    [SerializeField]
    private AudioController _triggeredAudioController;

    // reference to the audio controller of the activation effect
    [SerializeField]
    private AudioController _activatedAudioController;

    // reference to the locking mechanism for this object
    private string _triggerLock = null; 

    // enum of subscribers to this object
    private enum ActivatorSubscribers{
        TopPlayer,
        BottomPlayer,
        Crate
    }

    // verifies and locks triggering to single object upon collision
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_triggerLock != null)
        {
            return;
        }
        _triggerLock = collision.tag;
        TriggerActivator();

    }

    // sets triggering visualisation and logic of activator
    private void TriggerActivator()
    {
        _activatorAnimator.SetBool("Triggered", true);
        _triggering = true;
        if (_triggeredAudioController != null)
            _triggeredAudioController.PlaySfx();
    }

    // contains activation triggering logic
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_triggerLock != collision.tag && _triggerLock != null)
        {
            return;
        }
        else if (_triggerLock == null)
        {
            _triggerLock = collision.tag;
            TriggerActivator();
        }

        if ((collision.tag == ActivatorSubscribers.TopPlayer.ToString() || collision.tag == ActivatorSubscribers.BottomPlayer.ToString() || 
             collision.tag == ActivatorSubscribers.Crate.ToString()) && !_activated && _oppositeActivator.Triggering)
        {
            _activatorAnimator.SetBool("Triggered", false);
            _activatorAnimator.SetBool("Activated", true);

            _activated = true;
            _barrierAnimator.SetBool("UnlockedSection", true);

            if (_activatedAudioController != null)
                _activatedAudioController.PlaySfx();

            if (_barrierAudioController != null)
                _barrierAudioController.PlaySfx();
        }
    }

    // function disabling trigger if only one activator reached
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_triggerLock != collision.tag)
            return;

        if (!_activated)
        {
            _activatorAnimator.SetBool("Triggered", false);
            _triggering = false;
            if (_triggeredAudioController != null)
                _triggeredAudioController.PlaySfx();
            _triggerLock = null;

        }
    }

    // reset barriers upon player death
    public void ResetBarrier()
    {
        _activatorAnimator.SetBool("Triggered", false);
        _activatorAnimator.SetBool("Activated", false);
        _barrierAnimator.SetBool("UnlockedSection", false);
        _activated = false;
        _triggering = false;
        _triggerLock = null;
    }
}
