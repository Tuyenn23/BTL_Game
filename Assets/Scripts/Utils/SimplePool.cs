using System.Collections.Generic;
using Unicorn;
using UnityEngine;

public static class SimplePool
{

    // You can avoid resizing of the Stack's internal array by
    // setting this to a number equal to or greater to what you
    // expect most of your pool sizes to be.
    // Note, you can also use Preload() to set the initial size
    // of a pool -- this can be handy if only some of your pools
    // are going to be exceptionally large (for example, your bullets.)
    private const int DEFAULT_POOL_SIZE = 3;

    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    private class Pool
    {
        // We append an id to the name of anything we instantiate.
        // This is purely cosmetic.
        private int nextId = 1;

        // The structure containing our inactive objects.
        // Why a Stack and not a List? Because we'll never need to
        // pluck an object from the start or middle of the array.
        // We'll always just grab the last one, which eliminates
        // any need to shuffle the objects around in memory.
        private readonly Queue<GameObject> inactive;
        // The prefab that we are pooling
        private readonly GameObject prefab;
        public int StackCount
        {
            get
            {
                return inactive.Count;
            }
        }

        // Constructor
        public Pool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;

            // If Stack uses a linked list internally, then this
            // whole initialQty thing is a placebo that we could
            // strip out for more minimal code.
            inactive = new Queue<GameObject>(initialQty);
        }

        // Spawn an object from our pool
        public GameObject Spawn(Vector3 pos, Quaternion rot, bool isActive = true)
        {
            while (true)
            {
                GameObject obj;
                if (inactive.Count == 0)
                {
                    // We don't have an object in our pool, so we
                    // instantiate a whole new object.
                    obj = GameObject.Instantiate(prefab, pos, rot);
                    //obj.transform.SetParent(PrefabStorage.Instance.transform, true);
#if LOG_HACK
                    obj.name = string.Format("{0} ({1})", prefab.name, nextId++);
#endif
                    // Add a PoolMember component so we know what pool
                    // we belong to.
                    obj.AddComponent<PoolMember>().myPool = this;
                }
                else
                {
                    // Grab the last object in the inactive array
                    obj = inactive.Dequeue();

                    if (obj == null)
                    {
                        // The inactive object we expected to find no longer exists.
                        // The most likely causes are:
                        //   - Someone calling Destroy() on our object
                        //   - A scene change (which will destroy all our objects).
                        //     NOTE: This could be prevented with a DontDestroyOnLoad
                        //	   if you really don't want this.
                        // No worries -- we'll just try the next one in our sequence.

                        continue;
                    }
                }

                obj.transform.position = pos;
                obj.transform.rotation = rot;
                obj.SetActive(isActive);
                return obj;
            }
        }

        public T Spawn<T>(Vector3 pos, Quaternion rot, bool isActive = true)
        {
            return Spawn(pos, rot, isActive).GetComponent<T>();
        }
        //public void ManuelPush(GameObject obj)
        //{
        //    inactive.Push(obj);
        //}
        // Return an object to the inactive pool.
        public void Despawn(GameObject obj)
        {
            if (!obj.activeSelf)
                return;
            obj.SetActive(false);

            // Since Stack doesn't have a Capacity member, we can't control
            // the growth factor if it does have to expand an internal array.
            // On the other hand, it might simply be using a linked list 
            // internally.  But then, why does it allow us to specificy a size
            // in the constructor? Stack is weird.
            inactive.Enqueue(obj);
            // Debug.Log(obj.name + "<color=yellow>déspawn</color>");
        }

        /// <summary>
        /// Clear all object on stack
        /// </summary>
        public void Clear()
        {
            while (inactive.Count > 0)
            {
                GameObject.Destroy(inactive.Dequeue());
            }
            inactive.Clear();
        }

    }


    /// <summary>
    /// Added to freshly instantiated objects, so we can link back
    /// to the correct pool on despawn.
    /// </summary>
    private class PoolMember : MonoBehaviour
    {
        public Pool myPool;
    }

    // All of our pools
    private static Dictionary<GameObject, Pool> pools;

    /// <summary>
    /// Init our dictionary.
    /// </summary>
    private static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        if (pools == null)
        {
            pools = new Dictionary<GameObject, Pool>();
        }
        if (prefab != null && pools.ContainsKey(prefab) == false)
        {
            pools[prefab] = new Pool(prefab, qty);
        }
    }

    /// <summary>
    /// If you want to preload a few copies of an object at the start
    /// of a scene, you can use this. Really not needed unless you're
    /// going to go from zero instances to 10+ very quickly.
    /// Could technically be optimized more, but in practice the
    /// Spawn/Despawn sequence is going to be pretty darn quick and
    /// this avoids code duplication.
    /// </summary>
    public static GameObject[] Preload(GameObject prefab, int qty = 1)
    {
        Init(prefab, qty);

        // Make an array to grab the objects we're about to pre-spawn.
        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++)
        {
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity, false);
        }

        // Now despawn them all.
        for (int i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }

        return obs;
    }

    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// NOTE: Remember that Awake() or Start() will only run on the very first
    /// spawn and that member variables won't get reset.  OnEnable will run
    /// after spawning -- but remember that toggling IsActive will also
    /// call that function.
    /// </summary>
    /// 
    public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, bool isActive = true)
    {
        Init(prefab);

        return pools[prefab].Spawn(pos, rot, isActive);
    }
    public static GameObject Spawn(GameObject prefab, bool isActive = true)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, isActive);
    }
    public static T Spawn<T>(T prefab, bool isActive = true) where T : Component
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, isActive);
    }
    public static T Spawn<T>(T prefab, Vector3 pos, Quaternion rot, bool isActive = true) where T : Component
    {
        Init(prefab.gameObject);
        return pools[prefab.gameObject].Spawn<T>(pos, rot, isActive);
    }

    /// <summary>
    /// Despawn the specified gameobject back into its pool.
    /// </summary>
    public static void Despawn(GameObject obj)
    {
        if (obj == null)
            return;

        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null)
        {
#if LOG_HACK
            Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
#endif
            GameObject.Destroy(obj);
        }
        else
        {
            pm.myPool.Despawn(obj);
            //pm.transform.SetParent(PrefabStorage.Instance.transform, true);
        }
    }
    /// <summary>
    /// Clear all 1 type of object  on pool
    /// </summary>
    /// <param name="prefab"></param>
    public static void ClearAllPoolType<T>(T prefab) where T: Component
    {
        if (prefab == null)
            return;
        if (pools == null)
        {
            Debug.Log("pool null");
            pools = new Dictionary<GameObject, Pool>();
        }

        if (pools.ContainsKey(prefab.gameObject) == false)
        {
            Debug.Log("not ct,eturn");
        }
        else
        {
            pools[prefab.gameObject].Clear();
            pools.Remove(prefab.gameObject);
        }
    }

    public static int GetStackCount(GameObject prefab) {
        if (pools == null)
            pools = new Dictionary<GameObject, Pool>();
        if (prefab == null) return 0;
        return pools.ContainsKey(prefab) ? pools[prefab].StackCount : 0;
    }

    public static void ClearPool()
    {
        if (pools != null)
        {
            pools.Clear();
        }
    }



}