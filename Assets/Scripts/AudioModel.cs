using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

public interface IAudioModel
{
    IObservable<float> MusicVolume
    {
        get;
    }

    IObservable<float> SoundVolume
    {
        get;
    }

    public void SetMusicVolume(float value);
    public void SetSoundVolume(float value);
}

public class AudioModel : MonoBehaviour, IAudioModel
{
    [SerializeField] private AudioMixer audioMixer;

    private readonly ReactiveProperty<float> _musicVolume = new ReactiveProperty<float>();
    private readonly ReactiveProperty<float> _soundVolume = new ReactiveProperty<float>();

    public IObservable<float> MusicVolume => _musicVolume;
    public IObservable<float> SoundVolume => _soundVolume;

    private void Start()
    {
        MusicVolume.Subscribe(value =>
        {
            float settingValue = value > -40.0f ? value : -80.0f;
            audioMixer.SetFloat("Music", settingValue);
        });
        SoundVolume.Subscribe(value =>
        {
            float settingValue = value > -40.0f ? value : -80.0f;
            audioMixer.SetFloat("Sound", settingValue);
        });
    }

    public void SetMusicVolume(float value)
    {
        _musicVolume.Value = value;
    }

    public void SetSoundVolume(float value)
    {
        _soundVolume.Value = value;
    }
}
