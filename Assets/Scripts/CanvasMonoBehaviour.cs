using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMonoBehaviour : MonoBehaviour
{
    private CanvasScaler _canvasScaler;
    private void Awake()
    {
        _canvasScaler = GetComponent<CanvasScaler>();
        Update();
    }

    private void Update()
    {
        _canvasScaler.matchWidthOrHeight = Screen.width * 9 / Screen.height >= 16 ? 1.0f : 0.0f;
    }
}
