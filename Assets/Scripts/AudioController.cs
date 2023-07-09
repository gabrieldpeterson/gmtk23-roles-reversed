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
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip clip, float volume)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    public void ChangeMusic(AudioClip clip, float volume)
    {
        if (_audioSource.clip == clip) { return; }
        Debug.Log($"Music changed to {clip}");
        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.Play();
    }
}
