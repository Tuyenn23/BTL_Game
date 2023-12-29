using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    [SerializeField] float time;
   

    private void OnEnable()
    {
        Destroy(gameObject, time);

    }
}
