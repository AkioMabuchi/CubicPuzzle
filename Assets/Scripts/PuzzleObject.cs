using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class PuzzleObject : MonoBehaviour
{
    protected readonly float[] levelHeight = {0.4f, 0.7f, 1.0f};
    protected int floorHeight;
    protected int level;
    public void Move(int s)
    {
        float positionX = s % 13 - 6;
        float positionY = level * levelHeight[floorHeight];
        float positionZ = s / 13 - 6;
        transform.DOLocalMove(new Vector3(positionX, positionY, positionZ), 0.3f).SetEase(Ease.Linear);
    }

    public void Fall(int s)
    {
        level = s;
        float positionY = s * levelHeight[floorHeight];
        transform.DOLocalMoveY(positionY, 0.2f).SetEase(Ease.InQuad);
    }

    public void Elevate(int s)
    {
        level = s;
        float positionY = s * levelHeight[floorHeight];
        transform.DOLocalMoveY(positionY, 0.5f).SetEase(Ease.InOutSine);
    }

    public void Diminish()
    {
        Destroy(gameObject);
    }

    public virtual void ChangeFloorHeight(int s)
    {
        floorHeight = s;
        float positionY = level * levelHeight[floorHeight];
        transform.DOLocalMoveY(positionY, 0.0f);
    }
}
