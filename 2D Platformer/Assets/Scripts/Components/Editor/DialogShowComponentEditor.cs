using UnityEditor;
using Utils.Editor;

namespace Components.Editor
{
    [CustomEditor(typeof(DialogShowComponent))]
    public class DialogShowComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);

            if (_modeProperty.GetEnum(out DialogShowComponent.Mode mode))
            {
                switch (mode)
                {
                    case DialogShowComponent.Mode.Local:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_localDialog"));
                        break;
                    case DialogShowComponent.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_externalDialog"));
                        break;
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}