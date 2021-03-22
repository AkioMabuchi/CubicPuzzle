using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ICanvasTitle
{
    public IObservable<Unit> OnClickStart
    {
        get;
    }

    public IObservable<Unit> OnClickEdit
    {
        get;
    }

    public IObservable<Unit> OnClickSettings
    {
        get;
    }

    public void Show();
    public void Hide();
}
public class CanvasTitle : CanvasMonoBehaviour, ICanvasTitle
{
    private GameObject _gameObjectGroup;

    private readonly Subject<Unit> _onClickStart = new Subject<Unit>();
    private readonly Subject<Unit> _onClickEdit = new Subject<Unit>();
    private readonly Subject<Unit> _onClickSettings = new Subject<Unit>();

    public IObservable<Unit> OnClickStart => _onClickStart;
    public IObservable<Unit> OnClickEdit => _onClickEdit;
    public IObservable<Unit> OnClickSettings => _onClickSettings;
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

    public void OnClickButtonStart()
    {
        _onClickStart.OnNext(Unit.Default);
    }

    public void OnClickButtonEdit()
    {
        _onClickEdit.OnNext(Unit.Default);
    }

    public void OnClickButtonSettings()
    {
        _onClickSettings.OnNext(Unit.Default);
    }
}
