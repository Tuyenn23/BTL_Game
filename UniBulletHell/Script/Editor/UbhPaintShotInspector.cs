using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(UbhPaintShot), true)]
public class UbhPaintShotInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawProperties();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProperties()
    {
        var obj = target as UbhPaintShot;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Shot"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy)
            {
                obj.Shot();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (obj.m_bulletPrefab == null || obj.m_paintDataText == null)
        {
            Color guiColor = GUI.color;
            GUI.color = Color.yellow;

            EditorGUILayout.LabelField("*****WARNING*****");

            if (obj.m_bulletPrefab == null)
            {
                EditorGUILayout.LabelField("BulletPrefab has not been set!");
            }

            if (obj.m_paintDataText == null)
            {
                EditorGUILayout.LabelField("PaintDataText has not been set!");
            }

            GUI.color = guiColor;
        }

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }
}