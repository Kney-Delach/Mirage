using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// destroys an object after a set amount of alive time
public class ObjectDestructor : MonoBehaviour
{
    // reference to amount of time object should exist before destruction upon Destroy call
    public float aliveTime;

    void Start()
    {
        Destroy(gameObject, aliveTime);
    }
}