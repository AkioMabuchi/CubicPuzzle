using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public interface ICanvasVerified
{
    public IObservable<Unit> OnClickReturn
    {
        get;
    }
    public IObservable<Stage> OnClickUpload
    {
        get;
    }
    public IObservable<Unit> OnClickTitle
    {
        get;
    }
    public void Show();
    public void Hide();
    public void ShowSuccessMessage();
    public void ShowErrorMessage();
    public void SetEditorMap(EditorMap editorMap);
}
public class CanvasVerified : CanvasMonoBehaviour, ICanvasVerified
{
    private GameObject _gameObjectGroup;

    private GameObject _gameObjectImageBackground;
    
    private GameObject _gameObjectButtonReturn;
    private GameObject _gameObjectImageUploadForm;
    private GameObject _gameObjectImageSuccessForm;
    private GameObject _gameObjectImageErrorForm;
    
    private GameObject _gameObjectInputFieldTitle;
    private GameObject _gameObjectInputFieldName;

    private TMP_InputField _inputFieldTitle;
    private TMP_InputField _inputFieldName;
    
    private readonly Subject<Unit> _onClickReturn = new Subject<Unit>();
    private readonly Subject<Stage> _onClickUpload = new Subject<Stage>();
    private readonly Subject<Unit> _onClickTitle = new Subject<Unit>();
    public IObservable<Unit> OnClickReturn => _onClickReturn;
    public IObservable<Stage> OnClickUpload => _onClickUpload;
    public IObservable<Unit> OnClickTitle => _onClickTitle;

    private EditorMap _editorMap;
    private void OnEnable()
    {
        _gameObjectGroup = gameObject.transform.Find("Group").gameObject;
        _gameObjectImageBackground = _gameObjectGroup.transform.Find("ImageBackground").gameObject;

        _gameObjectButtonReturn = _gameObjectImageBackground.transform.Find("ButtonReturn").gameObject;
        _gameObjectImageUploadForm = _gameObjectImageBackground.transform.Find("ImageUploadForm").gameObject;
        _gameObjectImageSuccessForm = _gameObjectImageBackground.transform.Find("ImageSuccessForm").gameObject;
        _gameObjectImageErrorForm = _gameObjectImageBackground.transform.Find("ImageErrorForm").gameObject;
        
        _gameObjectInputFieldTitle = _gameObjectImageUploadForm.transform.Find("InputFieldTitle").gameObject;
        _gameObjectInputFieldName = _gameObjectImageUploadForm.transform.Find("InputFieldName").gameObject;

        _inputFieldTitle = _gameObjectInputFieldTitle.GetComponent<TMP_InputField>();
        _inputFieldName = _gameObjectInputFieldName.GetComponent<TMP_InputField>();
    }

    public void Show()
    {
        _gameObjectGroup.SetActive(true);
        _gameObjectButtonReturn.SetActive(true);
        _gameObjectImageUploadForm.SetActive(true);
        _gameObjectImageSuccessForm.SetActive(false);
        _gameObjectImageErrorForm.SetActive(false);
    }

    public void Hide()
    {
        _gameObjectGroup.SetActive(false);
    }

    public void ShowSuccessMessage()
    {
        _gameObjectImageSuccessForm.SetActive(true);
    }

    public void ShowErrorMessage()
    {
        _gameObjectImageErrorForm.SetActive(true);
    }

    public void SetEditorMap(EditorMap editorMap)
    {
        _editorMap = editorMap;
    }

    public void OnClickButtonReturn()
    {
        _onClickReturn.OnNext(Unit.Default);
    }
    public void OnClickButtonUpload()
    {
        _gameObjectButtonReturn.SetActive(false);
        _gameObjectImageUploadForm.SetActive(false);
        
        string title = _inputFieldTitle.text;
        string userName = _inputFieldName.text;
        string floors = "";
        string levels = "";
        string objects = "";

        if (title == "")
        {
            title = "(No Title)";
        }

        if (userName == "")
        {
            userName = "(No Name)";
        }

        for (int i = 0; i < 169; i++)
        {
            floors += _editorMap.floors[i];
            levels += _editorMap.levels[i];
            objects += _editorMap.objects[i];
        }

        Stage stage = new Stage
        {
            title = title,
            name = userName,
            floors = floors,
            levels = levels,
            objects = objects
        };

        _onClickUpload.OnNext(stage);
    }

    public void OnClickButtonTitle()
    {
        _onClickTitle.OnNext(Unit.Default);
    }
}
