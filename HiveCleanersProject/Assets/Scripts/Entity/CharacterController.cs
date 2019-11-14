using UnityEngine;

public class CharacterController {
    private readonly Character _character;

    public CharacterController(Character character) {
        _character = character;
    }

    public void ProcessMovement(Vector2 targetPosition, float velocity) {
        _character.Position = Vector2.MoveTowards(_character.Position, targetPosition, velocity * Time.deltaTime);
    }

    public void ProcessJump() {
        
    }
}