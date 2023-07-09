using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource _audioSource, _effectSource;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayEffect(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlayLoopingSound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.pitch = 1;
        _audioSource.loop = true;
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void StopMusic()
    {        
        _audioSource.Stop();
    }
}
