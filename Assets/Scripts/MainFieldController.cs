using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IMainFieldController
{
    public IObservable<Unit> OnStageClear
    {
        get;
    }

    public IObservable<Unit> OnVerified
    {
        get;
    }
}
public class MainFieldController : MonoBehaviour, IMainFieldController
{
    private IMainFieldModel _mainFieldModel;
    private IMainFieldInputs _mainFieldInputs;
    private ISoundPlayer _soundPlayer;

    private readonly Subject<Unit> _onStageClear = new Subject<Unit>();
    private readonly Subject<Unit> _onVerified = new Subject<Unit>();

    public IObservable<Unit> OnStageClear => _onStageClear;
    public IObservable<Unit> OnVerified => _onVerified;
    
    private MainMode _mainMode = MainMode.Idle;
    private void OnEnable()
    {
        _mainFieldModel = GameObject.Find("MainFieldModel").GetComponent<IMainFieldModel>();
        _mainFieldInputs = GameObject.Find("MainFieldInputs").GetComponent<IMainFieldInputs>();
        _soundPlayer = GameObject.Find("SoundPlayer").GetComponent<ISoundPlayer>();
    }

    private void Start()
    {
        _mainFieldInputs.MoveCommand.Subscribe(moveDirection =>
        {
            StartCoroutine(CoroutineMoving(moveDirection));
        });

        MainModeModel.Mode.Subscribe(mainMode =>
        {
            _mainMode = mainMode;
        });
    }

    IEnumerator CoroutineMoving(MoveDirection direction)
    {
        if (_mainFieldModel.MoveObjects(direction))
        {
            _mainFieldInputs.SetActive(false);
            
            if (_mainFieldModel.UpdateFlags())
            {
                if (_mainFieldModel.IsClear() && (_mainMode == MainMode.Main || _mainMode == MainMode.Verifying))
                {
                    _soundPlayer.PlaySound(1);
                }
                else
                {
                    _soundPlayer.PlaySound(0);
                }
            }
            yield return new WaitForSeconds(0.3f);
            if (_mainFieldModel.UpdateFloors())
            {
                _soundPlayer.PlaySound(2);
            }
            
            if (_mainFieldModel.FallObjects())
            {
                if (_mainFieldModel.UpdateFlags())
                {
                    if (_mainFieldModel.IsClear() && (_mainMode == MainMode.Main || _mainMode == MainMode.Verifying))
                    {
                        _soundPlayer.PlaySound(1);
                    }
                    else
                    {
                        _soundPlayer.PlaySound(0);
                    }
                }
                yield return new WaitForSeconds(0.2f);
                if (_mainFieldModel.UpdateFloors())
                {
                    _soundPlayer.PlaySound(2);
                }
            }

            if (_mainFieldModel.IsClear() && (_mainMode == MainMode.Main || _mainMode == MainMode.Verifying))
            {
                _mainFieldModel.ElevateObjects();
                switch (_mainMode)
                {
                    case MainMode.Main:
                        _onStageClear.OnNext(Unit.Default);
                        break;
                    case MainMode.Verifying:
                        _onVerified.OnNext(Unit.Default);
                        break;
                }
            }
            else
            {
                if (_mainFieldModel.ElevateObjects())
                {
                    yield return new WaitForSeconds(0.5f);
                }
                _mainFieldInputs.SetActive(true);
            }
        }
    }
}