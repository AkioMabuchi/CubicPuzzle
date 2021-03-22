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
    private IMainFieldInputs _mainFieldInputs;

    private readonly Subject<Unit> _onStageClear = new Subject<Unit>();
    private readonly Subject<Unit> _onVerified = new Subject<Unit>();

    public IObservable<Unit> OnStageClear => _onStageClear;
    public IObservable<Unit> OnVerified => _onVerified;
    
    private MainMode _mainMode = MainMode.Idle;
    private void OnEnable()
    {
        _mainFieldInputs = GameObject.Find("MainFieldInputs").GetComponent<IMainFieldInputs>();
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
        if (MainFieldModel.MoveObjects(direction))
        {
            _mainFieldInputs.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            
            MainFieldModel.FallObjects();

            if (MainFieldModel.IsClear() && (_mainMode == MainMode.Main || _mainMode == MainMode.Verifying))
            {
                MainFieldModel.ElevateObjects();
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
                if (MainFieldModel.ElevateObjects())
                {
                    yield return new WaitForSeconds(0.5f);
                }
                _mainFieldInputs.SetActive(true);
            }
        }
    }
}