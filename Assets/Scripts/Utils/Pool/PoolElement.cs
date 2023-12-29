using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolElement : MonoBehaviour
{
    private int _poolID;
    Transform _Transform;
    Rigidbody2D _Rigidbody2D;
    public int PoolID
    {
        get
        {
            return _poolID;
        }
        set
        {
            _poolID = value;
        }
    }
    public new Transform transform
    {
        get
        {
            if (_Transform == null)
            {
                _Transform = GetComponent<Transform>();
            }
            return _Transform;
        }
    }

    public new Rigidbody2D rigidbody2D
    {
        get
        {
            if (_Rigidbody2D == null)
            {
                _Rigidbody2D = GetComponent<Rigidbody2D>();
            }
            return _Rigidbody2D;
        }
    }
}
