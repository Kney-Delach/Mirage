using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls the traps
[RequireComponent(typeof(Collider2D))]
public class TrapController : MonoBehaviour
{
    // reference to life manager
    private LifeManager lifeManager;

    // reference to player collision with any trap
    private static bool colliding = false;

    private void Awake()
    {
        lifeManager = Object.FindObjectOfType<LifeManager>();
    }

    // kill player if triggered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "TopPlayer" || collision.tag == "BottomPlayer") && !colliding)
        {
            colliding = true;
            lifeManager.ReduceLife();
            StartCoroutine(WaitForRespawn());
        }
    }

    // wait for 0.1s before settings collision to false, fixes bug where multiple collisions would take effect due to multiple character objects 
    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(0.1f);
        colliding = false;
    }

}
