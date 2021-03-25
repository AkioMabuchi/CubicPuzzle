using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MainFieldDrawerPresenter : MonoBehaviour
{
    private IMainFieldModel _mainFieldModel;
    private IMainFieldDrawer _mainFieldDrawer;

    private void Start()
    {
        _mainFieldModel = GameObject.Find("MainFieldModel").GetComponent<IMainFieldModel>();
        _mainFieldDrawer = GameObject.Find("MainFieldDrawer").GetComponent<IMainFieldDrawer>();

        _mainFieldModel.InitializeFloors.Subscribe(floors =>
        {
            _mainFieldDrawer.InitializeFloors(floors);
        });
        
        _mainFieldModel.InitializePlayers.Subscribe(players =>
        {
            _mainFieldDrawer.InitializePlayers(players);
        });

        _mainFieldModel.InitializeFlags.Subscribe(flags =>
        {
            _mainFieldDrawer.InitializeFlags(flags);
        });

        _mainFieldModel.InitializeBoxes.Subscribe(boxes =>
        {
            _mainFieldDrawer.InitializeBoxes(boxes);
        });
        

        _mainFieldModel.MovingPlayers.Subscribe(players =>
        {
            _mainFieldDrawer.MovePlayers(players);
        });
        
        _mainFieldModel.MovingBoxes.Subscribe(boxes =>
        {
            _mainFieldDrawer.MoveBoxes(boxes);
        });

        _mainFieldModel.FallingPlayers.Subscribe(players =>
        {
            _mainFieldDrawer.FallPlayers(players);
        });

        _mainFieldModel.FallingBoxes.Subscribe(boxes =>
        {
            _mainFieldDrawer.FallBoxes(boxes);
        });

        _mainFieldModel.ElevateFloors.Subscribe(floors =>
        {
            _mainFieldDrawer.ElevateFloors(floors);
        });

        _mainFieldModel.ElevatePlayers.Subscribe(players =>
        {
            _mainFieldDrawer.ElevatePlayers(players);
        });

        _mainFieldModel.ElevateFlags.Subscribe(flags =>
        {
            _mainFieldDrawer.ElevateFlags(flags);
        });

        _mainFieldModel.ElevateBoxes.Subscribe(boxes =>
        {
            _mainFieldDrawer.ElevateBoxes(boxes);
        });

        _mainFieldModel.DrawFloors.Subscribe(floors =>
        {
            _mainFieldDrawer.UpdateFloors(floors);
        });

        _mainFieldModel.DrawFlags.Subscribe(flags =>
        {
            _mainFieldDrawer.UpdateFlags(flags);
        });
        
        EditorMapModel.map.Subscribe(editorMap =>
        {
            _mainFieldDrawer.DrawMap(editorMap);
        });
    }
}
