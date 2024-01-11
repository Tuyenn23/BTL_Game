using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(UbhShotCtrl))]
public class UbhShotCtrlInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawProperties();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProperties()
    {
        var obj = target as UbhShotCtrl;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Shot Routine"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy)
            {
                obj.StartShotRoutine();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Stop Shot Routine"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy)
            {
                obj.StopShotRoutine();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Stop Shot Routine And Playing Shot"))
        {
            if (Application.isPlaying && obj.gameObject.activeInHierarchy)
            {
                obj.StopShotRoutineAndPlayingShot();
            }
        }
        EditorGUILayout.EndHorizontal();

        Color guiColor = GUI.color;
        if (obj.m_shotList == null || obj.m_shotList.Count <= 0)
        {
            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("*****WARNING*****");
            EditorGUILayout.LabelField("Size of ShotList is 0!");
            GUI.color = guiColor;

        }
        else
        {
            bool isShotErr = true;
            foreach (UbhShotCtrl.ShotInfo shotInfo in obj.m_shotList)
            {
                if (shotInfo.m_shotObj != null)
                {
                    isShotErr = false;
                    break;
                }
            }
            bool isDelayErr = true;
            foreach (UbhShotCtrl.ShotInfo shotInfo in obj.m_shotList)
            {
                if (0f < shotInfo.m_afterDelay)
                {
                    isDelayErr = false;
                    break;
                }
            }
            if (isShotErr || isDelayErr)
            {
                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("*****WARNING*****");
                if (isShotErr)
                {
                    EditorGUILayout.LabelField("Some ShotObj of ShotList has not been set!");
                }
                if (obj.m_loop && isDelayErr)
                {
                    EditorGUILayout.LabelField("Loop is true and All AfterDelay of ShotList is zero!");
                }
                GUI.color = guiColor;
            }
        }

        EditorGUILayout.Space();

        DrawDefaultInspector();
    }
}