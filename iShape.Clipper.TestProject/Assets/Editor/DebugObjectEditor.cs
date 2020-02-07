using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugObject))]
[CanEditMultipleObjects]
public class DebugObjectEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        var shape = target as DebugObject;
        if (shape == null) {
            return;
        }

        if (GUILayout.Button("DoMemoryLeak", GUILayout.Width(120), GUILayout.Height(24))) {
            shape.DoMemoryLeak();
        }
        
        if (GUILayout.Button("Test", GUILayout.Width(120), GUILayout.Height(24))) {
            shape.DoMemoryLeakTest();
        }

    }
}