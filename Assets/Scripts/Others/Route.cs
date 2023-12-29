using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPoint;
    private Vector2 gizmosPosition;

    private void OnDrawGizmos()
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - i, 3) * controlPoint[0].position + 3 
                * Mathf.Pow(1 - i, 2) * i * controlPoint[1].position + 3 
                * (1 - i) * Mathf.Pow(i, 2) * controlPoint[2].position 
                + Mathf.Pow(i, 3) * controlPoint[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }

        Gizmos.DrawLine(new Vector2(controlPoint[0].position.x, controlPoint[0].position.y), new Vector2(controlPoint[1].position.x, controlPoint[1].position.y));

        Gizmos.DrawLine(new Vector2(controlPoint[2].position.x, controlPoint[2].position.y), new Vector2(controlPoint[3].position.x, controlPoint[3].position.y));

    }
}
