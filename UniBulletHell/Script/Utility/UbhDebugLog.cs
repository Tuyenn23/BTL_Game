using System.Diagnostics;
using System.Text;
using UnityEngine;

/// <summary>
/// The class that wrapped the Unity debug log.
/// </summary>
public static class UbhDebugLog
{
    private const string PREFIX = "[UBH] ";

    private static bool s_enableDebugLog = true;

    public static void EnableDebugLog()
    {
        s_enableDebugLog = true;
    }

    public static void DisableDebugLog()
    {
        s_enableDebugLog = false;
    }

    public static bool IsEnableLog()
    {
        return s_enableDebugLog;
    }

    public static void Log(object message)
    {
        if (s_enableDebugLog)
        {
            UnityEngine.Debug.Log(PREFIX + message);
        }
    }

    public static void Log(object message, Object context)
    {
        if (s_enableDebugLog)
        {
            UnityEngine.Debug.Log(PREFIX + message, context);
        }
    }

    public static void LogWarning(object message)
    {
        if (s_enableDebugLog)
        {
            UnityEngine.Debug.LogWarning(PREFIX + message);
        }
    }

    public static void LogWarning(object message, Object context)
    {
        if (s_enableDebugLog)
        {
            UnityEngine.Debug.LogWarning(PREFIX + message, context);
        }
    }

    public static void LogError(object message)
    {
        if (s_enableDebugLog)
        {
            UnityEngine.Debug.LogError(PREFIX + message);
        }
    }

    public static void LogError(object message, Object context)
    {
        if (s_enableDebugLog)
        {
            UnityEngine.Debug.LogError(PREFIX + message, context);
        }
    }
}
