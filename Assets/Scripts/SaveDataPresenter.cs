using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SaveDataPresenter : MonoBehaviour
{
    private IFloorHeightModel _floorHeightModel;
    private IAudioModel _audioModel;
    private ISaveData _saveData;

    private void Awake()
    {
        _floorHeightModel = GameObject.Find("FloorHeightModel").GetComponent<IFloorHeightModel>();
        _audioModel = GameObject.Find("AudioModel").GetComponent<IAudioModel>();
        _saveData = GameObject.Find("SaveData").GetComponent<ISaveData>();
    }

    private void Start()
    {
        _saveData.DatumMusic.Subscribe(value =>
        {
            _audioModel.SetMusicVolume(value);
        });

        _saveData.DatumSound.Subscribe(value =>
        {
            _audioModel.SetSoundVolume(value);
        });

        _saveData.DatumFloorHeight.Subscribe(value =>
        {
            _floorHeightModel.SetFloorHeight(value);
        });

        _audioModel.MusicVolume.Subscribe(value =>
        {
            _saveData.SaveDatumMusic(value);
        });

        _audioModel.SoundVolume.Subscribe(value =>
        {
            _saveData.SaveDatumSound(value);
        });

        _floorHeightModel.FloorHeight.Subscribe(value =>
        {
            _saveData.SaveDatumFloorHeight(value);
        });
        
        _saveData.IssueCaches();
    }
}
