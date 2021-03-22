using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private float rotationSpeedX;
    [SerializeField] private float rotationSpeedY;

    private GameObject _gameObjectXAxis;
    
    private float _rotationX = 20.0f;
    private float _rotationY;
    private void OnEnable()
    {
        _gameObjectXAxis = gameObject.transform.Find("GameCameraXAxis").gameObject;
    }

    private void Start()
    {
        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            .Subscribe(_ =>
            {
                _rotationY -= rotationSpeedY;
            });
        
        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            .Subscribe(_ =>
            {
                _rotationY += rotationSpeedY;
            });
        
        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            .Subscribe(_ =>
            {
                _rotationX += rotationSpeedX;
                if (_rotationX > 90.0f)
                {
                    _rotationX = 90.0f;
                }
            });
        
        this.FixedUpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
            .Subscribe(_ =>
            {
                _rotationX -= rotationSpeedX;
                if (_rotationX < 20.0f)
                {
                    _rotationX = 20.0f;
                }
            });

        this.LateUpdateAsObservable()
            .Subscribe(_ =>
            {
                gameObject.transform.localRotation = Quaternion.Euler(0.0f, _rotationY, 0.0f);
                _gameObjectXAxis.transform.localRotation = Quaternion.Euler(_rotationX, 0.0f, 0.0f);
            });
    }
}
