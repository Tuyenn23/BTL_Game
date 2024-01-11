using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class UbhConditionalHideAttribute : PropertyAttribute
{
    public string m_conditionalSourceField = "";
    public bool m_hideInInspector = false;

    public UbhConditionalHideAttribute(string conditionalSourceField)
    {
        m_conditionalSourceField = conditionalSourceField;
        m_hideInInspector = false;
    }

    public UbhConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        m_conditionalSourceField = conditionalSourceField;
        m_hideInInspector = hideInInspector;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(UbhConditionalHideAttribute))]
public class UbhConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        UbhConditionalHideAttribute condHAtt = (UbhConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (condHAtt.m_hideInInspector == false || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        UbhConditionalHideAttribute condHAtt = (UbhConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (condHAtt.m_hideInInspector == false || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(UbhConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        bool enabled = true;
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, condHAtt.m_conditionalSourceField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.boolValue;
        }
        else
        {
            UbhDebugLog.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.m_conditionalSourceField);
        }

        return enabled;
    }
}
#endif