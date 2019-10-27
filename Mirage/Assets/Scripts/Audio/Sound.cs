using UnityEngine.Audio;
using UnityEngine;
using LevelManagement;

// sounds object stores references to required components to initialise a sound in the game
// used by the AudioManager 
[System.Serializable]
public class Sound 
{
    // reference to the audio type of the sound (from the menus)
    public AudioTypes _audioType;

    // reference to sound name
    public string _soundName;

    // reference to the sound's audio clip 
    public AudioClip _audioClip;

    // reference to custom volume of the sound object
    public float _volume = 0;

    // reference to an audio source to be created by AudioManager
    [HideInInspector]
    public AudioSource _audioSource;

    // reference to pause status of sound
    private bool _isPaused = false; 
    public bool IsPaused { get { return _isPaused; } set { _isPaused = value; } }
}
