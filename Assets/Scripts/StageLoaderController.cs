using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class StageLoaderController : MonoBehaviour
{
    [SerializeField] private GameObject prefabStageLoader;

    private ICanvasTitle _canvasTitle;
    private ICanvasStageSelect _canvasStageSelect;
    private ICanvasMain _canvasMain;
    private ICanvasStageClear _canvasStageClear;
    private ICanvasVerified _canvasVerified;
    private IEditorMapDrawer _editorMapDrawer;
    
    private void OnEnable()
    {
        _canvasTitle = GameObject.Find("CanvasTitle").GetComponent<ICanvasTitle>();
        _canvasStageSelect = GameObject.Find("CanvasStageSelect").GetComponent<ICanvasStageSelect>();
        _canvasMain = GameObject.Find("CanvasMain").GetComponent<ICanvasMain>();
        _canvasStageClear = GameObject.Find("CanvasStageClear").GetComponent<ICanvasStageClear>();
        _canvasVerified = GameObject.Find("CanvasVerified").GetComponent<ICanvasVerified>();
        _editorMapDrawer = GameObject.Find("EditorMapDrawer").GetComponent<IEditorMapDrawer>();
    }

    private void Start()
    {
        IStageLoader stageLoaderForStageSelect = Instantiate(prefabStageLoader).GetComponent<IStageLoader>();
        IStageLoader stageLoaderForUpload = Instantiate(prefabStageLoader).GetComponent<IStageLoader>();
        IStageLoader stageLoaderForTitle = Instantiate(prefabStageLoader).GetComponent<IStageLoader>();

        _canvasTitle.OnClickStart.Subscribe(_ =>
        {
            stageLoaderForStageSelect.ReceiveStages();
        });

        _canvasMain.OnClickPause.Subscribe(_ =>
        {
            stageLoaderForStageSelect.ReceiveStages();
        });

        _canvasStageClear.OnClickStageSelect.Subscribe(_ =>
        {
            stageLoaderForStageSelect.ReceiveStages();
        });
        
        _canvasVerified.OnClickUpload.Subscribe(stage =>
        {
            stageLoaderForUpload.SendStage(stage);
        });

        stageLoaderForStageSelect.ReceiveStageResult.Subscribe(result =>
        {
            if (result == null)
            {

            }
            else
            {
                _canvasStageSelect.SetStages(result);
            }
        });
        stageLoaderForUpload.SendStageResult.Subscribe(result =>
        {
            if (result)
            {
                _canvasVerified.ShowSuccessMessage();
            }
            else
            {
                _canvasVerified.ShowErrorMessage();
            }
        });

        stageLoaderForTitle.ReceiveStageResult.Subscribe(result =>
        {
            if (result == null)
            {
                Debug.Log("ローディングに失敗しました");
            }
            else
            {
                if (result.Length > 0)
                {
                    Stage stage = result[UnityEngine.Random.Range(0, result.Length)];
                    EditorMap editorMap = new EditorMap();
                    editorMap.floors = stage.floors.ToCharArray();
                    editorMap.levels = stage.levels.ToCharArray();
                    editorMap.objects = stage.objects.ToCharArray();
                    _editorMapDrawer.DrawMap(editorMap);
                }
            }
        });
        
        stageLoaderForTitle.ReceiveStages();
    }
}
