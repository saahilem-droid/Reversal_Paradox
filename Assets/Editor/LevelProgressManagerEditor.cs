using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelProgressManager))]
public class LevelProgressManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        GUI.backgroundColor = Color.red;

        if (GUILayout.Button("Reset Saved Progress", GUILayout.Height(35)))
        {
            if (EditorUtility.DisplayDialog(
                "Reset Progress",
                "Delete all saved level progress?",
                "Yes",
                "Cancel"))
            {
                ((LevelProgressManager)target).ResetProgress();
            }
        }

        GUI.backgroundColor = Color.white;
    }
}