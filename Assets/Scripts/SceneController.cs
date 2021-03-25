using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private ICanvasTitle _canvasTitle;
    private ICanvasStageSelect _canvasStageSelect;
    private ICanvasMain _canvasMain;
    private ICanvasStageClear _canvasStageClear;
    private ICanvasEditor _canvasEditor;
    private ICanvasRunning _canvasRunning;
    private ICanvasVerified _canvasVerified;
    private ICanvasSettings _canvasSettings;
    
    private IMainFieldDrawer _mainFieldDrawer;
    private IMainFieldInputs _mainFieldInputs;
    private IMainFieldController _mainFieldController;
    private IMainFieldModel _mainFieldModel;
    
    private EditorMap _editorMap;
    private EditorMap _mainMap;
    private void OnEnable()
    {
        _canvasTitle = GameObject.Find("CanvasTitle").GetComponent<ICanvasTitle>();
        _canvasStageSelect = GameObject.Find("CanvasStageSelect").GetComponent<ICanvasStageSelect>();
        _canvasMain = GameObject.Find("CanvasMain").GetComponent<ICanvasMain>();
        _canvasStageClear = GameObject.Find("CanvasStageClear").GetComponent<ICanvasStageClear>();
        _canvasEditor = GameObject.Find("CanvasEditor").GetComponent<ICanvasEditor>();
        _canvasRunning = GameObject.Find("CanvasRunning").GetComponent<ICanvasRunning>();
        _canvasVerified = GameObject.Find("CanvasVerified").GetComponent<ICanvasVerified>();
        _canvasSettings = GameObject.Find("CanvasSettings").GetComponent<ICanvasSettings>();
        
        _mainFieldDrawer = GameObject.Find("MainFieldDrawer").GetComponent<IMainFieldDrawer>();
        _mainFieldInputs = GameObject.Find("MainFieldInputs").GetComponent<IMainFieldInputs>();
        _mainFieldController = GameObject.Find("MainFieldController").GetComponent<IMainFieldController>();
        _mainFieldModel = GameObject.Find("MainFieldModel").GetComponent<IMainFieldModel>();
    }

    private void Start()
    {
        _canvasTitle.OnClickStart.Subscribe(_ =>
        {
            _canvasTitle.Hide();
            _canvasStageSelect.Show();
        });
        
        _canvasTitle.OnClickEdit.Subscribe(_ =>
        {
            EditorMapModel.Initialize();
            _canvasTitle.Hide();
            _canvasEditor.Show();
            _mainFieldDrawer.ClearField();
        });

        _canvasTitle.OnClickSettings.Subscribe(_ =>
        {
            _canvasSettings.FadeIn();
        });
        
        _canvasStageSelect.OnClickReturn.Subscribe(_ =>
        {
            _canvasStageSelect.Hide();
            _canvasTitle.Show();
        });

        _canvasStageSelect.OnClickPlay.Subscribe(stage =>
        {
            _mainMap.floors = stage.floors.ToCharArray();
            _mainMap.levels = stage.levels.ToCharArray();
            _mainMap.objects = stage.objects.ToCharArray();

            _canvasStageSelect.Hide();
            _canvasMain.Show();
            
            _mainFieldInputs.SetActive(true);
            _mainFieldModel.InitializeMap(_mainMap);
            MainModeModel.SetMode(MainMode.Main);
        });

        _canvasMain.OnClickPause.Subscribe(_ =>
        {
            _canvasStageSelect.Show();
            _canvasMain.Hide();
            _mainFieldInputs.SetActive(false);
            MainModeModel.SetMode(MainMode.Idle);
        });

        _canvasMain.OnClickReset.Subscribe(_ =>
        {
            _mainFieldModel.InitializeMap(_mainMap);
        });

        _canvasStageClear.OnClickStageSelect.Subscribe(_ =>
        {
            _canvasStageSelect.Show();
            _canvasStageClear.Hide();
        });
        
        _canvasEditor.OnClickReturn.Subscribe(_ =>
        {
            _canvasEditor.Hide();
            _canvasTitle.Show();
        });
        
        _canvasEditor.OnClickRun.Subscribe(_ =>
        {
            _canvasEditor.Hide();
            _canvasRunning.Show();
            _mainFieldInputs.SetActive(true);
            _mainFieldModel.InitializeMap(_editorMap);
            MainModeModel.SetMode(MainMode.Running);
        });

        _canvasEditor.OnClickUpload.Subscribe(_ =>
        {
            _canvasEditor.Hide();
            _canvasRunning.Show();
            _mainFieldInputs.SetActive(true);
            _mainFieldModel.InitializeMap(_editorMap);
            MainModeModel.SetMode(MainMode.Verifying);
        });

        _canvasRunning.OnClickPause.Subscribe(_ =>
        {
            _canvasEditor.Show();
            _canvasRunning.Hide();
            _mainFieldInputs.SetActive(false);
            _mainFieldDrawer.DrawMap(_editorMap);
            MainModeModel.SetMode(MainMode.Idle);
        });
        
        _canvasVerified.OnClickReturn.Subscribe(_ =>
        {
            _canvasEditor.Show();
            _canvasVerified.Hide();
            _mainFieldInputs.SetActive(false);
            _mainFieldDrawer.DrawMap(_editorMap);
        });

        _canvasVerified.OnClickTitle.Subscribe(_ =>
        {
            _canvasTitle.Show();
            _canvasVerified.Hide();
            _mainFieldDrawer.ClearField();
        });

        _canvasSettings.OnClickFinish.Subscribe(_ =>
        {
            _canvasSettings.FadeOut();
        });
        _mainFieldController.OnStageClear.Subscribe(_ =>
        {
            _canvasMain.Hide();
            MainModeModel.SetMode(MainMode.Idle);
            Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(_ =>
            {
                _canvasStageClear.Show();
            });
        });
        _mainFieldController.OnVerified.Subscribe(_ =>
        {
            _canvasRunning.Hide();
            MainModeModel.SetMode(MainMode.Idle);
            Observable.Timer(TimeSpan.FromSeconds(0.8)).Subscribe(_ =>
            {
                _canvasVerified.Show();
            });
        });
        
        EditorMapModel.map.Subscribe(editorMap =>
        {
            _editorMap = editorMap;
            _canvasVerified.SetEditorMap(editorMap);
        });

        _canvasTitle.Show();
        _canvasStageSelect.Hide();
        _canvasMain.Hide();
        _canvasStageClear.Hide();
        _canvasEditor.Hide();
        _canvasRunning.Hide();
        _canvasVerified.Hide();
        _canvasSettings.Hide();
    }
}
