using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;
    public static AudioManager GetInstance()
    {
        return Instance;
    }
    public enum Sound
    {
        MainMenu,
        ButtonClick,
        ToggleSelect,
        PlayerCollision,
        Traffic
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }

    public SoundAudioClip[] audioClips;

    private GameObject _soundGameObject;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Audio_Manager Instance created");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Audio_Manager duplicate Destroyed");
        }
    }

    public void PlaySound(Sound sound)
    {
        _soundGameObject = new GameObject("Sound_" + sound);
        _audioSource = _soundGameObject.AddComponent<AudioSource>();
        _audioSource.clip = GetAudioClip(sound);
        _audioSource.Play();
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        foreach (var audioClip in audioClips)
        {
            if (audioClip.sound == sound)
                return audioClip.audioClip;
        }
        return null;
    }

    public void PauseSound()
    {
        foreach (var audio in GameObject.FindObjectsOfType<AudioSource>())
            audio.Pause();
    }

    public void UnPauseSound()
    {
        foreach (var audio in GameObject.FindObjectsOfType<AudioSource>())
            audio.UnPause();
    }

    public void StopSound()
    {
        foreach (var audio in GameObject.FindObjectsOfType<AudioSource>())
            audio.Stop();
    }
}