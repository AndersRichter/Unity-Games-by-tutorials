using UnityEditor;
using UnityEditor.UI;

namespace UI.Widgets.Editor
{
    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normalText"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressedText"));
            serializedObject.ApplyModifiedProperties();
            
            base.OnInspectorGUI();
        }
    }
}