using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public interface ICanvasStageClear
{
    IObservable<Unit> OnClickStageSelect
    {
        get;
    }

    public void Show();
    public void Hide();
}
public class CanvasStageClear : MonoBehaviour, ICanvasStageClear
{
    private GameObject _gameObjectGroup;

    private readonly Subject<Unit> _onClickStageSelect = new Subject<Unit>();
    public IObservable<Unit> OnClickStageSelect => _onClickStageSelect;

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

    public void OnClickButtonStageSelect()
    {
        _onClickStageSelect.OnNext(Unit.Default);
    }
}
