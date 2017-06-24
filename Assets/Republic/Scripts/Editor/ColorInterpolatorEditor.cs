using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ColorInterpolator))]
    public class ColorInterpolatorEditor : Editor
    {
        SerializedProperty _speed;
        SerializedProperty _outputEvent;

        void OnEnable()
        {
            _speed = serializedObject.FindProperty("_speed");
            _outputEvent = serializedObject.FindProperty("_outputEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_speed);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_outputEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
