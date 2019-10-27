using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement.Data
{
    // manages saved data
    public class DataManager : MonoBehaviour
    {
        private SaveData _saveData;
        private JsonSaver _jsonSaver;

        // public properties to set and get values from the SaveData object
        public float MasterVolume
        {
            get { return _saveData.masterVolume; }
            set { _saveData.masterVolume = value; }
        }

        public float SfxVolume
        {
            get { return _saveData.sfxVolume; }
            set { _saveData.sfxVolume = value; }
        }

        public float MusicVolume
        {
            get { return _saveData.musicVolume; }
            set { _saveData.musicVolume = value; }
        }

        public int FullScreenIndex
        {
            get { return _saveData.fullScreenIndex; }
            set { _saveData.fullScreenIndex = value; }
        }

        // initializes SaveData and JsonSaver objects
        private void Awake()
        {
            _saveData = new SaveData();
            _jsonSaver = new JsonSaver();
        }

        // saves the data using the JsonSaver
        public void Save()
        {
            _jsonSaver.Save(_saveData);
        }

        // loasd the data using the JsonSaver
        public void Load()
        {
            _jsonSaver.Load(_saveData);
        }

	}
}
