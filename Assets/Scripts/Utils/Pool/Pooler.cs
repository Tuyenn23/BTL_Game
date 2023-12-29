using System.Collections.Generic;
using UnityEngine;
public class Pooler : Singleton<Pooler>
{
    private Dictionary<int, ObjectPooling<PoolElement>> _pools;
    protected override void OnAwake()
    {
        base.OnAwake();
        Instance.Init();
    }
    public void Clear()
    {
        if (_pools != null)
            _pools.Clear();
    }
    public PoolElement Spawn(PoolElement prefab, Vector3 pos, Quaternion rotation)
    {
        PoolElement temp = null;
        int poolID = prefab.GetInstanceID();
        if (_pools == null)
            _pools = new Dictionary<int, ObjectPooling<PoolElement>>();
        if (_pools.ContainsKey(poolID))
        {
            if (!_pools[poolID].IsStackPoolNull())
            {
                temp = _pools[poolID].New();
                temp.transform.position = pos;
                temp.transform.rotation = rotation;
            }
        }
        else
        {
            _pools.Add(poolID, new ObjectPooling<PoolElement>());
        }
        if (temp == null)
        {
            temp = Instantiate(prefab, pos, rotation);
            temp.PoolID = poolID;
        }
        return temp;
    }
    public void Despawn(PoolElement element)
    {
        if (element == null)
            return;
        if (_pools == null)
            return;
        if (_pools.ContainsKey(element.PoolID))
        {
            element.gameObject.SetActive(false);
            _pools[element.PoolID].Store(element);
        }
    }
    public T Spawn<T>(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        GameObject g = Spawn(prefab, pos, rotation);
        T t = g.GetComponent<T>();
        return t;
    }
    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        GameObject temp = null;
        int poolID = prefab.GetInstanceID();
        if (_pools == null)
            _pools = new Dictionary<int, ObjectPooling<PoolElement>>();
        if (_pools.ContainsKey(poolID))
        {
            if (!_pools[poolID].IsStackPoolNull())
            {
                var gNew = _pools[poolID].New();
                if (gNew != null)
                {
                    temp = gNew.gameObject;
                    temp.transform.position = pos;
                    temp.transform.rotation = rotation;
                }
            }
        }
        else
        {
            _pools.Add(poolID, new ObjectPooling<PoolElement>());
        }
        if (temp == null)
        {
            temp = Instantiate(prefab, pos, rotation);
            PoolElement item = temp.GetComponent<PoolElement>();
            if (item == null)
                item = temp.AddComponent<PoolElement>();

            item.PoolID = poolID;
        }
        if (temp != null)
            temp.gameObject.SetActive(true);
        return temp;
    }
    public void Despawn(GameObject element)
    {
        if (element == null)
            return;
        if (_pools == null)
            return;
        PoolElement item = element.GetComponent<PoolElement>();
        if (item == null)
        {
            Debug.LogError("Despawn Error");
            return;
        }
        if (_pools.ContainsKey(item.PoolID))
        {
            element.gameObject.SetActive(false);
            _pools[item.PoolID].Store(item);
        }
    }
}

