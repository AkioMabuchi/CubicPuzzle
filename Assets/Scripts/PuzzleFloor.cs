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
    public void Initialize(MainFieldFloor floor, int s);
    public void Elevate(int s);
    public void Draw(MainFieldFloor floor);
    public void Diminish();
    public void ChangeFloorHeight(int s);
}

public class PuzzleFloor : PuzzleObject, IPuzzleFloor
{
    [SerializeField] private GameObject[] gameObjectsNormal = new GameObject[3];
    [SerializeField] private GameObject[] gameObjectsYellowUp = new GameObject[3];
    [SerializeField] private GameObject[] gameObjectsYellowDown = new GameObject[3];
    [SerializeField] private GameObject[] gameObjectsBlueUp = new GameObject[3];
    [SerializeField] private GameObject[] gameObjectsBlueDown = new GameObject[3];

    public void Initialize(MainFieldFloor floor, int s)
    {
        floorHeight = s;
        level = floor.level;

        float positionX = floor.position % 13 - 6;
        float positionY = floor.level * levelHeight[s];
        float positionZ = floor.position / 13 - 6;

        transform.localPosition = new Vector3(positionX, positionY, positionZ);

        int objectIndex = 0;
        switch (floor.floorColor)
        {
            case FloorColor.Yellow:
                objectIndex = 1;
                break;
            case FloorColor.Blue:
                objectIndex = 2;
                break;
        }

        switch (floor.switchPanel)
        {
            case SwitchPanel.None:
                gameObjectsNormal[objectIndex].SetActive(true);
                break;
            case SwitchPanel.YellowUp:
                gameObjectsYellowUp[objectIndex].SetActive(true);
                break;
            case SwitchPanel.YellowDown:
                gameObjectsYellowDown[objectIndex].SetActive(true);
                break;
            case SwitchPanel.BlueUp:
                gameObjectsBlueUp[objectIndex].SetActive(true);
                break;
            case SwitchPanel.BlueDown:
                gameObjectsBlueDown[objectIndex].SetActive(true);
                break;
        }
    }

    public void Draw(MainFieldFloor floor)
    {
        int objectIndex = 0;
        switch (floor.floorColor)
        {
            case FloorColor.Yellow:
                objectIndex = 1;
                break;
            case FloorColor.Blue:
                objectIndex = 2;
                break;
        }

        switch (floor.switchPanel)
        {
            case SwitchPanel.YellowUp:
                gameObjectsYellowUp[objectIndex].SetActive(true);
                gameObjectsYellowDown[objectIndex].SetActive(false);
                break;
            case SwitchPanel.YellowDown:
                gameObjectsYellowDown[objectIndex].SetActive(true);
                gameObjectsYellowUp[objectIndex].SetActive(false);
                break;
            case SwitchPanel.BlueUp:
                gameObjectsBlueUp[objectIndex].SetActive(true);
                gameObjectsBlueDown[objectIndex].SetActive(false);
                break;
            case SwitchPanel.BlueDown:
                gameObjectsBlueDown[objectIndex].SetActive(true);
                gameObjectsBlueUp[objectIndex].SetActive(false);
                break;
        }
    }
}
