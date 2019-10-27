using System.Collections;
using UnityEngine;

// controls audio playback for a single sound
public class AudioController : MonoBehaviour
{
    // reference the sfx clip to play 
    [SerializeField]
    private string _soundName;

    // refernece to the audio manager
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = Object.FindObjectOfType<AudioManager>();       
    }

    // play single shot sfx
    public void PlaySfx()
    {
        if (_audioManager == null)
            return;

        _audioManager.PlaySound(_soundName);            
    }
}
