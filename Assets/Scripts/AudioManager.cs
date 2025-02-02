using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSfx;
    [SerializeField] private AudioClip slotSpinSfx;
    [SerializeField] private AudioClip winSFX;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSfx()
    {
        _audioSource.loop = false;
        _audioSource.PlayOneShot(buttonSfx);
    }

    public void PlaySlotSpinSfx()
    {
        _audioSource.PlayOneShot(slotSpinSfx);
        _audioSource.loop = true;
    }

    public void PlayWinSfx()
    {
        _audioSource.loop = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(winSFX);
    }

    public void Stop()
    {
        _audioSource.loop = false;
        _audioSource.Stop();
    }
}
