using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls dynamic crates
[RequireComponent(typeof(Collider2D))]
public class CrateController : MonoBehaviour
{
    // reference to initial crate position for resetting upon death
    private Vector3 _originalPosition;

    // reference to crate sfx 
    [SerializeField]
    private AudioController _crateSfxAC;

    // reference to active status of sfx 
    private bool _playSfx = false;

    private void Start()
    {
        StartCoroutine(LevelStartWait());
        _originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        spawnManager.notifyRespawnObservers += OnRespawn; // observation handler registration
    }

    // reset crate position upon player death 
    private void OnRespawn(int index)
    {
        _playSfx = false;
        transform.position = _originalPosition;
        StartCoroutine(LevelStartWait());
    }

    // play crate sfx upon collision with player or ground
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_crateSfxAC != null && _playSfx && collision.tag != "TimerCollect")
            _crateSfxAC.PlaySfx();
    }

    // level start coroutine
    private IEnumerator LevelStartWait()
    {
        yield return new WaitForSeconds(0.1f);
        _playSfx = true;
    }
}
