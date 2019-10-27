using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelManagement
{
    // manages scene management and traversal 
    public class LevelLoader : MonoBehaviour
    {
        // index of the MainMenu level
        private static int MAIN_MENU_INDEX = 1;

        // index of the current scene
        private static int _currentSceneIndex;
        public static int CurrentSceneIndex { get { return _currentSceneIndex; } }

        // loads a level by name
        public static void LoadLevel(string levelName)
        {
            // if the scene is in the BuildSettings, load the scene
            if (Application.CanStreamedLevelBeLoaded(levelName))
                SceneManager.LoadScene(levelName);
            else
                Debug.LogWarning("GAMEMANAGER LoadLevel Error: invalid scene specified");            
        }

        // loads a level by index
        public static void LoadLevel(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
            {
                _currentSceneIndex = levelIndex;

                if (levelIndex == LevelLoader.MAIN_MENU_INDEX)
                    MainMenu.Open();

                SceneManager.LoadScene(levelIndex);
            }
            else
                Debug.LogWarning("LEVELLOADER LoadLevel Error: invalid scene specified");
        }

        // reloads the currently active scene
        public static void ReloadLevel()
        {
            LoadLevel(SceneManager.GetActiveScene().name);
        }

        // loads the next scene in the BuildSettings, wraps back to MainMenu if run out of scenes
        public static void LoadNextLevel()
        {
            int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            nextSceneIndex = Mathf.Clamp(nextSceneIndex, MAIN_MENU_INDEX, nextSceneIndex);
            _currentSceneIndex = nextSceneIndex - 1; 
            LoadLevel(nextSceneIndex);
        }

        // loads the MainMenu level
        public static void LoadMainMenuLevel()
        {
            _currentSceneIndex = MAIN_MENU_INDEX;
            LoadLevel(MAIN_MENU_INDEX);
        }

    }
}
