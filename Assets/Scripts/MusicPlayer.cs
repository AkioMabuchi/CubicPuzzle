using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public interface IMusicPlayer
{
    public void StartMusic();
}
public class MusicPlayer : MonoBehaviour, IMusicPlayer
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => _audioSource.isPlaying)
            .Where(_ => _audioSource.time > 96.0f)
            .Subscribe(_ =>
            {
                _audioSource.time = 0.0f;
            });
    }

    public void StartMusic()
    {
        _audioSource.Play();
    }
}
