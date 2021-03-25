using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface IPuzzleBox
{
    public void Initialize(MainFieldBox box, int s);
    public void Move(int s);
    public void Fall(int s);
    public void Elevate(int s);
    public void Diminish();
    public void ChangeFloorHeight(int s);
}

public class PuzzleBox : PuzzleObject, IPuzzleBox
{
    [SerializeField] private GameObject[] gameObjectsBox = new GameObject[9];

    private int _boxSize;
    public void Initialize(MainFieldBox box, int s)
    {
        floorHeight = s;
        level = box.level;
        
        float positionX = box.position % 13 - 6;
        float positionY = box.level * levelHeight[s];
        float positionZ = box.position / 13 - 6;

        transform.localPosition = new Vector3(positionX, positionY, positionZ);

        _boxSize = box.size - 1;
        int boxIndex = _boxSize * 3 + floorHeight;
        gameObjectsBox[boxIndex].SetActive(true);
    }

    public override void ChangeFloorHeight(int s)
    {
        base.ChangeFloorHeight(s);

        int boxIndex = _boxSize * 3 + floorHeight;
        for (int i = 0; i < 9; i++)
        {
            gameObjectsBox[i].SetActive(i == boxIndex);
        }
    }
}
