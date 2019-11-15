using UnityEngine.InputSystem;

namespace Input {
    public class PlayerInputProvider : IInputProvider, PlayerControls.IPlayerActions {
        public float MoveAxis { get; private set; }
        public float JumpButton { get; private set; }
        
        public void OnMove(InputAction.CallbackContext context) => MoveAxis = context.ReadValue<float>();

        public void OnJump(InputAction.CallbackContext context) {
            
        }
    }
}