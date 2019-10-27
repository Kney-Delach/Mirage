using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes an object persistent in between scene loads 
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
        Object.DontDestroyOnLoad(gameObject);
    }
}
