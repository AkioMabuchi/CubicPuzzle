using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public interface ICanvasSettings
{
    IObservable<float> OnValueChangedMusic
    {
        get;
    }

    IObservable<float> OnValueChangedSound
    {
        get;
    }

    IObservable<Unit> OnPointerUpSound
    {
        get;
    }
    IObservable<int> OnClickFloorHeight
    {
        get;
    }

    IObservable<Unit> OnClickFinish
    {
        get;
    }

    public void FadeIn();
    public void FadeOut();
    public void Hide();
    public void SetFloorHeight(int floorHeight);
    public void SetMusicVolume(float value);
    public void SetSoundVolume(float value);
}

public class CanvasSettings : CanvasMonoBehaviour, ICanvasSettings
{
    private GameObject _gameObjectGroup;
    private GameObject _gameObjectImageBackground;
    private GameObject _gameObjectImageForm;
    private GameObject _gameObjectGroupElements;
    private GameObject _gameObjectSliderMusic;
    private GameObject _gameObjectSliderSound;
    private readonly GameObject[] _gameObjectsImagesButtonFloorHeight = new GameObject[3];
    private readonly GameObject[] _gameObjectsTextMeshProsButtonFloorHeight = new GameObject[3];

    private Image _imageBackground;
    private Image _imageForm;
    private Slider _sliderMusic;
    private Slider _sliderSound;
    private readonly Image[] _imagesButtonFloorHeight = new Image[3];
    private readonly TextMeshProUGUI[] _textMeshProsButtonFloorHeight = new TextMeshProUGUI[3];
    

    private readonly Subject<float> _onValueChangedMusic = new Subject<float>();
    private readonly Subject<float> _onValueChangedSound = new Subject<float>();
    private readonly Subject<Unit> _onPointerUpSound = new Subject<Unit>();
    private readonly Subject<int> _onClickFloorHeight = new Subject<int>();
    private readonly Subject<Unit> _onClickFinish = new Subject<Unit>();

    public IObservable<float> OnValueChangedMusic => _onValueChangedMusic;
    public IObservable<float> OnValueChangedSound => _onValueChangedSound;
    public IObservable<Unit> OnPointerUpSound => _onPointerUpSound;
    public IObservable<int> OnClickFloorHeight => _onClickFloorHeight;
    public IObservable<Unit> OnClickFinish => _onClickFinish;

    private int _floorHeight;
    private void OnEnable()
    {
        _gameObjectGroup = gameObject.transform.Find("Group").gameObject;
        _gameObjectImageBackground = _gameObjectGroup.transform.Find("ImageBackground").gameObject;
        _gameObjectImageForm = _gameObjectImageBackground.transform.Find("ImageForm").gameObject;
        _gameObjectGroupElements = _gameObjectImageForm.transform.Find("GroupElements").gameObject;

        _gameObjectSliderMusic = _gameObjectGroupElements.transform.Find("SliderMusic").gameObject;
        _gameObjectSliderSound = _gameObjectGroupElements.transform.Find("SliderSound").gameObject;
        
        _imageBackground = _gameObjectImageBackground.GetComponent<Image>();
        _imageForm = _gameObjectImageForm.GetComponent<Image>();

        _sliderMusic = _gameObjectSliderMusic.GetComponent<Slider>();
        _sliderSound = _gameObjectSliderSound.GetComponent<Slider>();
        
        for (int i = 0; i < 3; i++)
        {
            _gameObjectsImagesButtonFloorHeight[i] =
                _gameObjectGroupElements.transform.Find("ImageButtonFloorHeight (" + i + ")").gameObject;
            _gameObjectsTextMeshProsButtonFloorHeight[i]
                = _gameObjectsImagesButtonFloorHeight[i].transform.Find("TextMeshPro").gameObject;

            _imagesButtonFloorHeight[i] = _gameObjectsImagesButtonFloorHeight[i].GetComponent<Image>();
            _textMeshProsButtonFloorHeight[i] =
                _gameObjectsTextMeshProsButtonFloorHeight[i].GetComponent<TextMeshProUGUI>();
        }
    }

    public void FadeIn()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_imageBackground.DOColor(new Color(0.0f, 0.0f, 0.0f, 0.8f), 0.2f).SetEase(Ease.Linear));
        sequence.Append(_gameObjectImageForm.transform.DOScale(new Vector3(0.1f,0.1f,1.0f),0.1f));
        sequence.Append(_gameObjectImageForm.transform.DOScaleX(1.0f, 0.2f));
        sequence.Append(_gameObjectImageForm.transform.DOScaleY(1.0f, 0.2f));
        sequence.OnComplete(() =>
        {
            _gameObjectGroupElements.SetActive(true);
        });
        
        _gameObjectGroup.SetActive(true);
    }

    public void FadeOut()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_gameObjectImageForm.transform.DOScaleY(0.1f, 0.2f));
        sequence.Append(_gameObjectImageForm.transform.DOScaleX(0.1f, 0.2f));
        sequence.Append(_gameObjectImageForm.transform.DOScale(new Vector3(0.0f, 0.0f, 1.0f), 0.1f));
        sequence.Append(_imageBackground.DOColor(new Color(0.0f, 0.0f, 0.0f, 0.0f), 0.2f).SetEase(Ease.Linear));
        sequence.OnComplete(() =>
        {
            _gameObjectGroup.SetActive(false);
        });
        
        _gameObjectGroupElements.SetActive(false);
    }

    public void Hide()
    {
        _imageBackground.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        _gameObjectImageForm.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
        _gameObjectGroupElements.SetActive(false);
        _gameObjectGroup.SetActive(false);
    }

    public void SetFloorHeight(int floorHeight)
    {
        _floorHeight = floorHeight;
        for (int i = 0; i < 3; i++)
        {
            if (i == floorHeight)
            {
                _imagesButtonFloorHeight[i].color = new Color(0.0f, 0.5f, 1.0f);
                _textMeshProsButtonFloorHeight[i].color = new Color(0.5f, 0.75f, 1.0f);
            }
            else
            {
                _imagesButtonFloorHeight[i].color = new Color(0.0f, 0.0f, 0.0f);
                _textMeshProsButtonFloorHeight[i].color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    public void OnClickButtonFinish()
    {
        _onClickFinish.OnNext(Unit.Default);
    }

    public void SetMusicVolume(float value)
    {
        _sliderMusic.value = value;
    }

    public void SetSoundVolume(float value)
    {
        _sliderSound.value = value;
    }

    public void OnValueChangedSliderMusic()
    {
        _onValueChangedMusic.OnNext(_sliderMusic.value);
    }

    public void OnValueChangedSliderSound()
    {
        _onValueChangedSound.OnNext(_sliderSound.value);
    }

    public void OnPointerUpSliderSound()
    {
        _onPointerUpSound.OnNext(Unit.Default);
    }
    
    public void OnPointerEnterFloorHeight(int index)
    {
        if (index != _floorHeight)
        {
            _imagesButtonFloorHeight[index].color = new Color(0.3f, 0.3f, 0.3f);
        }
    }

    public void OnPointerExitFloorHeight(int index)
    {
        if (index != _floorHeight)
        {
            _imagesButtonFloorHeight[index].color = new Color(0.0f, 0.0f, 0.0f);
        }
    }

    public void OnPointerDownFloorHeight(int index)
    {
        if (index != _floorHeight)
        {
            _onClickFloorHeight.OnNext(index);
        }
    }
}
