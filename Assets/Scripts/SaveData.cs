using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ISaveData
{
    public IObservable<float> DatumMusic
    {
        get;
    }
    public IObservable<float> DatumSound
    {
        get;
    }
    public IObservable<int> DatumFloorHeight
    {
        get;
    }
    public void SaveDatumMusic(float value);
    public void SaveDatumSound(float value);
    public void SaveDatumFloorHeight(int value);
    public void IssueCaches();
}
public class SaveData : MonoBehaviour, ISaveData
{
    [SerializeField] private string musicKey;
    [SerializeField] private string soundKey;
    [SerializeField] private string floorHeightKey;

    private readonly Subject<float> _datumMusic = new Subject<float>();
    private readonly Subject<float> _datumSound = new Subject<float>();
    private readonly Subject<int> _datumFloorHeight = new Subject<int>();

    public IObservable<float> DatumMusic => _datumMusic;
    public IObservable<float> DatumSound => _datumSound;
    public IObservable<int> DatumFloorHeight => _datumFloorHeight;

    private float _cacheMusic;
    private float _cacheSound;
    private int _cacheFloorHeight;
    private void Awake()
    {
        if (ES3.KeyExists(musicKey))
        {
            _cacheMusic = ES3.Load<float>(musicKey);
        }
        else
        {
            ES3.Save(musicKey, 0.0f);
            _cacheMusic = 0.0f;
        }

        if (ES3.KeyExists(soundKey))
        {
            _cacheSound = ES3.Load<float>(soundKey);
        }
        else
        {
            ES3.Save(soundKey, 0.0f);
            _cacheSound = 0.0f;
        }

        if (ES3.KeyExists(floorHeightKey))
        {
            _cacheFloorHeight = ES3.Load<int>(floorHeightKey);
        }
        else
        {
            ES3.Save(floorHeightKey, 1);
            _cacheFloorHeight = 1;
        }
    }

    public void SaveDatumMusic(float value)
    {
        ES3.Save(musicKey, value);
    }

    public void SaveDatumSound(float value)
    {
        ES3.Save(soundKey, value);
    }

    public void SaveDatumFloorHeight(int value)
    {
        ES3.Save(floorHeightKey, value);
    }

    public void IssueCaches()
    {
        _datumMusic.OnNext(_cacheMusic);
        _datumSound.OnNext(_cacheSound);
        _datumFloorHeight.OnNext(_cacheFloorHeight);
    }
}
