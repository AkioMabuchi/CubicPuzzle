using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public interface ICanvasStageSelect
{
    public IObservable<Stage> OnClickPlay
    {
        get;
    }

    public IObservable<Unit> OnClickReturn
    {
        get;
    }

    public void Show();
    public void Hide();
    public void SetStages(Stage[] stages);
}
public class CanvasStageSelect : CanvasMonoBehaviour, ICanvasStageSelect
{
    private GameObject _gameObjectGroup;
    private GameObject _gameObjectImageBackground;
    private GameObject _gameObjectImageSelectForm;
    private GameObject _gameObjectButtonPrev;
    private GameObject _gameObjectButtonNext;
    private GameObject _gameObjectButtonRandom;

    private Button _buttonPrev;
    private Button _buttonNext;
    private Button _buttonRandom;
    
    private readonly GameObject[] _gameObjectsButtonsPlay = new GameObject[10];
    private readonly GameObject[] _gameObjectsTextMeshProsTitle = new GameObject[10];
    private readonly GameObject[] _gameObjectsTextMeshProsName = new GameObject[10];

    private readonly TextMeshProUGUI[] _textMeshProsTitle = new TextMeshProUGUI[10];
    private readonly TextMeshProUGUI[] _textMeshProsName = new TextMeshProUGUI[10];
    
    private readonly Subject<Stage> _onClickPlay = new Subject<Stage>();
    private readonly Subject<Unit> _onClickReturn = new Subject<Unit>();
    public IObservable<Stage> OnClickPlay => _onClickPlay;
    public IObservable<Unit> OnClickReturn => _onClickReturn;

    private Stage[] _stages;
    private int _page;

    private void OnEnable()
    {
        _gameObjectGroup = gameObject.transform.Find("Group").gameObject;
        _gameObjectImageBackground = _gameObjectGroup.transform.Find("ImageBackground").gameObject;
        _gameObjectImageSelectForm = _gameObjectImageBackground.transform.Find("ImageSelectForm").gameObject;
        
        _gameObjectButtonPrev = _gameObjectImageSelectForm.transform.Find("ButtonPrev").gameObject;
        _gameObjectButtonNext = _gameObjectImageSelectForm.transform.Find("ButtonNext").gameObject;
        _gameObjectButtonRandom = _gameObjectImageSelectForm.transform.Find("ButtonRandom").gameObject;
        
        _buttonPrev = _gameObjectButtonPrev.GetComponent<Button>();
        _buttonNext = _gameObjectButtonNext.GetComponent<Button>();
        _buttonRandom = _gameObjectButtonRandom.GetComponent<Button>();
        
        for (int i = 0; i < 10; i++)
        {
            _gameObjectsButtonsPlay[i] = _gameObjectImageSelectForm.transform.Find("ButtonPlay (" + i + ")").gameObject;
            _gameObjectsTextMeshProsTitle[i] = _gameObjectsButtonsPlay[i].transform.Find("TextMeshProTitle").gameObject;
            _gameObjectsTextMeshProsName[i] = _gameObjectsButtonsPlay[i].transform.Find("TextMeshProName").gameObject;
            _textMeshProsTitle[i] = _gameObjectsTextMeshProsTitle[i].GetComponent<TextMeshProUGUI>();
            _textMeshProsName[i] = _gameObjectsTextMeshProsName[i].GetComponent<TextMeshProUGUI>();
        }
    }

    public void Show()
    {
        for (int i = 0; i < 10; i++)
        {
            _gameObjectsButtonsPlay[i].SetActive(false);
        }
        _page = 0;
        _buttonPrev.interactable = false;
        _buttonNext.interactable = false;
        _buttonRandom.interactable = false;
        
        _gameObjectGroup.SetActive(true);
    }

    public void Hide()
    {
        _gameObjectGroup.SetActive(false);
    }

    public void SetStages(Stage[] stages)
    {
        _stages = stages;
        DrawUIs();
    }

    public void OnClickButtonReturn()
    {
        _onClickReturn.OnNext(Unit.Default);
    }

    public void OnClickButtonPrev()
    {
        _page--;
    }
    
    public void OnClickButtonNext()
    {
        _page++;
    }

    public void OnClickButtonRandom()
    {
        if (_stages.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, _stages.Length);
            _onClickPlay.OnNext(_stages[index]);
        }
    }

    public void OnClickButtonPlay(int index)
    {
        int stageIndex = _page * 10 + index;
        if (stageIndex < _stages.Length)
        {
            _onClickPlay.OnNext(_stages[stageIndex]);
        }
    }

    void DrawUIs()
    {
        for (int i = 0; i < 10; i++)
        {
            int index = _page * 10 + i;
            if (index < _stages.Length)
            {
                _textMeshProsTitle[i].text = _stages[index].title;
                _textMeshProsName[i].text = _stages[index].name;
                _gameObjectsButtonsPlay[i].SetActive(true);
            }
            else
            {
                _gameObjectsButtonsPlay[i].SetActive(false);
            }
        }
        _buttonPrev.interactable = _page > 0;
        _buttonNext.interactable = _stages.Length > (_page + 1) * 10;
        _buttonRandom.interactable = _stages.Length > 0;
    }
}
