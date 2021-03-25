using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GameCameraPresenter
{
    public GameCameraPresenter()
    {
        IGameCamera gameCamera = GameObject.Find("GameCamera").GetComponent<IGameCamera>();
        IGameCameraInputs gameCameraInputs = GameObject.Find("GameCameraInputs").GetComponent<IGameCameraInputs>();

        gameCameraInputs.RotateUp.Subscribe(_ =>
        {
            gameCamera.RotateUp();
        });

        gameCameraInputs.RotateDown.Subscribe(_ =>
        {
            gameCamera.RotateDown();
        });

        gameCameraInputs.RotateRight.Subscribe(_ =>
        {
            gameCamera.RotateRight();
        });

        gameCameraInputs.RotateLeft.Subscribe(_ =>
        {
            gameCamera.RotateLeft();
        });

        gameCameraInputs.ZoomUp.Subscribe(_ =>
        {
            gameCamera.ZoomUp();
        });

        gameCameraInputs.ZoomDown.Subscribe(_ =>
        {
            gameCamera.ZoomDown();
        });
    }
}

public interface IGameCameraInputs
{
    public IObservable<Unit> RotateUp
    {
        get;
    }
    public IObservable<Unit> RotateDown
    {
        get;
    }
    public IObservable<Unit> RotateRight
    {
        get;
    }
    public IObservable<Unit> RotateLeft
    {
        get;
    }
    public IObservable<Unit> ZoomUp
    {
        get;
    }
    public IObservable<Unit> ZoomDown
    {
        get;
    }
}
public class GameCameraInputs : MonoBehaviour, IGameCameraInputs
{
    private readonly Subject<Unit> _rotateUp = new Subject<Unit>();
    private readonly Subject<Unit> _rotateDown = new Subject<Unit>();
    private readonly Subject<Unit> _rotateRight = new Subject<Unit>();
    private readonly Subject<Unit> _rotateLeft = new Subject<Unit>();
    private readonly Subject<Unit> _zoomUp = new Subject<Unit>();
    private readonly Subject<Unit> _zoomDown = new Subject<Unit>();

    public IObservable<Unit> RotateUp => _rotateUp;
    public IObservable<Unit> RotateDown => _rotateDown;
    public IObservable<Unit> RotateRight => _rotateRight;
    public IObservable<Unit> RotateLeft => _rotateLeft;
    public IObservable<Unit> ZoomUp => _zoomUp;
    public IObservable<Unit> ZoomDown => _zoomDown;

    private void Start()
    {
        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            .Subscribe(_ =>
            {
                _rotateUp.OnNext(Unit.Default);
            });

        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            .Subscribe(_ =>
            {
                _rotateDown.OnNext(Unit.Default);
            });

        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            .Subscribe(_ =>
            {
                _rotateRight.OnNext(Unit.Default);
            });
        
        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            .Subscribe(_ =>
            {
                _rotateLeft.OnNext(Unit.Default);
            });

        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q))
            .Subscribe(_ =>
            {
                _zoomUp.OnNext(Unit.Default);
            });

        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
            .Subscribe(_ =>
            {
                _zoomDown.OnNext(Unit.Default);
            });
    }
}
