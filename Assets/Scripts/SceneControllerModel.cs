using System;
using UniRx;

public enum SceneMode
{
    Title,
    Editor,
    Main
}

public static class SceneControllerModel
{
    private static readonly ReactiveProperty<SceneMode> _mode = new ReactiveProperty<SceneMode>(SceneMode.Editor);
    public static IObservable<SceneMode> Mode => _mode;

    public static void ChangeSceneMode(SceneMode mode)
    {
        _mode.Value = mode;
    }
}
