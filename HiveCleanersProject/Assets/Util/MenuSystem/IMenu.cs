namespace Util.MenuSystem {
    public interface IMenu {
        IMenuController Controller { get; }
        bool IsOpen { get; set; }
        float TimeScaleActivated { get; }
    }
}