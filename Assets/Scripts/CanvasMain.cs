using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ICanvasMain
{
    IObservable<Unit> OnClickPause
    {
        get;
    }

    IObservable<Unit> OnClickReset
    {
        get;
    }

    public void Show();
    public void Hide();
}

public class CanvasMain : CanvasMonoBehaviour, ICanvasMain
{
    private GameObject _gameObjectGroup;
    
    private readonly Subject<Unit> _onClickPause = new Subject<Unit>();
    private readonly Subject<Unit> _onClickReset = new Subject<Unit>();
    public IObservable<Unit> OnClickPause => _onClickPause;
    public IObservable<Unit> OnClickReset => _onClickReset;

    private void OnEnable()
    {
        _gameObjectGroup = gameObject.transform.Find("Group").gameObject;
    }

    public void Show()
    {
        _gameObjectGroup.SetActive(true);
    }

    public void Hide()
    {
        _gameObjectGroup.SetActive(false);
    }

    public void OnClickButtonPause()
    {
        _onClickPause.OnNext(Unit.Default);
    }

    public void OnClickButtonReset()
    {
        _onClickReset.OnNext(Unit.Default);
    }
}
