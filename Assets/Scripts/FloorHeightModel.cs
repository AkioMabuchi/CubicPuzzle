using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IFloorHeightModel
{
    public IObservable<int> FloorHeight
    {
        get;
    }

    public void SetFloorHeight(int s);
}
public class FloorHeightModel : MonoBehaviour, IFloorHeightModel
{
    private readonly ReactiveProperty<int> _floorHeight = new ReactiveProperty<int>();

    public IObservable<int> FloorHeight => _floorHeight;

    public void SetFloorHeight(int s)
    {
        _floorHeight.Value = s;
    }
}
