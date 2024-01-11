using UnityEngine;

/// <summary>
/// Ubh singleton mono behavior.
/// </summary>
public class UbhSingletonMonoBehavior<T> : UbhMonoBehaviour where T : UbhMonoBehaviour
{
    private static T s_instance;
    private static bool s_instanceCreated;
    private static bool s_isQuitting;

    /// <summary>
    /// Get singleton instance.
    /// </summary>
    public static T instance
    {
        get
        {
            if (s_isQuitting || Application.isPlaying == false)
            {
                return null;
            }

            if (s_instanceCreated == false)
            {
                CreateInstance();
            }

            return s_instance;
        }
    }

    /// <summary>
    /// Create Singleton Instance
    /// </summary>
    public static void CreateInstance(Transform parent = null)
    {
        s_instanceCreated = true;

        if (s_instance == null)
        {
            s_instance = FindObjectOfType<T>();

            if (s_instance == null)
            {
                UbhDebugLog.Log(typeof(T).Name + " Create instance.");
                new GameObject(typeof(T).Name).AddComponent<T>();
            }
        }

        if (parent != null)
        {
            s_instance.transform.SetParent(parent, false);
        }
    }

    /// <summary>
    /// Base Awake
    /// Call from override Awake method in inheriting classes.
    /// Example : protected override void Awake () { base.Awake (); }
    /// </summary>
    protected virtual void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this as T;
        }
        else if (s_instance != this)
        {
            GameObject go = gameObject;
            Destroy(this);
            Destroy(go);
            return;
        }

        DontDestroyOnLoad(gameObject);

        DoAwake();
    }

    /// <summary>
    /// Inheritance Awake
    /// </summary>
    protected virtual void DoAwake() { }

    /// <summary>
    /// Call from override OnDestroy method in inheriting classes.
    /// Example : protected override void OnDestroy () { base.OnDestroy (); }
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (this == s_instance)
        {
            s_instance = null;
            s_instanceCreated = false;
        }
    }

    /// <summary>
    /// Call from override OnApplicationQuit method in inheriting classes.
    /// Example : protected override void OnApplicationQuit () { base.OnApplicationQuit (); }
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        s_isQuitting = true;
    }
}
