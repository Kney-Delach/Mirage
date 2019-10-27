using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace LevelManagement
{
    // controls the pause menu screen
    public class PauseMenu : Menu<PauseMenu>
    {
        // delay before switching scenes
        [SerializeField]
        private float _playDelay = 0.5f;

        // reference to transition prefab
        [SerializeField]
        private TransitionFader levelTransitionPrefab;

        // reference to initial selected component
        [SerializeField]
        private Button _selectedComponent;

        // reference to active status of menu
        private bool _active = false;

        private void Update()
        {
            if (!_active)
            {
                _active = true;
                _selectedComponent.Select();
                _selectedComponent.OnSelect(null);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnResumePressed();
            }
        }
        // resumes the game and closes the pause menu
        public void OnResumePressed()
        {
            Time.timeScale = 1;
            AudioManager audioManager = Object.FindObjectOfType<AudioManager>();
            audioManager.UnPauseSfx();
            base.OnBackPressed();
            _active = false;
        }

        // unpauses and restarts the current level
        public void OnRestartPressed()
        {
            Time.timeScale = 1;
            LevelLoader.ReloadLevel();
            base.OnBackPressed();
            _active = false;
        }

        // opens the settings menu
        public void OnSettingsPressed()
        {
            SettingsMenu.Open();
            _active = false;
        }

        // unpauses and loads the MainMenu level
        public void OnMainMenuPressed()
        {
            Time.timeScale = 1;
            LevelLoader.LoadMainMenuLevel();
            MainMenu.Open();
            _active = false;
        }

        // quits the application (does not work in Editor, build only)
        public void OnQuitPressed()
        {
            Application.Quit();
        }
    }
}
