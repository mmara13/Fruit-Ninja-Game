using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("------------- Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("------------- Audio Clip ------------")]
    public AudioClip background;
    public AudioClip bomb;
    public AudioClip slice;
    public AudioClip pause;
    public AudioClip unpause;

    public bool isMuted;


    public void Start()
    {
        musicSource.clip = background;
        isMuted = false;
        musicSource.Play();
    }

    public void Stop()
    {
        isMuted = true;
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
