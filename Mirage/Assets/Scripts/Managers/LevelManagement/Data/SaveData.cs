using System.Collections;
using System.Collections.Generic;
using System;

namespace LevelManagement.Data
{
    // class containing the data to be saved after closing the application 
    [Serializable]
    public class SaveData
    {
        // audio levels and full screen status references 
        public float masterVolume;
        public float sfxVolume;
        public float musicVolume;
        public int fullScreenIndex;

        // hash value calculated from the contents of SaveData
        public string hashValue;

        // constructor to initialize data
        public SaveData()
        {
            masterVolume = 0f;
            sfxVolume = 0f;
            musicVolume = 0f;
            fullScreenIndex = 0;
            hashValue = String.Empty;
        }

    }
}
