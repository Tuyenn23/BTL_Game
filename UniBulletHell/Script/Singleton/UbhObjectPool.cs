// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// *Please enable this define if you want to use the DarkTonic's CoreGameKit pooling system.
// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// #define USING_CORE_GAME_KIT

using System;
using System.Collections.Generic;
using UnityEngine;

#if USING_CORE_GAME_KIT
using DarkTonic.CoreGameKit;
#endif

/// <summary>
/// Ubh object pool.
/// </summary>
[AddComponentMenu("UniBulletHell/Manager/Object Pool")]
[DisallowMultipleComponent]
public sealed class UbhObjectPool : UbhSingletonMonoBehavior<UbhObjectPool>
{
    [Serializable]
    private class InitializePool
    {
        public GameObject m_bulletPrefab = null;
        public int m_initialPoolNum = 0;
    }

    [SerializeField]
    private List<InitializePool> m_initializePoolList = null;

#if USING_CORE_GAME_KIT
    // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
    private PoolBoss m_poolBoss = null;
    private HashSet<Transform> m_poolPrefabTransformList = new HashSet<Transform>();

    /// <summary>
    /// Initialize CoreGameKit's PoolBoss
    /// </summary>
    private void InitializePoolBoss()
    {
        if (m_poolBoss == null)
        {
            // PoolBoss Initialize
            m_poolBoss = FindObjectOfType<PoolBoss>();
            if (m_poolBoss == null)
            {
                m_poolBoss = new GameObject(typeof(PoolBoss).Name).AddComponent<PoolBoss>();
            }
            m_poolBoss.autoAddMissingPoolItems = true;

            if (PoolBoss.IsReady == false)
            {
                // Force PoolBoss to be initialized.
                GameObject goDummyPoolBossInit = new GameObject("DummyPoolBossInit");
                goDummyPoolBossInit.SetActive(false);
                PoolBoss.CreateNewPoolItem(goDummyPoolBossInit.transform, 1, false, 1, false, null, PoolBoss.PrefabSource.Prefab);
            }
        }
    }
#else
    private class PoolingParam
    {
        public List<UbhBullet> m_bulletList = new List<UbhBullet>(1024);
        public int m_searchStartIndex = 0;
    }

    private Dictionary<int, PoolingParam> m_pooledBulletDic = new Dictionary<int, PoolingParam>(256);
#endif

    protected override void Awake()
    {
        base.Awake();

        if (this == null ||
            gameObject == null ||
            this != instance)
        {
            // Debug.Log("There is another UbhObjectPool.");

            if (instance != null)
            {
                if (m_initializePoolList != null && m_initializePoolList.Count > 0)
                {
                    for (int i = 0; i < m_initializePoolList.Count; i++)
                    {
                        instance.CreatePool(m_initializePoolList[i].m_bulletPrefab, m_initializePoolList[i].m_initialPoolNum);
                    }
                }
            }
        }
    }

    protected override void DoAwake()
    {
        transform.hierarchyCapacity = 2048;

        if (m_initializePoolList != null && m_initializePoolList.Count > 0)
        {
            for (int i = 0; i < m_initializePoolList.Count; i++)
            {
                CreatePool(m_initializePoolList[i].m_bulletPrefab, m_initializePoolList[i].m_initialPoolNum);
            }
        }
    }

    /// <summary>
    /// Create object pool from prefab.
    /// </summary>
    public void CreatePool(GameObject goPrefab, int createNum)
    {
        for (int i = 0; i < createNum; i++)
        {
            UbhBullet bullet = GetBullet(goPrefab, UbhUtil.VECTOR3_ZERO, true);
            if (bullet == null)
            {
                break;
            }
            ReleaseBullet(bullet);
        }
    }

    /// <summary>
    /// Remove object pool.
    /// </summary>
    public void RemovePool(GameObject goPrefab)
    {
#if USING_CORE_GAME_KIT
        // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
        InitializePoolBoss();
        Transform transPrefab = goPrefab.transform;
        PoolBoss.DestroyPoolItem(transPrefab, PoolBoss.PrefabSource.Prefab);
        m_poolPrefabTransformList.Remove(transPrefab);
#else
        int key = goPrefab.GetInstanceID();
        if (m_pooledBulletDic.ContainsKey(key))
        {
            PoolingParam poolParam = m_pooledBulletDic[key];
            RemovePoolParam(poolParam);
        }
#endif
    }

