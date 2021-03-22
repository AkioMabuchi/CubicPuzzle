using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface ICanvasEditor
{
    public IObservable<int> OnClickMap
    {
        get;
    }
    public IObservable<int> OnClickTool
    {
        get;
    }
    public IObservable<Unit> OnClickReturn
    {
        get;
    }
    public IObservable<Unit> OnClickRun
    {
        get;
    }
    public IObservable<Unit> OnClickUpload
    {
        get;
    }

    public void Show();
    public void Hide();
    public void DrawMap(EditorMap editorMap);
    public void DrawTools(int index);
    public void VerifyMap(EditorMap editorMap);
}

public interface IMapClickable
{
    public void OnClickButtonMap(int index);
}
public class CanvasEditor : CanvasMonoBehaviour, ICanvasEditor, IMapClickable
{
    [SerializeField] private GameObject prefabImageButtonMap;

    [SerializeField] private Sprite spriteMapFloorNothing;
    [SerializeField] private Sprite spriteMapFloorWhite;
    [SerializeField] private Sprite spriteMapFloorYellow;
    [SerializeField] private Sprite spriteMapFloorBlue;
    [SerializeField] private Sprite spriteMapLevel0;
    [SerializeField] private Sprite spriteMapLevel1;
    [SerializeField] private Sprite spriteMapLevel2;
    [SerializeField] private Sprite spriteMapLevel3;
    [SerializeField] private Sprite spriteMapObjectPlayer;
    [SerializeField] private Sprite spriteMapObjectFlag;
    [SerializeField] private Sprite spriteMapObjectBox1;
    [SerializeField] private Sprite spriteMapObjectBox2;
    [SerializeField] private Sprite spriteMapObjectBox3;
    [SerializeField] private Sprite spriteMapObjectSwitchYellowUp;
    [SerializeField] private Sprite spriteMapObjectSwitchYellowDown;
    [SerializeField] private Sprite spriteMapObjectSwitchBlueUp;
    [SerializeField] private Sprite spriteMapObjectSwitchBlueDown;
    [SerializeField] private Sprite spriteMapDummy;
    
    private GameObject _gameObjectBackground;

    private GameObject _gameObjectImageBackground;
    private GameObject _gameObjectImageMapForm;
    private GameObject _gameObjectImageToolForm;
    private GameObject _gameObjectButtonUpload;

    private CanvasGroup _canvasGroupBackground;

    private Button _buttonUpload;
    
    private readonly GameObject[] _gameObjectsImagesMapPanel = new GameObject[169];
    private readonly GameObject[] _gameObjectsImagesMapPanelLevel = new GameObject[169];
    private readonly GameObject[] _gameObjectsImagesMapPanelObjects = new GameObject[169];

    private readonly GameObject[] _gameObjectsImagesTool = new GameObject[18];
    
    private readonly Image[] _imagesMapPanel = new Image[169];
    private readonly Image[] _imagesMapPanelLevel = new Image[169];
    private readonly Image[] _imagesMapPanelObjects = new Image[169];

    private readonly Image[] _imagesTool = new Image[18];
    
    private GameObject _gameObjectInputs;
    private GameObject _gameObjectMap;
    
    private readonly Subject<int> _onClickMap = new Subject<int>();
    private readonly Subject<int> _onClickTool = new Subject<int>();
    private readonly Subject<Unit> _onClickReturn = new Subject<Unit>();
    private readonly Subject<Unit> _onClickRun = new Subject<Unit>();
    private readonly Subject<Unit> _onClickUpload = new Subject<Unit>();
    
    public IObservable<int> OnClickMap => _onClickMap;
    public IObservable<int> OnClickTool => _onClickTool;
    public IObservable<Unit> OnClickReturn => _onClickReturn;
    public IObservable<Unit> OnClickRun => _onClickRun;
    public IObservable<Unit> OnClickUpload => _onClickUpload;

    private bool _isActive = true;
    
