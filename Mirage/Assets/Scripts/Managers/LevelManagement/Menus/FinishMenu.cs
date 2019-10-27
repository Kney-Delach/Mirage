using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelManagement
{
    // shown when player completes the level
    public class FinishMenu : Menu<FinishMenu>
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
        public void OnRestartTutorial()
        {
            Time.timeScale = 1;
            StartCoroutine(OnRestartTutorialPressedRoutine());
        }

        // restart the current level
        public void OnRestartPressed()
        {
            Time.timeScale = 1;
            StartCoroutine(OnRestartLevelPressedRoutine());

        }

        // return to MainMenu scene
        public void OnMainMenuPressed()
        {
            Time.timeScale = 1;
            LevelLoader.LoadMainMenuLevel();
            MainMenu.Open();
            _active = false;
        }

        // restart level coroutine
        private IEnumerator OnRestartLevelPressedRoutine()
        {
            TransitionFader.PlayTransition(levelTransitionPrefab, LevelLoader.CurrentSceneIndex - 1);
            yield return new WaitForSeconds(_playDelay);
            base.OnBackPressed();
            LevelLoader.ReloadLevel();
            _active = false;
        }

        // restart tutorial coroutine
        private IEnumerator OnRestartTutorialPressedRoutine()
        {
            TransitionFader.PlayTransition(levelTransitionPrefab, 1);
            yield return new WaitForSeconds(_playDelay);
            LevelLoader.LoadLevel("2_Level-1");
            base.OnBackPressed();
            _active = false;
        }
    }
}
