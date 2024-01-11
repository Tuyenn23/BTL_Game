using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class UbhAutoDespawnParticle : UbhMonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(CheckIfAliveCoroutine());
    }

    private IEnumerator CheckIfAliveCoroutine()
    {
        ParticleSystem pSystem = GetComponent<ParticleSystem>();

        while (true)
        {

            yield return new WaitForSeconds(1.0f);

            if (pSystem.IsAlive(true) == false)
            {
                Destroy(gameObject);
            }
        }
    }
}
