using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class FloorHeightPresenter : MonoBehaviour
{
    private IFloorHeightModel _floorHeightModel;
    private ICanvasSettings _canvasSettings;
    private IMainFieldDrawer _mainFieldDrawer;

    private void Awake()
    {
        _floorHeightModel = GameObject.Find("FloorHeightModel").GetComponent<IFloorHeightModel>();
        _canvasSettings = GameObject.Find("CanvasSettings").GetComponent<ICanvasSettings>();
        _mainFieldDrawer = GameObject.Find("MainFieldDrawer").GetComponent<IMainFieldDrawer>();
    }

    private void Start()
    {
        _floorHeightModel.FloorHeight.Subscribe(floorHeight =>
        {
            _canvasSettings.SetFloorHeight(floorHeight);
            _mainFieldDrawer.ChangeFloorHeight(floorHeight);
        });

        _canvasSettings.OnClickFloorHeight.Subscribe(floorHeight =>
        {
            _floorHeightModel.SetFloorHeight(floorHeight);
        });
    }
}