    /// <summary>
    /// Remove all object pool.
    /// </summary>
    public void RemoveAllPool()
    {
#if USING_CORE_GAME_KIT
        // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
        InitializePoolBoss();
        foreach (Transform transPrefab in m_poolPrefabTransformList)
        {
            if (transPrefab != null)
            {
                PoolBoss.DestroyPoolItem(transPrefab, PoolBoss.PrefabSource.Prefab);
                m_poolPrefabTransformList.Remove(transPrefab);
            }
        }
        m_poolPrefabTransformList.Clear();
#else
        foreach (PoolingParam poolParam in m_pooledBulletDic.Values)
        {
            RemovePoolParam(poolParam);
        }
#endif
    }

#if !USING_CORE_GAME_KIT
    private void RemovePoolParam(PoolingParam poolParam)
    {
        if (poolParam == null)
        {
            return;
        }

        poolParam.m_searchStartIndex = 0;

        if (poolParam.m_bulletList != null &&
            poolParam.m_bulletList.Count > 0)
        {
            for (int i = 0; i < poolParam.m_bulletList.Count; i++)
            {
                UbhBullet bullet = poolParam.m_bulletList[i];
                if (bullet != null && bullet.gameObject != null)
                {
                    Destroy(bullet.gameObject);
                    Destroy(bullet);
                    poolParam.m_bulletList[i] = null;
                }
            }

            poolParam.m_bulletList.Clear();
        }
    }
#endif

    /// <summary>
    /// Get Bullet from object pool or instantiate.
    /// </summary>
    public UbhBullet GetBullet(GameObject goPrefab, Vector3 position, bool forceInstantiate = false)
    {
        if (goPrefab == null)
        {
            return null;
        }

        UbhBullet bullet = null;

#if USING_CORE_GAME_KIT
        // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
        InitializePoolBoss();
        Transform transPrefab = goPrefab.transform;
        Transform trans = PoolBoss.Spawn(transPrefab, position, UbhUtil.QUATERNION_IDENTITY, transform);
        if (trans == null)
        {
            return null;
        }
        m_poolPrefabTransformList.Add(transPrefab);

        bullet = trans.GetComponent<UbhBullet>();
        if (bullet == null)
        {
            bullet = trans.gameObject.AddComponent<UbhBullet>();
        }
#else
        int key = goPrefab.GetInstanceID();

        if (m_pooledBulletDic.ContainsKey(key) == false)
        {
            m_pooledBulletDic.Add(key, new PoolingParam());
        }

        PoolingParam poolParam = m_pooledBulletDic[key];

        if (forceInstantiate == false && poolParam.m_bulletList.Count > 0)
        {
            if (poolParam.m_searchStartIndex < 0 || poolParam.m_searchStartIndex >= poolParam.m_bulletList.Count)
            {
                poolParam.m_searchStartIndex = poolParam.m_bulletList.Count - 1;
            }

            for (int i = poolParam.m_searchStartIndex; i >= 0; i--)
            {
                if (poolParam.m_bulletList[i] == null || poolParam.m_bulletList[i].gameObject == null)
                {
                    poolParam.m_bulletList.RemoveAt(i);
                    continue;
                }
                if (poolParam.m_bulletList[i].isActive == false)
                {
                    poolParam.m_searchStartIndex = i - 1;
                    bullet = poolParam.m_bulletList[i];
                    break;
                }
            }
            if (bullet == null)
            {
                for (int i = poolParam.m_bulletList.Count - 1; i > poolParam.m_searchStartIndex; i--)
                {
                    if (poolParam.m_bulletList[i] == null || poolParam.m_bulletList[i].gameObject == null)
                    {
                        poolParam.m_bulletList.RemoveAt(i);
                        continue;
                    }
                    if (i < poolParam.m_bulletList.Count && poolParam.m_bulletList[i].isActive == false)
                    {
                        poolParam.m_searchStartIndex = i - 1;
                        bullet = poolParam.m_bulletList[i];
                        break;
                    }
                }
            }
        }

        if (bullet == null)
        {
            GameObject go = Instantiate(goPrefab, transform);
            bullet = go.GetComponent<UbhBullet>();
            if (bullet == null)
            {
                bullet = go.AddComponent<UbhBullet>();
            }
            poolParam.m_bulletList.Add(bullet);
            poolParam.m_searchStartIndex = poolParam.m_bulletList.Count - 1;
        }

        bullet.transform.SetPositionAndRotation(position, UbhUtil.QUATERNION_IDENTITY);
#endif

        bullet.SetActive(true);

        if (bullet == null)
        {
            return null;
        }

        UbhBulletManager.instance.AddBullet(bullet);

        return bullet;
    }

