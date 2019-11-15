using UnityEngine;

namespace Util.MenuSystem {
    /**
     * TODO: Move Lock cursor to a static script
     */
    public abstract class MenuBase : MonoBehaviour, IMenu {
        [SerializeField] private bool openOnInitialization;
        [SerializeField] private bool showMouseWhenOpen = true;

        public IMenuController Controller { get; protected set; }

        public bool IsOpen {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        [field: SerializeField] public float TimeScaleActivated { get; } = 0;

        protected virtual void Awake() {
            IsOpen = openOnInitialization;
        }

        private void OnEnable() {
            if (showMouseWhenOpen) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void OnDisable() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}