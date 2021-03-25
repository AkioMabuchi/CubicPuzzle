using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public enum MoveDirection
{
    Up,
    Right,
    Down,
    Left
}
public interface IMainFieldInputs
{
    public IObservable<MoveDirection> MoveCommand
    {
        get;
    }

    public void SetActive(bool s);
}
public class MainFieldInputs : MonoBehaviour, IMainFieldInputs
{
    private readonly Subject<MoveDirection> _moveCommand = new Subject<MoveDirection>();

    public IObservable<MoveDirection> MoveCommand => _moveCommand;

    private bool _isActive;
    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => _isActive)
            .Where(_ => Input.GetKeyDown(KeyCode.UpArrow))
            .Subscribe(_ =>
            {
                _moveCommand.OnNext(MoveDirection.Up);
            });
        
        this.UpdateAsObservable()
            .Where(_ => _isActive)
            .Where(_ => Input.GetKeyDown(KeyCode.LeftArrow))
            .Subscribe(_ =>
            {
                _moveCommand.OnNext(MoveDirection.Left);
            });
        
        this.UpdateAsObservable()
            .Where(_ => _isActive)
            .Where(_ => Input.GetKeyDown(KeyCode.DownArrow))
            .Subscribe(_ =>
            {
                _moveCommand.OnNext(MoveDirection.Down);
            });
        
        this.UpdateAsObservable()
            .Where(_ => _isActive)
            .Where(_ => Input.GetKeyDown(KeyCode.RightArrow))
            .Subscribe(_ =>
            {
                _moveCommand.OnNext(MoveDirection.Right);
            });
    }

    public void SetActive(bool s)
    {
        _isActive = s;
    }
}
