using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages the barrier resetting upon player death
public class BarrierManager : MonoBehaviour
{
    // reference to barrier controllers for each level 
    [SerializeField]
    private BarrierController[] _barriers;

    private void Start()
    {
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        spawnManager.notifyRespawnObservers += OnRespawn; // observation handler registration
    }

    // called upon player death
    private void OnRespawn(int index)
    {
        if( index < _barriers.Length)
            _barriers[index].OnRespawn();
    }
}
