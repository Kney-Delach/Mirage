using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using LevelManagement.Data;

// TODO : Resolution functionality verification 
// TODO : Implement Master / Music / SFX volume control 

namespace LevelManagement
{
    // enum of audio types 
    public enum AudioTypes
    {
        Master, 
        Sfx, 
        Music
    }

    public class SettingsMenu : Menu<SettingsMenu>
    {
        //references to all relavent settings menu sliders and dropdown lists
        [SerializeField]
        private Slider _masterVolumeSlider;

        [SerializeField]
        private Slider _sfxVolumeSlider;

        [SerializeField]
        private Slider _musicVolumeSlider;

        [SerializeField]
        private Dropdown _fullscreenDropdown;

        [SerializeField]
        private Dropdown _resolutionDropdown;

        // reference to data manager 
        private DataManager _dataManager;

        // reference to audio manager 
        private AudioManager _audioManager;

        // reference to user system resolutions 
        private Resolution[] _resolutions;

        // reference to initially selected button 
        [SerializeField]
        private Button _selectedComponent;

        // reference to activity status of this menu 
        private bool _active = false;

        // declare delegate for audio change, type specifying 
        public delegate void OnAudioChange(AudioTypes audioType, float value);

        // instantiate audio observer set
        public event OnAudioChange notifyAudioObservers; 

        protected override void Awake()
        {
            base.Awake();
            _dataManager = Object.FindObjectOfType<DataManager>();
            _audioManager = Object.FindObjectOfType<AudioManager>();
                                            
            _audioManager.SubscribeToSettings(this);

            LoadData();

            InitializeSettings();

            _audioManager.InitializeSounds();
        }

        private void Update()
        {
            if (!_active)
            {
                _active = true;
                _selectedComponent.Select();
                _selectedComponent.OnSelect(null);
            }
        }

        // called when master volume slider changes 
        public void OnMasterVolumeChanged(float volume)
        {
            if (_dataManager != null)
            {
                _dataManager.MasterVolume = volume;
                notifyAudioObservers(AudioTypes.Master, volume);
            }
        }

        // called when sfx volume slider changes 
        public void OnSFXVolumeChanged(float volume)
        {
            if (_dataManager != null)
            {
                _dataManager.SfxVolume = volume;
                notifyAudioObservers(AudioTypes.Sfx, volume);
            }
        }

        // called when music volume slider changes 
        public void OnMusicVolumeChanged(float volume)
        {
            if (_dataManager != null)
            {
                _dataManager.MusicVolume = volume;
                notifyAudioObservers(AudioTypes.Music, volume);
            }
        }

        // called when fullscreen dropdown value changed
        public void OnFullScreenChanged(int fullScreenIndex)
        {
            if (_dataManager != null)
            {
                SetFullScreen(fullScreenIndex);
                _dataManager.FullScreenIndex = fullScreenIndex;
            }
        }

        // TODO: Add saving functionality [Add checks for previous saved resolution existing in new resolution set]
        // called when resolution dropdown value changed
        public void OnResolutionChanged(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        // custom back press function, sets menu to inactive
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            if (_dataManager != null)
            {
                _dataManager.Save();
            }
            _active = false; 
        }

        // loads data from data manager upon initialization
        public void LoadData()
        {
            if (_dataManager == null || _masterVolumeSlider == null ||
                _sfxVolumeSlider == null || _musicVolumeSlider == null || 
                _fullscreenDropdown == null)
            {
                return;
            }
            _dataManager.Load();

            _masterVolumeSlider.value = _dataManager.MasterVolume;
            notifyAudioObservers(AudioTypes.Master, _dataManager.MasterVolume);

            _sfxVolumeSlider.value = _dataManager.SfxVolume;
            notifyAudioObservers(AudioTypes.Sfx, _dataManager.SfxVolume);

            _musicVolumeSlider.value = _dataManager.MusicVolume;
            notifyAudioObservers(AudioTypes.Music, _dataManager.MusicVolume);

            _fullscreenDropdown.value = _dataManager.FullScreenIndex;            
        }

        // initalizing function for all previously saved settings
        private void InitializeSettings()
        {
            InitializeResolutions();
            SetFullScreen(_dataManager.FullScreenIndex);
        }
        // resolutions initializer helper function
        private void InitializeResolutions()
        {
            _resolutions = Screen.resolutions;
            _resolutionDropdown.ClearOptions();

            // Gather resolutions of local system for dropdown usage
            List<string> resolutionOptions = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                resolutionOptions.Add(option);

                if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
            _resolutionDropdown.AddOptions(resolutionOptions);
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }

        // full screen helper function
        private void SetFullScreen(int fullScreenIndex)
        {
            if (fullScreenIndex == 0)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
        }

    }
}