    /// <summary>
    /// Releases bullet (back to pool or destroy).
    /// </summary>
    public void ReleaseBullet(UbhBullet bullet, bool destroy = false)
    {
        if (bullet == null || bullet.gameObject == null)
        {
            return;
        }

        bullet.OnFinishedShot();

        UbhBulletManager.instance.RemoveBullet(bullet, destroy);

#if USING_CORE_GAME_KIT
        // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
        InitializePoolBoss();
        PoolBoss.Despawn(bullet.transform);
#else
        if (destroy)
        {
            Destroy(bullet.gameObject);
            Destroy(bullet);
            bullet = null;
            return;
        }
#endif
        bullet.SetActive(false);
    }

    /// <summary>
    /// Releases all bullet (back to pool or destroy).
    /// </summary>
    public void ReleaseAllBullet(bool destroy = false)
    {
#if USING_CORE_GAME_KIT
        // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
        InitializePoolBoss();
        foreach (Transform transPrefab in m_poolPrefabTransformList)
        {
            if (transPrefab != null)
            {
                PoolBoss.DespawnAllOfPrefab(transPrefab);
            }
        }
        m_poolPrefabTransformList.Clear();
#else
        foreach (PoolingParam poolParam in m_pooledBulletDic.Values)
        {
            if (poolParam.m_bulletList != null)
            {
                for (int i = 0; i < poolParam.m_bulletList.Count; i++)
                {
                    UbhBullet bullet = poolParam.m_bulletList[i];
                    if (bullet != null && bullet.gameObject != null && bullet.isActive)
                    {
                        ReleaseBullet(bullet, destroy);
                    }
                }
            }
        }
#endif
    }

    /// <summary>
    /// Get active bullets list.
    /// </summary>
    public List<UbhBullet> GetActiveBulletsList(GameObject goPrefab)
    {
        int key = goPrefab.GetInstanceID();
        List<UbhBullet> findList = null;

#if USING_CORE_GAME_KIT
        // +++++ Replace PoolingSystem with DarkTonic's CoreGameKit. +++++
        InitializePoolBoss();
        if (PoolBoss.Instance.poolItems != null && PoolBoss.Instance.poolItems.Count > 0)
        {
            for (int i = 0; i < PoolBoss.Instance.poolItems.Count; i++)
            {
                if (PoolBoss.Instance.poolItems[i].prefabTransform.gameObject.GetInstanceID() == key)
                {
                    var poolIteminfo = PoolBoss.PoolItemInfoByName(PoolBoss.Instance.poolItems[i].prefabTransform.gameObject.name);
                    if (poolIteminfo != null && poolIteminfo.SpawnedClones != null && poolIteminfo.SpawnedClones.Count > 0)
                    {
                        for (int k = 0; k < poolIteminfo.SpawnedClones.Count; k++)
                        {
                            if (poolIteminfo.SpawnedClones[k] == null)
                            {
                                continue;
                            }
                            var bullet = poolIteminfo.SpawnedClones[k].GetComponent<UbhBullet>();
                            if (bullet == null)
                            {
                                continue;
                            }
                            if (findList == null)
                            {
                                findList = new List<UbhBullet>(1024);
                            }
                            findList.Add(bullet);
                        }
                    }
                }
            }
        }
#else
        if (m_pooledBulletDic.ContainsKey(key))
        {
            PoolingParam poolParam = m_pooledBulletDic[key];

            for (int i = 0; i < poolParam.m_bulletList.Count; i++)
            {
                UbhBullet bullet = poolParam.m_bulletList[i];
                if (bullet != null && bullet.gameObject != null && bullet.isActive)
                {
                    if (findList == null)
                    {
                        findList = new List<UbhBullet>(1024);
                    }
                    findList.Add(poolParam.m_bulletList[i]);
                }
            }
        }
#endif
        return findList;
    }
}