    private void OnEnable()
    {
        _gameObjectInputs = gameObject.transform.Find("Inputs").gameObject;
        _gameObjectMap = _gameObjectInputs.transform.Find("Map").gameObject;

        _gameObjectBackground = gameObject.transform.Find("Background").gameObject;
        _gameObjectImageBackground = _gameObjectBackground.transform.Find("ImageBackground").gameObject;
        _gameObjectImageMapForm = _gameObjectImageBackground.transform.Find("ImageMapForm").gameObject;
        
        for (int i = 0; i < 169; i++)
        {
            _gameObjectsImagesMapPanel[i] =
                _gameObjectImageMapForm.transform.Find("ImageMapPanel (" + i + ")").gameObject;
            _gameObjectsImagesMapPanelLevel[i]
                = _gameObjectsImagesMapPanel[i].transform.Find("ImageLevel").gameObject;
            _gameObjectsImagesMapPanelObjects[i]
                = _gameObjectsImagesMapPanel[i].transform.Find("ImageObject").gameObject;

            _imagesMapPanel[i] = _gameObjectsImagesMapPanel[i].GetComponent<Image>();
            _imagesMapPanelLevel[i] = _gameObjectsImagesMapPanelLevel[i].GetComponent<Image>();
            _imagesMapPanelObjects[i] = _gameObjectsImagesMapPanelObjects[i].GetComponent<Image>();
        }

        _gameObjectImageToolForm = _gameObjectImageBackground.transform.Find("ImageToolForm").gameObject;

        for (int i = 0; i < 18; i++)
        {
            _gameObjectsImagesTool[i]
                = _gameObjectImageToolForm.transform.Find("ImagePart (" + i + ")").gameObject;

            _imagesTool[i] = _gameObjectsImagesTool[i].GetComponent<Image>();
        }

        _gameObjectButtonUpload = _gameObjectImageBackground.transform.Find("ButtonUpload").gameObject;

        _buttonUpload = _gameObjectButtonUpload.GetComponent<Button>();
        _canvasGroupBackground = _gameObjectBackground.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        IMapClickable mapClickable = GetComponent<IMapClickable>();
        for (int i = 0; i < 169; i++)
        {
            float positionX = i % 13 * 70.0f - 420.0f;
            float positionY = i / 13 * 70.0f - 420.0f;
            GameObject prefab = Instantiate(prefabImageButtonMap, _gameObjectMap.transform, true);
            prefab.transform.localPosition = new Vector3(positionX, positionY, 0.0f);
            prefab.transform.localScale = Vector3.one;
            prefab.GetComponent<IImageButtonMap>().Initialize(mapClickable, i);
        }
        
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ =>
            {
                _canvasGroupBackground.DOFade(0.0f, 0.3f);
            });
        
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyUp(KeyCode.Space))
            .Subscribe(_ =>
            {
                _canvasGroupBackground.DOFade(1.0f, 0.3f);
            });
    }

    public void Show()
    {
        _isActive = true;
        _gameObjectBackground.SetActive(true);
        _gameObjectInputs.SetActive(true);
    }

    public void Hide()
    {
        _isActive = false;
        _gameObjectBackground.SetActive(false);
        _gameObjectInputs.SetActive(false);
    }
    public void OnClickButtonMap(int index)
    {
        if (_isActive)
        {
            _onClickMap.OnNext(index);
        }
    }

    public void OnClickButtonTool(int index)
    {
        if (_isActive)
        {
            _onClickTool.OnNext(index);
        }
    }

    public void OnClickButtonReturn()
    {
        if (_isActive)
        {
            _onClickReturn.OnNext(Unit.Default);
        }
    }
    
    public void OnClickButtonRun()
    {
        if (_isActive)
        {
            _onClickRun.OnNext(Unit.Default);
        }
    }

    public void OnClickButtonUpload()
    {
        if (_isActive)
        {
            _onClickUpload.OnNext(Unit.Default);
        }
    }

    public void DrawMap(EditorMap map)
    {
        for (int i = 0; i < 169; i++)
        {
            switch (map.floors[i])
            {
                case '_':
                    _imagesMapPanel[i].sprite = spriteMapFloorNothing;
                    break;
                case 'W':
                    _imagesMapPanel[i].sprite = spriteMapFloorWhite;
                    break;
                case 'Y':
                    _imagesMapPanel[i].sprite = spriteMapFloorYellow;
                    break;
                case 'B':
                    _imagesMapPanel[i].sprite = spriteMapFloorBlue;
                    break;
            }

            switch (map.levels[i])
            {
                case '0':
                    _imagesMapPanelLevel[i].sprite = spriteMapLevel0;
                    break;
                case '1':
                    _imagesMapPanelLevel[i].sprite = spriteMapLevel1;
                    break;
                case '2':
                    _imagesMapPanelLevel[i].sprite = spriteMapLevel2;
                    break;
                case '3':
                    _imagesMapPanelLevel[i].sprite = spriteMapLevel3;
                    break;
            }

            switch (map.objects[i])
            {
                case '.':
                    _imagesMapPanelObjects[i].sprite = spriteMapDummy;
                    break;
                case 'P':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectPlayer;
                    break;
                case 'F':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectFlag;
                    break;
                case '1':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectBox1;
                    break;
                case '2':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectBox2;
                    break;
                case '3':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectBox3;
                    break;
                case 'Y':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectSwitchYellowUp;
                    break;
                case 'y':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectSwitchYellowDown;
                    break;
                case 'B':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectSwitchBlueUp;
                    break;
                case 'b':
                    _imagesMapPanelObjects[i].sprite = spriteMapObjectSwitchBlueDown;
                    break;
            }

            _gameObjectsImagesMapPanelLevel[i].SetActive(map.floors[i] != '_');
        }
    }

    public void DrawTools(int index)
    {
        for (int i = 0; i < 18; i++)
        {
            if (i == index)
            {
                _imagesTool[i].color = new Color(0.0f, 1.0f, 0.25f);
            }
            else
            {
                _imagesTool[i].color = new Color(0.4f, 0.4f, 0.4f);
            }
        }
    }

    public void VerifyMap(EditorMap editorMap)
    {
        bool hasPlayer = false;
        bool hasFlag = false;
        for (int i = 0; i < 169; i++)
        {
            if (editorMap.objects[i] == 'P')
            {
                hasPlayer = true;
            }

            if (editorMap.objects[i] == 'F')
            {
                hasFlag = true;
            }
        }

        if (hasPlayer && hasFlag)
        {
            _buttonUpload.interactable = true;
        }
        else
        {
            _buttonUpload.interactable = false;
        }
    }
}
