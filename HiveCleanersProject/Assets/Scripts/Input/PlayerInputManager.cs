using Util;

namespace Input {
    
    public class PlayerInputManager : Singleton<PlayerInputManager> {
        public readonly PlayerInputProvider InputProvider = new PlayerInputProvider();
        private PlayerControls _controls;

        private void Awake() {
            _controls = new PlayerControls();
            _controls.Player.SetCallbacks(InputProvider);
        }

        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();
    }
}