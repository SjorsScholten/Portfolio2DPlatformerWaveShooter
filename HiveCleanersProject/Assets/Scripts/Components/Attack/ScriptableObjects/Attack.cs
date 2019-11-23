using System.Linq;
using UnityEngine;

namespace Components.Attack.ScriptableObjects {
    public abstract class Attack : ScriptableObject {
        [SerializeField] protected float damage = 10f;
        [SerializeField] protected float range = 0;
        //[SerializeField] private float attacksPerSecond = 0;
        //[SerializeField] private int attacksPerSeries = 0;
        //[SerializeField] private float seriesCooldown = 0;
        [SerializeField] private int[] ignoreLayers = new int[0];

        private float _timeSinceLastAttack;
        
        public void HandleAttack(Vector2 origin, Vector2 direction, int ignoreLayer = 0) {
            //var time = Time.time;
            //if (time < _timeSinceLastAttack + attacksPerSecond) return;

            var mask = 0;
            if (ignoreLayer != 0) mask = ~(1 << ignoreLayer);
            mask = ignoreLayers.Aggregate(mask, (current, t) => current | ~(1 << t));

            ProcessAttack(origin, direction, mask);
            
            //_timeSinceLastAttack = time;
        }

        protected abstract void ProcessAttack(Vector2 origin, Vector2 direction, int layerMask);
    }
}
