using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UbhTimer))]
public class UbhTimerInspector : Editor
{
    private float m_orgTimeScale;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawProperties();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProperties()
    {
        UbhTimer obj = target as UbhTimer;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Pause UniBulletHell"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy)
            {
                UbhTimer.instance.Pause();
            }
        }
        if (GUILayout.Button("Resume UniBulletHell"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy)
            {
                UbhTimer.instance.Resume();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Pause TimeScale"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy)
            {
                m_orgTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
        }
        if (GUILayout.Button("Resume TimeScale"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy && Time.timeScale == 0f)
            {
                Time.timeScale = m_orgTimeScale;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }
}
