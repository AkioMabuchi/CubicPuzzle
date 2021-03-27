using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public interface ICanvasStageSelect
{
    public IObservable<Unit> OnFormShown
    {
        get;
    }
    public IObservable<Stage> OnClickPlay
    {
        get;
    }

    public IObservable<Unit> OnClickReturn
    {
        get;
    }

    public void FadeIn();
    public void Appear();
    public void Show();
    public void Hide();
    public void SetStages(Stage[] stages);
    public void LoadingError();
}
public class CanvasStageSelect : CanvasMonoBehaviour, ICanvasStageSelect
{
    private GameObject _gameObjectGroup;
    private GameObject _gameObjectImageBackground;
    private GameObject _gameObjectImageSelectForm;
    private GameObject _gameObjectGroupElements;
    private GameObject _gameObjectButtonPrev;
    private GameObject _gameObjectButtonNext;
    private GameObject _gameObjectButtonRandom;
    private GameObject _gameObjectTextMeshProLoading;

    private GameObject _gameObjectButtonReturn;
    
    private CanvasGroup _canvasGroup;

    private Button _buttonPrev;
    private Button _buttonNext;
    private Button _buttonRandom;

    private TextMeshProUGUI _textMeshProLoading;
    
    private readonly GameObject[] _gameObjectsButtonsPlay = new GameObject[10];
    private readonly GameObject[] _gameObjectsTextMeshProsTitle = new GameObject[10];
    private readonly GameObject[] _gameObjectsTextMeshProsName = new GameObject[10];

    private readonly TextMeshProUGUI[] _textMeshProsTitle = new TextMeshProUGUI[10];
    private readonly TextMeshProUGUI[] _textMeshProsName = new TextMeshProUGUI[10];
    
    private readonly Subject<Unit> _onFormShown = new Subject<Unit>();
    private readonly Subject<Stage> _onClickPlay = new Subject<Stage>();
    private readonly Subject<Unit> _onClickReturn = new Subject<Unit>();
    public IObservable<Unit> OnFormShown => _onFormShown;
    public IObservable<Stage> OnClickPlay => _onClickPlay;
    public IObservable<Unit> OnClickReturn => _onClickReturn;

    private Stage[] _stages;
    private int _page;

    private void OnEnable()
    {
        _gameObjectGroup = gameObject.transform.Find("Group").gameObject;
        _gameObjectImageBackground = _gameObjectGroup.transform.Find("ImageBackground").gameObject;
        _gameObjectImageSelectForm = _gameObjectImageBackground.transform.Find("ImageSelectForm").gameObject;
        _gameObjectGroupElements = _gameObjectImageSelectForm.transform.Find("GroupElements").gameObject;

        _gameObjectButtonReturn = _gameObjectImageBackground.transform.Find("ButtonReturn").gameObject;
        
        _canvasGroup = _gameObjectGroup.GetComponent<CanvasGroup>();

        _gameObjectButtonPrev = _gameObjectGroupElements.transform.Find("ButtonPrev").gameObject;
        _gameObjectButtonNext = _gameObjectGroupElements.transform.Find("ButtonNext").gameObject;
        _gameObjectButtonRandom = _gameObjectGroupElements.transform.Find("ButtonRandom").gameObject;
        _gameObjectTextMeshProLoading = _gameObjectGroupElements.transform.Find("TextMeshProLoading").gameObject;
        
        _buttonPrev = _gameObjectButtonPrev.GetComponent<Button>();
        _buttonNext = _gameObjectButtonNext.GetComponent<Button>();
        _buttonRandom = _gameObjectButtonRandom.GetComponent<Button>();
        _textMeshProLoading = _gameObjectTextMeshProLoading.GetComponent<TextMeshProUGUI>();
        
        for (int i = 0; i < 10; i++)
        {
            _gameObjectsButtonsPlay[i] = _gameObjectGroupElements.transform.Find("ButtonPlay (" + i + ")").gameObject;
            _gameObjectsTextMeshProsTitle[i] = _gameObjectsButtonsPlay[i].transform.Find("TextMeshProTitle").gameObject;
            _gameObjectsTextMeshProsName[i] = _gameObjectsButtonsPlay[i].transform.Find("TextMeshProName").gameObject;
            _textMeshProsTitle[i] = _gameObjectsTextMeshProsTitle[i].GetComponent<TextMeshProUGUI>();
            _textMeshProsName[i] = _gameObjectsTextMeshProsName[i].GetComponent<TextMeshProUGUI>();
        }
    }

    public void FadeIn()
    {
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear).OnComplete(Appear);
        _gameObjectGroup.SetActive(true);
    }

    public void Appear()
    {
        _canvasGroup.alpha = 1.0f;
        
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(_gameObjectImageSelectForm.transform.DOScale(new Vector3(0.1f,0.1f,1.0f),0.1f));
        sequence.Append(_gameObjectImageSelectForm.transform.DOScaleX(1.0f, 0.2f));
        sequence.Append(_gameObjectImageSelectForm.transform.DOScaleY(1.0f, 0.2f));
        sequence.OnComplete(Show);
        
        _gameObjectGroup.SetActive(true);
    }

    public void Show()
    {
        _gameObjectImageSelectForm.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        for (int i = 0; i < 10; i++)
        {
            _gameObjectsButtonsPlay[i].SetActive(false);
        }
        _page = 0;
        _buttonPrev.interactable = false;
        _buttonNext.interactable = false;
        _buttonRandom.interactable = false;

        _textMeshProLoading.color = new Color(0.8f, 0.8f, 0.8f);
        _textMeshProLoading.text = "ローディング中・・・";

        _gameObjectTextMeshProLoading.SetActive(true);
        _gameObjectButtonReturn.SetActive(true);
        _gameObjectGroup.SetActive(true);
        _gameObjectGroupElements.SetActive(true);
        
        _onFormShown.OnNext(Unit.Default);
    }

    public void Hide()
    {
        _gameObjectButtonReturn.SetActive(false);
        _gameObjectGroup.SetActive(false);
        _gameObjectTextMeshProLoading.SetActive(false);
        
        _gameObjectImageSelectForm.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
        _gameObjectGroupElements.SetActive(false);
    }

    public void SetStages(Stage[] stages)
    {
        _gameObjectTextMeshProLoading.SetActive(false);
        _stages = stages;
        DrawUIs();
    }

    public void LoadingError()
    {
        _textMeshProLoading.color = new Color(1.0f, 0.4f, 0.4f);
        _textMeshProLoading.text = "ローディングに失敗しました";
    }

    public void OnClickButtonReturn()
    {
        _onClickReturn.OnNext(Unit.Default);
    }

    public void OnClickButtonPrev()
    {
        _page--;
        DrawUIs();
    }
    
    public void OnClickButtonNext()
    {
        _page++;
        DrawUIs();
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
