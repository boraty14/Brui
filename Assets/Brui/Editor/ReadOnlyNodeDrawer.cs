using Brui.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Brui.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyNodeAttribute))]
    public class ReadOnlyNodeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool previousGUIState = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = previousGUIState;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
#endif
}