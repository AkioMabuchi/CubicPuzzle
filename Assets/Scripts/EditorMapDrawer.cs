using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EditorMapDrawerPresenter
{
    public EditorMapDrawerPresenter()
    {
        IEditorMapDrawer editorMapDrawer = GameObject.Find("EditorMapDrawer").GetComponent<IEditorMapDrawer>();

        EditorMapModel.map.Subscribe(map =>
        {
            editorMapDrawer.DrawMap(map);
        });
    }
}

public interface IEditorMapDrawer
{
    public void DrawMap(EditorMap map);

    public void ClearMap();
}
public class EditorMapDrawer : MonoBehaviour, IEditorMapDrawer
{
    [SerializeField] private GameObject prefabFloorWhite;
    [SerializeField] private GameObject prefabFloorYellow;
    [SerializeField] private GameObject prefabFloorBlue;
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabFlag;
    [SerializeField] private GameObject prefabBox1;
    [SerializeField] private GameObject prefabBox2;
    [SerializeField] private GameObject prefabBox3;
    
    private readonly IPuzzleFloor[] _puzzleFloors = new IPuzzleFloor[169];
    private readonly IPuzzleObject[] _puzzleObjects = new IPuzzleObject[169];

    public void DrawMap(EditorMap map)
    {
        for (int i = 0; i < 169; i++)
        {
            IPuzzleFloor puzzleFloor = null;
            IPuzzleObject puzzleObject = null;
            
            _puzzleFloors[i]?.Diminish();
            _puzzleObjects[i]?.Diminish();
            
            _puzzleFloors[i] = null;
            _puzzleObjects[i] = null;
            
            switch (map.floors[i])
            {
                case 'W':
                    puzzleFloor = Instantiate(prefabFloorWhite).GetComponent<IPuzzleFloor>();
                    break;
                case 'Y':
                    puzzleFloor = Instantiate(prefabFloorYellow).GetComponent<IPuzzleFloor>();
                    break;
                case 'B':
                    puzzleFloor = Instantiate(prefabFloorBlue).GetComponent<IPuzzleFloor>();
                    break;
            }

            switch (map.objects[i])
            {
                case 'P':
                    puzzleObject = Instantiate(prefabPlayer).GetComponent<IPuzzleObject>();
                    break;
                case 'F':
                    puzzleObject = Instantiate(prefabFlag).GetComponent<IPuzzleObject>();
                    break;
                case '1':
                    puzzleObject = Instantiate(prefabBox1).GetComponent<IPuzzleObject>();
                    break;
                case '2':
                    puzzleObject = Instantiate(prefabBox2).GetComponent<IPuzzleObject>();
                    break;
                case '3':
                    puzzleObject = Instantiate(prefabBox3).GetComponent<IPuzzleObject>();
                    break;
            }
            
            puzzleFloor?.SetPosition(i);
            puzzleObject?.SetPosition(i);
            
            switch (map.levels[i])
            {
                case '0':
                    puzzleFloor?.SetLevel(0);
                    puzzleObject?.SetLevel(0);
                    break;
                case '1':
                    puzzleFloor?.SetLevel(1);
                    puzzleObject?.SetLevel(1);
                    break;
                case '2':
                    puzzleFloor?.SetLevel(2);
                    puzzleObject?.SetLevel(2);
                    break;
                case '3':
                    puzzleFloor?.SetLevel(3);
                    puzzleObject?.SetLevel(3);
                    break;
            }

            switch (map.objects[i])
            {
                case 'Y':
                    puzzleFloor?.SetModel(PuzzleFloorMode.YellowUp);
                    break;
                case 'y':
                    puzzleFloor?.SetModel(PuzzleFloorMode.YellowDown);
                    break;
                case 'B':
                    puzzleFloor?.SetModel(PuzzleFloorMode.BlueUp);
                    break;
                case 'b':
                    puzzleFloor?.SetModel(PuzzleFloorMode.BlueDown);
                    break;
                default:
                    puzzleFloor?.SetModel(PuzzleFloorMode.Normal);
                    break;
            }
            
            _puzzleFloors[i] = puzzleFloor;
            _puzzleObjects[i] = puzzleObject;
        }
    }

    public void ClearMap()
    {
        for (int i = 0; i < 169; i++)
        {
            _puzzleFloors[i]?.Diminish();
            _puzzleObjects[i]?.Diminish();
            _puzzleFloors[i] = null;
            _puzzleObjects[i] = null;
        }
    }
}
