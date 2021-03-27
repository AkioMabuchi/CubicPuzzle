using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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

    IObservable<Unit> OnEndStageClear
    {
        get;
    }

    public void Show();
    public void Hide();
    public void StageClear();
}
public class CanvasStageClear : MonoBehaviour, ICanvasStageClear
{
    private GameObject _gameObjectGroup;
    private GameObject _gameObjectImageBackground;
    private GameObject _gameObjectImageBanner;
    private GameObject _gameObjectTextMeshProStageClear;
    private GameObject _gameObjectTextMeshProStageClearEffect;

    private CanvasGroup _canvasGroup;
    private TextMeshProUGUI _textMeshProStageClearEffect;

    private readonly Subject<Unit> _onClickStageSelect = new Subject<Unit>();
    private readonly Subject<Unit> _onEndStageClear = new Subject<Unit>();
    public IObservable<Unit> OnClickStageSelect => _onClickStageSelect;
    public IObservable<Unit> OnEndStageClear => _onEndStageClear;

    private void OnEnable()
    {
        _gameObjectGroup = gameObject.transform.Find("Group").gameObject;
        _gameObjectImageBackground = _gameObjectGroup.transform.Find("ImageBackground").gameObject;
        _gameObjectImageBanner = _gameObjectImageBackground.transform.Find("ImageBanner").gameObject;
        _gameObjectTextMeshProStageClear = _gameObjectImageBanner.transform.Find("TextMeshProStageClear").gameObject;
        _gameObjectTextMeshProStageClearEffect =
            _gameObjectTextMeshProStageClear.transform.Find("TextMeshProStageClearEffect").gameObject;

        _canvasGroup = _gameObjectGroup.GetComponent<CanvasGroup>();
        _textMeshProStageClearEffect = _gameObjectTextMeshProStageClearEffect.GetComponent<TextMeshProUGUI>();
    }

    public void Show()
    {
        _gameObjectGroup.SetActive(true);
    }

    public void Hide()
    {
        _gameObjectGroup.SetActive(false);
        
        _canvasGroup.alpha = 0.0f;
        _gameObjectImageBanner.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        _gameObjectTextMeshProStageClear.transform.localPosition = new Vector3(1920.0f,0.0f,0.0f);
        _gameObjectTextMeshProStageClearEffect.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        _textMeshProStageClearEffect.color = new Color(0.5625f, 0.75f, 0.0f, 1.0f);
    }

    public void StageClear()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_canvasGroup.DOFade(1.0f, 0.5f));
        sequence.Append(_gameObjectImageBanner.transform.DOScaleY(1.0f, 1.0f).SetEase(Ease.OutQuad));
        sequence.Append(_gameObjectTextMeshProStageClear.transform.DOLocalMoveX(0.0f, 1.0f).SetEase(Ease.OutQuad));
        sequence.Append(_gameObjectTextMeshProStageClearEffect.transform.DOScale(new Vector3(3.0f, 3.0f, 1.0f), 1.0f));
        sequence.Join(_textMeshProStageClearEffect.DOColor(new Color(0.5625f, 0.75f, 0.0f, 0.0f), 1.0f));
        sequence.AppendInterval(1.5f);
        sequence.Append(_gameObjectTextMeshProStageClear.transform.DOLocalMoveX(-1920.0f, 1.0f).SetEase(Ease.InQuad));
        sequence.Append(_gameObjectImageBanner.transform.DOScaleY(0.0f, 1.0f).SetEase(Ease.InQuad));
        sequence.AppendInterval(2.0f);
        sequence.OnComplete(() =>
        {
            _onEndStageClear.OnNext(Unit.Default);
        });

        _gameObjectGroup.SetActive(true);
    }

    public void OnClickButtonStageSelect()
    {
        _onClickStageSelect.OnNext(Unit.Default);
    }
}
