using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CanvasEditorPresenter
{
    public CanvasEditorPresenter()
    {
        ICanvasEditor canvasEditor = GameObject.Find("CanvasEditor").GetComponent<ICanvasEditor>();

        canvasEditor.OnClickMap.Subscribe(EditorMapModel.UpdateMap);

        canvasEditor.OnClickTool.Subscribe(EditorMapModel.ChangeTool);
        
        EditorMapModel.map.Subscribe(map =>
        {
            canvasEditor.DrawMap(map);
            canvasEditor.VerifyMap(map);
        });

        EditorMapModel.selectedTool.Subscribe(index =>
        {
            canvasEditor.DrawTools(index);
        });
    }
}
