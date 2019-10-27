using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls barriers, if player dies before reaching checkpoint, resets
public class BarrierController : MonoBehaviour
{
    // reference to top barrier activator controller
    [SerializeField]
    private ActivatorController _topAC;

    // reference to bottom barrier activator controller
    [SerializeField]
    private ActivatorController _bottomAC;

    // upon player death, reset the barriers by resetting the activators
    public void OnRespawn()
    {
        _topAC.ResetBarrier();
        _bottomAC.ResetBarrier();
    }
}
