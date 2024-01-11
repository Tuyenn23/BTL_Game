using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UbhObjectPool))]
public class UbhObjectPoolInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawProperties();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProperties()
    {
        UbhObjectPool obj = target as UbhObjectPool;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Release All Bullet"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy)
            {
                UbhObjectPool.instance.ReleaseAllBullet();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }
}
