using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum PuzzleFloorMode
{
    Normal,
    YellowUp,
    YellowDown,
    BlueUp,
    BlueDown
}

public interface IPuzzleFloor
{
    public void SetModel(PuzzleFloorMode mode);
    public void SetPosition(int index);
    public void SetLevel(int level);
    public void Elevate(int level);
    public void Diminish();
}

public abstract class PuzzleFloor : MonoBehaviour, IPuzzleFloor
{
    [SerializeField] private GameObject gameObjectNormal;
    [SerializeField] private GameObject gameObjectYellowUp;
    [SerializeField] private GameObject gameObjectYellowDown;
    [SerializeField] private GameObject gameObjectBlueUp;
    [SerializeField] private GameObject gameObjectBlueDown;

    public void SetModel(PuzzleFloorMode mode)
    {
        gameObjectNormal.SetActive(false);
        gameObjectYellowUp.SetActive(false);
        gameObjectYellowDown.SetActive(false);
        gameObjectBlueUp.SetActive(false);
        gameObjectBlueDown.SetActive(false);
        
        switch (mode)
        {
            case PuzzleFloorMode.Normal:
                gameObjectNormal.SetActive(true);
                break;
            case PuzzleFloorMode.YellowUp:
                gameObjectYellowUp.SetActive(true);
                break;
            case PuzzleFloorMode.YellowDown:
                gameObjectYellowDown.SetActive(true);
                break;
            case PuzzleFloorMode.BlueUp:
                gameObjectBlueUp.SetActive(true);
                break;
            case PuzzleFloorMode.BlueDown:
                gameObjectBlueDown.SetActive(true);
                break;
        }
    }

    public void SetPosition(int index)
    {
        float positionX = index % 13 - 6;
        float positionZ = index / 13 - 6;
        Vector3 position = transform.position;
        position.x = positionX;
        position.z = positionZ;
        transform.position = position;
    }
    public void SetLevel(int level)
    {
        Vector3 position = transform.position;
        position.y = level;
        transform.position = position;
    }

    public void Elevate(int level)
    {
        float positionY = level;
        transform.DOLocalMoveY(positionY, 0.5f);
    }
    
    public void Diminish()
    {
        Destroy(gameObject);
    }
}
