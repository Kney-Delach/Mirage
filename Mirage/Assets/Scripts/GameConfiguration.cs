using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace config
{
    public class GameConfiguration : MonoBehaviour
    {
        // reference to platform running 
        private string _platform = "EDITOR";
        public string Platform { get { return _platform; } }

        private static GameConfiguration _instance;
        public static GameConfiguration Instance { get { return _instance; } }

        // Start is called before the first frame update
        private void Awake()
        {
            // singleton initialization
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;


#if UNITY_ANDROID
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                _platform = "ANDROID";
#endif

#if UNITY_IOS
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            _platform = "IOS";
#endif 
        }

        // destroy instance on destroy
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }


    }
}