using UniRx;

public enum MainMode
{
    Idle,
    Main,
    Running,
    Verifying
}

public static class MainModeModel
{
    public static ReactiveProperty<MainMode> Mode = new ReactiveProperty<MainMode>(MainMode.Idle);

    public static void SetMode(MainMode mode)
    {
        Mode.Value = mode;
    }
}
