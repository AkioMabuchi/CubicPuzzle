using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UniRx;
using UnityEngine;


public interface ICanvasRunning
{
    public IObservable<Unit> OnClickPause
    {
        get;
    }
    public void Show();
    public void Hide();
}
public class CanvasRunning : CanvasMonoBehaviour, ICanvasRunning
{
    private GameObject _gameObjectGroup;
    
    private readonly Subject<Unit> _onClickPause = new Subject<Unit>();

    public IObservable<Unit> OnClickPause => _onClickPause;

    private bool _isActive;
    private void OnEnable()
    {
        _gameObjectGroup = gameObject.transform.Find("Group").gameObject;
    }

    public void Show()
    {
        _isActive = true;
        _gameObjectGroup.SetActive(true);
    }

    public void Hide()
    {
        _isActive = false;
        _gameObjectGroup.SetActive(false);
    }

    public void OnClickButtonPause()
    {
        if (_isActive)
        {
            _onClickPause.OnNext(Unit.Default);
        }
    }
}
