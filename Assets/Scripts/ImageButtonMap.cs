using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public interface IImageButtonMap
{
    public void Initialize(IMapClickable mapClickable, int index);
}
public class ImageButtonMap : MonoBehaviour, IImageButtonMap
{
    [SerializeField] private Sprite spriteDummy;
    private IMapClickable _mapClickable;

    private Image _image;
    private bool _isPointerIn;
    private int _index;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerEnter()
    {
        _isPointerIn = true;
        _image.sprite = null;
    }

    public void OnPointerExit()
    {
        _isPointerIn = false;
        _image.sprite = spriteDummy;
    }

    public void OnPointerDown()
    {
        if (_isPointerIn)
        {
            _mapClickable.OnClickButtonMap(_index);
        }
    }

    public void Initialize(IMapClickable mapClickable, int index)
    {
        _mapClickable = mapClickable;
        _index = index;
    }
}
