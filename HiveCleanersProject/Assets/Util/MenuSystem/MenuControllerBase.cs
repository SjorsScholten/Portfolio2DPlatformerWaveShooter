using UnityEngine;

namespace Util.MenuSystem {
    public abstract class MenuControllerBase : IMenuController {
        private readonly IMenu _menu;

        public MenuControllerBase(IMenu menu) {
            _menu = menu;
        }

        public virtual void Show() {
            if (!_menu.IsOpen) _menu.IsOpen = true;
            Time.timeScale = _menu.TimeScaleActivated;
        }

        public virtual void Hide() {
            if (_menu.IsOpen) _menu.IsOpen = false;
            Time.timeScale = 1;
        }
    }
}