using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace LevelManagement
{
    // manages the menus
    public class MenuManager : MonoBehaviour
    {

        // references to all menu prefabs
        [SerializeField]
        private  MainMenu mainMenuPrefab;
        [SerializeField]
        private SettingsMenu settingsMenuPrefab;
        [SerializeField]
        private GameMenu gameMenuPrefab;
        [SerializeField]
        private PauseMenu pauseMenuPrefab;
        [SerializeField]
        private WinScreen winScreenPrefab;
        [SerializeField]
        private FinishMenu finMenuPrefab;

        // reference to menu parent object
        [SerializeField]
        private Transform _menuParent;

        // reference to stack for tracking active menus
        private Stack<Menu> _menuStack = new Stack<Menu>();

        // reference to manager instance
        private static MenuManager _instance;
        public static MenuManager Instance { get { return _instance; } }

        // initialise menus if instance doesn't exist, and make persistent 
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;

                InitializeMenus();
                DontDestroyOnLoad(gameObject);
            }
        }

        // remove instance if destroyed
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        // initializes all the menu prefabs
        private void InitializeMenus()
        {
            if (_menuParent == null)
            {
                GameObject menuParentObject = new GameObject("Menus");
                _menuParent = menuParentObject.transform;
            }

            DontDestroyOnLoad(_menuParent.gameObject);

            BindingFlags myFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            FieldInfo[] fields = this.GetType().GetFields(myFlags);

            foreach (FieldInfo field in fields)
            {
                Menu prefab = field.GetValue(this) as Menu;

                if (prefab != null)
                {
                    Menu menuInstance = Instantiate(prefab, _menuParent);

                    if (prefab != mainMenuPrefab)
                        menuInstance.gameObject.SetActive(false);
                    else
                        OpenMenu(menuInstance);
                }
            }
        }

        // opens a menu and adds it to the menu stack
        public void OpenMenu(Menu menuInstance)
        {
            if (menuInstance == null)
            {
                Debug.Log("MENUMANAGER OpenMenu ERROR: invalid menu");
                return;
            }

            if (_menuStack.Count > 0)
            {
                foreach (Menu menu in _menuStack)
                    menu.gameObject.SetActive(false);
            }

            menuInstance.gameObject.SetActive(true);
            _menuStack.Push(menuInstance);
        }

        // closes a menu and removes it from the menu stack
        public void CloseMenu()
        {
            if (_menuStack.Count == 0)
            {
                Debug.LogWarning("MENUMANAGER CloseMenu ERROR: No menus in stack");
                return;
            }

            Menu topMenu = _menuStack.Pop();
            topMenu.gameObject.SetActive(false);

            if (_menuStack.Count > 0)
            {
                Menu nextMenu = _menuStack.Peek();
                nextMenu.gameObject.SetActive(true);
            }
        }
    }
}