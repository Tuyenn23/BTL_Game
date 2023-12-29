using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFormationHelper
{

    public static void ApplyFormationCentering(ref List<Vector3> positions, float rowCount, float rowSpacing)
    {
        float offsetY = Mathf.Max(0, (rowCount - 1) * rowSpacing / 2);

        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            pos.y += offsetY;
            positions[i] = pos;
        }
    }

    /// <summary>
    /// Generates random "noise" for the position. In reality takes random
    /// range in the offset, does not use actual Math noise methods.
    /// </summary>
    /// <param name="factor">Factor for which the position can be offset.</param>
    /// <returns>Returns local offset for axes X and Z.</returns>
    public static Vector3 GetNoise(float factor)
    {
        return new Vector3(Random.Range(-factor, factor), 0, Random.Range(-factor, factor));
    }
}
