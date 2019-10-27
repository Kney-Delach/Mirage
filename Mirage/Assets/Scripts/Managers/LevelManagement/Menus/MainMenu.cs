using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LevelManagement.Data;

namespace LevelManagement
{
    // controls the main menu screen
    public class MainMenu : Menu<MainMenu>
    {
        // delay before game begins
        [SerializeField]
        private float _playDelay = 0.5f;

        // reference to transition prefab
        [SerializeField]
        private TransitionFader startTransitionPrefab;

        // reference to DataManager
        private DataManager _dataManager;

        // reference to initially selected button 
        [SerializeField]
        private Button _selectedComponent;

        // reference to whether or not this menu has been made active or not 
        private bool _active = false;

		protected override void Awake()
		{
            base.Awake();
            _dataManager = Object.FindObjectOfType<DataManager>();
        }

        private void Update()
        {
            if(!_active)
            {
                _active = true;
                _selectedComponent.Select();
                _selectedComponent.OnSelect(null);
            }

        }

        // launch the first game level
        public void OnPlayPressed()
        {
            StartCoroutine(OnPlayPressedRoutine());
        }

        // start the transition and play the first level
        private IEnumerator OnPlayPressedRoutine()
        {
            TransitionFader.PlayTransition(startTransitionPrefab, LevelLoader.CurrentSceneIndex);
            yield return new WaitForSeconds(_playDelay);
            LevelLoader.LoadNextLevel();
            GameMenu.Open();
            _active = false;
        }

        // open the settings menu
        public void OnSettingsPressed()
        {
            SettingsMenu.Open();
            _active = false;
        }

        // quit the application
        public override void OnBackPressed()
        {
            Application.Quit();
        }

    }
}