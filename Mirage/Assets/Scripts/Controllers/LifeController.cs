using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [OBSELETE]
/// </summary>

// controlls player life collection objects 
[RequireComponent(typeof(Collider2D))]
public class LifeController : MonoBehaviour
{
    // reference to the LifeManager
    private LifeManager lifeManager;

    // reference to the collection effect
    [SerializeField]
    private GameObject collectionEffect;

    // reference to check if currency has been collected
    private bool _isCollected = false;

    private void Awake()
    {
        lifeManager = Object.FindObjectOfType<LifeManager>();
    }

    // trigger life collection 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "BottomPlayer" || collision.tag == "TopPlayer") && !_isCollected)
        {
            _isCollected = true;
            lifeManager.IncrementLife();
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //transform.parent.gameObject.SetActive(false); //TODO: life collection destroy vs setactive
        }
    }
}
