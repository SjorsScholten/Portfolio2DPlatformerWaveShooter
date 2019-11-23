using System.Linq;
using System.Reflection;
using Components.Movement.Behaviours;
using UnityEditor;

namespace Components.Movement.Editor {
    [CustomEditor(typeof(MovementComponent))]
    public class MovementComponentEditor : UnityEditor.Editor {
        private int _index = 0;
        private readonly string[] _options = new[] {"option 1", "option 2", "option 3"};
        
        public override void OnInspectorGUI() {
            _index = EditorGUILayout.Popup("Movement Behaviour", _index, GetBehaviours<MovementBehaviour>());
            base.OnInspectorGUI();
        }

        private string[] GetBehaviours<T>() {
            var types = Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType =>
                myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));
            var classes = types.Select(type => type.FullName).ToList();
            classes.Sort();
            return classes.ToArray();
        }
    }
}