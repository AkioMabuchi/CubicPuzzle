using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public interface IGameCamera
{
    public void RotateUp();
    public void RotateDown();
    public void RotateRight();
    public void RotateLeft();
    public void ZoomUp();
    public void ZoomDown();
}
public class GameCamera : MonoBehaviour, IGameCamera
{
    [SerializeField] private float rotationSpeedX;
    [SerializeField] private float rotationSpeedY;
    [SerializeField] private float movingSpeedZ;
    [SerializeField] private float maximumZ;
    [SerializeField] private float minimumZ;
    
    private GameObject _gameObjectXAxis;
    private GameObject _gameObjectZAxis;
    
    private float _rotationX = 20.0f;
    private float _rotationY;
    private float _positionZ;
    private void OnEnable()
    {
        _gameObjectXAxis = gameObject.transform.Find("GameCameraXAxis").gameObject;
        _gameObjectZAxis = _gameObjectXAxis.transform.Find("Camera").gameObject;
    }

    private void Start()
    {
        this.LateUpdateAsObservable()
            .Subscribe(_ =>
            {
                gameObject.transform.localRotation = Quaternion.Euler(0.0f, _rotationY, 0.0f);
                _gameObjectXAxis.transform.localRotation = Quaternion.Euler(_rotationX, 0.0f, 0.0f);
                _gameObjectZAxis.transform.localPosition = new Vector3(0.0f, 0.0f, -_positionZ);
            });

        _positionZ = minimumZ;
    }

    public void RotateUp()
    {
        _rotationX += rotationSpeedX;
        if (_rotationX > 90.0f)
        {
            _rotationX = 90.0f;
        }
    }

    public void RotateDown()
    {
        _rotationX -= rotationSpeedX;
        if (_rotationX < 20.0f)
        {
            _rotationX = 20.0f;
        }
    }

    public void RotateRight()
    {
        _rotationY -= rotationSpeedY;
    }

    public void RotateLeft()
    {
        _rotationY += rotationSpeedY;
    }

    public void ZoomUp()
    {
        _positionZ -= movingSpeedZ;
        if (_positionZ < minimumZ)
        {
            _positionZ = minimumZ;
        }
    }

    public void ZoomDown()
    {
        _positionZ += movingSpeedZ;
        if (_positionZ > maximumZ)
        {
            _positionZ = maximumZ;
        }
    }
}
