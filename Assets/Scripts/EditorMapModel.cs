using UniRx;
using UnityEngine;
using UnityEngine.SearchService;

public struct EditorMap
{
    public char[] floors;
    public char[] levels;
    public char[] objects;
}

public static class EditorMapModel
{
    public static ReactiveProperty<int> selectedTool = new ReactiveProperty<int>(0);
    public static readonly ReactiveProperty<EditorMap> map = new ReactiveProperty<EditorMap>();

    static EditorMapModel()
    {
        EditorMap editorMap = new EditorMap();
        char[] floors = new char[169];
        char[] levels = new char[169];
        char[] objects = new char[169];
        for (int i = 0; i < 169; i++)
        {
            floors[i] = '_';
            levels[i] = '0';
            objects[i] = '.';
        }

        editorMap.floors = floors;
        editorMap.levels = levels;
        editorMap.objects = objects;

        map.Value = editorMap;
    }

    public static void UpdateMap(int index)
    {
        EditorMap editorMap = map.Value;
        switch (selectedTool.Value)
        {
            case 0:
                editorMap.objects[index] = '.';
                break;
            case 1:
                editorMap.floors[index] = '_';
                break;
            case 2:
                editorMap.floors[index] = 'W';
                break;
            case 3:
                editorMap.floors[index] = 'Y';
                break;
            case 4:
                editorMap.floors[index] = 'B';
                break;
            case 5:
                editorMap.levels[index] = '0';
                break;
            case 6:
                editorMap.levels[index] = '1';
                break;
            case 7:
                editorMap.levels[index] = '2';
                break;
            case 8:
                editorMap.levels[index] = '3';
                break;
            case 9:
                editorMap.objects[index] = 'P';
                break;
            case 10:
                editorMap.objects[index] = 'F';
                break;
            case 11:
                editorMap.objects[index] = '1';
                break;
            case 12:
                editorMap.objects[index] = '2';
                break;
            case 13:
                editorMap.objects[index] = '3';
                break;
            case 14:
                editorMap.objects[index] = 'Y';
                break;
            case 15:
                editorMap.objects[index] = 'y';
                break;
            case 16:
                editorMap.objects[index] = 'B';
                break;
            case 17:
                editorMap.objects[index] = 'b';
                break;
        }

        if (editorMap.floors[index] == '_')
        {
            editorMap.levels[index] = '0';
            editorMap.objects[index] = '.';
        }
        map.SetValueAndForceNotify(editorMap);
    }

    public static void ChangeTool(int index)
    {
        selectedTool.Value = index;
    }

    public static void Initialize()
    {
        EditorMap editorMap = new EditorMap();
        char[] floors = new char[169];
        char[] levels = new char[169];
        char[] objects = new char[169];
        for (int i = 0; i < 169; i++)
        {
            floors[i] = '_';
            levels[i] = '0';
            objects[i] = '.';
        }

        editorMap.floors = floors;
        editorMap.levels = levels;
        editorMap.objects = objects;

        map.SetValueAndForceNotify(editorMap);
    }
}