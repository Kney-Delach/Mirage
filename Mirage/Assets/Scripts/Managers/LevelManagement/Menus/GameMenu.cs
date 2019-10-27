using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    // shown when in game 
    public class GameMenu : Menu<GameMenu>
    {        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPausePressed();
            }
        }

        // pauses the game and opens the pause menu
        public void OnPausePressed()
        {
            Time.timeScale = 0;
            AudioManager audioManager = Object.FindObjectOfType<AudioManager>();
            audioManager.PauseSfx();
            PauseMenu.Open();
        }

    }
}
