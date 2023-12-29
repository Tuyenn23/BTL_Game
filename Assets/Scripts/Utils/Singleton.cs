/*
 * Singleton.cs
 * 
 * - Unity Implementation of Singleton template
 * 
 */

using System;
using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _instantiated;
    protected bool isDestroy;

    public virtual void Awake()
    {
        isDestroy = false;
        var objects = FindObjectsOfType<T>();

        if (objects.Length > 1)
        {
            isDestroy = true;
            Destroy(gameObject);
        }
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

    public static T Instance
    {
        get
        {
            if (_instantiated) return _instance;

            var type = typeof(T);
            var attribute = Attribute.GetCustomAttribute(type, typeof(SingletonAttribute)) as SingletonAttribute;

            var objects = FindObjectsOfType<T>();

            if (objects.Length > 0)
            {
                _instance = objects[0];
                if (objects.Length > 1)
                {
                    //DebugCustom.LogWarning("There is more than one instance of Singleton of type \"" + type + "\". Keeping the first. Destroying the others.");
                    for (var i = 1; i < objects.Length; i++) DestroyImmediate(objects[i].gameObject);
                }

                if (attribute != null && attribute.IsDontDestroy)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }

                _instantiated = true;
                return _instance;
            }

            if (attribute == null)
            {
                //DebugCustom.LogError(type + "class does not have SingletonAttribute ! Please add SingletonAttribute for " + type);
                return null;
            }
            if (string.IsNullOrEmpty(attribute.Name))
            {
                //DebugCustom.LogError("Cannot find prefab of "+ type);
                return null;
            }

            GameObject prefab = Resources.Load(attribute.Name) as GameObject;
            if (prefab == null)
            {
                //DebugCustom.LogError("Cannot find prefab of " + type + "! Put prefab of" + type + " into Resources folder");
                return null;
            }

            GameObject gameObject = Instantiate(prefab);
            _instance = gameObject.GetComponent<T>();
            gameObject.name = type.ToString();

            _instantiated = true;
            return _instance;
        }

        private set
        {
            _instance = value;
            _instantiated = value != null;
        }
    }

    public static bool Instantiated
    {
        get { return _instantiated; }
    }

    public virtual void Init()
    {

    }

    private void OnDestroy() { _instantiated = false; }
}