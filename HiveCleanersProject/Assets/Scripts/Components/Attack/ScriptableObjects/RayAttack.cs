using UnityEngine;

namespace Components.Attack.ScriptableObjects {
    
    [CreateAssetMenu]
    public class RayAttack : Attack {
        protected override void ProcessAttack(Vector2 origin, Vector2 direction, int layerMask) {
            var hit = Physics2D.Raycast(origin, direction, range, layerMask);
            
            var color = hit ? Color.red : Color.green;
            Debug.DrawRay(origin, direction * range, color, 3);
            
            if (!hit) return;
            var target = hit.transform.GetComponent<TargetComponent>();
            if (target) target.ProcessTakeHit();
        }
    }
}