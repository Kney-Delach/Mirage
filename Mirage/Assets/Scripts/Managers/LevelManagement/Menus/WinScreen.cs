using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelManagement
{
    // shown when player completes the level
    public class WinScreen : Menu<WinScreen>
    {
        // delay before we play the game
        [SerializeField]
        private float _playDelay = 0.5f;

        // reference to transition prefab
        [SerializeField]
        private TransitionFader levelTransitionPrefab;

        // reference to initially selected button 
        [SerializeField]
        private Button _selectedComponent;

        // reference to whether or not this menu has been made active or not 
        private bool _active = false;

        private void Update()
        {
            if (!_active)
            {
                _active = true;
                _selectedComponent.Select();
                _selectedComponent.OnSelect(null);
            }
        }

        // advance to the next level
        public void OnNextLevelPressed()
        {
            Time.timeScale = 1;
            StartCoroutine(OnNextPressedRoutine());
        }

        // restart the current level
        public void OnRestartPressed()
        {
            Time.timeScale = 1;
            StartCoroutine(OnRestartPressedRoutine());
        }

        private IEnumerator OnRestartPressedRoutine()
        {
            TransitionFader.PlayTransition(levelTransitionPrefab, LevelLoader.CurrentSceneIndex-1);
            yield return new WaitForSeconds(_playDelay);
            LevelLoader.ReloadLevel();
            base.OnBackPressed();
            _active = false;
        }

        // return to MainMenu scene
        public void OnMainMenuPressed()
        {
            LevelLoader.LoadMainMenuLevel();
            MainMenu.Open();
            _active = false;
        }

        private IEnumerator OnNextPressedRoutine()
        {
            // TODO: Add nicer start transition 
            TransitionFader.PlayTransition(levelTransitionPrefab,LevelLoader.CurrentSceneIndex);
            yield return new WaitForSeconds(_playDelay);
            LevelLoader.LoadNextLevel();
            base.OnBackPressed();
            _active = false;
        }
    }
}
