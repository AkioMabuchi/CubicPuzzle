using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzlePlayer
{
    public void Initialize(MainFieldPlayer player, int s);
    public void Move(int s);
    public void Fall(int s);
    public void Elevate(int s);
    public void Diminish();
    public void ChangeFloorHeight(int s);
}
public class PuzzlePlayer : PuzzleObject, IPuzzlePlayer
{
    public void Initialize(MainFieldPlayer player, int s)
    { 
        floorHeight = s;
        level = player.level;

        float positionX = player.position % 13 - 6;
        float positionY = player.level * levelHeight[s];
        float positionZ = player.position / 13 - 6;

        transform.localPosition = new Vector3(positionX, positionY, positionZ);
    }
}
