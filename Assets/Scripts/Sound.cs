using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public interface ISound
{
    public void Initialize(AudioClip audioClip);
}
public class Sound : MonoBehaviour, ISound
{
    private AudioSource _audioSource;

    private AudioClip _audioClip;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.clip = _audioClip;
        _audioSource.time = 0.0f;
        _audioSource.Play();

        this.UpdateAsObservable()
            .Where(_ => !_audioSource.isPlaying)
            .Subscribe(_ =>
            {
                Destroy(gameObject);
            });
    }

    public void Initialize(AudioClip audioClip)
    {
        _audioClip = audioClip;
    }
}
