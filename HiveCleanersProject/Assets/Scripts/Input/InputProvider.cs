using System;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Input {
    public class InputProvider : MonoBehaviour {
        private enum ProviderType { None, Player, Enemy }

        [SerializeField] private ProviderType providerType = ProviderType.None;

        private IInputProvider _inputProvider;
        
        public event Action OnJumpAction;
        public event Action OnAttackAction;

        private void Awake() {
            CheckProvider();
        }

        private void CheckProvider() {
            switch (providerType) {
                
                case ProviderType.None:
                    _inputProvider = new NullInputProvider();
                    break;
                
                
                case ProviderType.Player:
                    
                    _inputProvider = PlayerInputProvider.Instance;
                    
                    break;
                
                
                case ProviderType.Enemy:
                    
                    EnemyInputProvider provider;
                    if ((provider = GetComponent<EnemyInputProvider>()) == null) {
                        this.gameObject.AddComponent(typeof(EnemyInputProvider));
                        provider = GetComponent<EnemyInputProvider>();
                    }
                    _inputProvider = provider;
                    break;
                
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable() {
            _inputProvider.OnJumpAction += OnJumpActionInvoker;
            _inputProvider.OnAttackAction += OnAttackActionInvoker;
        }

        private void OnDisable() {
            _inputProvider.OnJumpAction -= OnJumpActionInvoker;
            _inputProvider.OnAttackAction -= OnAttackActionInvoker;
        }
        
        private void OnJumpActionInvoker() => OnJumpAction?.Invoke();
        private void OnAttackActionInvoker() => OnAttackAction?.Invoke();

        public float HorizontalAxisInput => _inputProvider.HorizontalAxisInput;
        public Vector2 MousePosition => _inputProvider.LookDirection;
    }
}