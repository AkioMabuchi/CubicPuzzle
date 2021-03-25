using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleFlag
{
    public void Initialize(MainFieldFlag flag, int s);
    public void Elevate(int s);
    public void Draw(bool s);
    public void Diminish();
    public void ChangeFloorHeight(int s);
}
public class PuzzleFlag : PuzzleObject, IPuzzleFlag
{
    [SerializeField] private GameObject gameObjectFlag;
    public void Initialize(MainFieldFlag flag, int s)
    {
        base.floorHeight = s;
        level = flag.level;

        float positionX = flag.position % 13 - 6;
        float positionY = flag.level * levelHeight[s];
        float positionZ = flag.position / 13 - 6;

        transform.localPosition = new Vector3(positionX, positionY, positionZ);

        gameObjectFlag.SetActive(flag.doesExist);
    }

    public void Draw(bool s)
    {
        gameObjectFlag.SetActive(s);
    }
}
