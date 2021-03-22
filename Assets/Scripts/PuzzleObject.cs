using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface IPuzzleObject
{
    public void SetPosition(int index);
    public void MovePosition(int index);
    public void SetLevel(int level);
    public void Elevate(int level);
    public void Diminish();
}
public abstract class PuzzleObject : MonoBehaviour, IPuzzleObject
{
    public void SetPosition(int index)
    {
        float positionX = index % 13 - 6;
        float positionZ = index / 13 - 6;
        Vector3 position = transform.position;
        position.x = positionX;
        position.z = positionZ;
        transform.position = position;
    }

    public void MovePosition(int index)
    {
        float positionX = index % 13 - 6;
        float positionY = transform.position.y;
        float positionZ = index / 13 - 6;
        transform.DOLocalMove(new Vector3(positionX, positionY, positionZ), 0.3f);
    }
    public void SetLevel(int level)
    {
        Vector3 position = transform.position;
        position.y = level + 0.2f;
        transform.position = position;
    }

    public void Elevate(int level)
    {
        float positionY = level + 0.2f;
        transform.DOLocalMoveY(positionY, 0.5f);
    }

    public void Diminish()
    {
        Destroy(gameObject);
    }
}
