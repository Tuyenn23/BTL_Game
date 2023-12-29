using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjectPooling<T> where T : class, new()
{
    private Stack<T> _objectStack;

    public ObjectPooling()
    {
        _objectStack = new Stack<T>();
    }

    public ObjectPooling(int initialBufferSize)
    {
        _objectStack = new Stack<T>(initialBufferSize);
    }

    public T New()
    {
        T t = null;

        if (_objectStack.Count > 0)
        {
            t = _objectStack.Pop();
        }

        return t;
    }

    public void Store(T obj)
    {
        if (_objectStack.Contains(obj))
            return;
        _objectStack.Push(obj);
    }

    public int Count
    {
        get { return _objectStack.Count; }
    }

    public void RemovePool()
    {
        _objectStack.Clear();
    }

    public bool IsStackPoolNull()
    {
        if (_objectStack.Count > 0)
        {
            return false;
        }
        return true;
    }
}
