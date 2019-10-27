using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// TODO: Clean - Remove GUI life component
public class LifeManager : MonoBehaviour
{
    // reference to the life text 
    [SerializeField]
    private TextMeshProUGUI playerLivesText;

    // reference to the life text 
    [SerializeField]
    private int playerLives = 99;
    private int PlayerLives { get { return playerLives; } set { playerLives = value; } }

    public delegate void OnLifeLost(int playerLives); // declare new life delegate type type
    public event OnLifeLost notifyLifeObservers; // instantiate lives observer set

    // spawn manager instance for each level
    private static LifeManager _instance;
    public static LifeManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        playerLivesText.text = playerLives.ToString();
    }

    // destroy instance on destroy
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    // reduce player life
    public void ReduceLife()
    {
        playerLives -= 1;
        playerLivesText.text = playerLives.ToString();

        notifyLifeObservers(playerLives); // notify observers of life value decrease
        //if (playerLives == 0)
        //    notifyLifeObservers(true); // notify observers that player has died
    }

    // increment lives count and update HUD
    public void IncrementLife()
    {
        playerLives += 1;
        playerLivesText.text = playerLives.ToString();
    }
}
