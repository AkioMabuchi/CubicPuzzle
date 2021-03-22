using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void RuntimeInitializeApplication()
    {
        CanvasEditorPresenter canvasEditorPresenter
            = new CanvasEditorPresenter();
        
        EditorMapDrawerPresenter editorMapDrawerPresenter
            = new EditorMapDrawerPresenter();
        
        MainFieldDrawerPresenter mainFieldDrawerPresenter
            = new MainFieldDrawerPresenter();
    }
}
