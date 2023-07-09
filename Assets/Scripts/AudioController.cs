using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] public AudioClip peacefulMusic;
    [SerializeField] public AudioClip intenseMusic;
    [SerializeField] public AudioClip whack;
    [SerializeField] public AudioClip mouseEep;
    
    private AudioSource _audioSource;
    
    public static AudioController Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the AudioController GameObject.");
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayAudioClip(AudioClip clip, float volume)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    public void ChangeMusic(AudioClip clip, float volume)
    {
        Debug.Log("Method running correctly");
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is null.");
            return;
        }
        if (_audioSource.clip == clip) { return; }
        Debug.Log($"Music changed to {clip}");
        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.Play();
    }
}
