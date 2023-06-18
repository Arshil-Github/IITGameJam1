using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnaptoGrid : MonoBehaviour
{
    // Start is called before the first frame update
    void FixedUpdate()
    {
        transform.position = GetClosestEnemy(FindObjectsOfType<PlacementCell>()).position;
    }

    Transform GetClosestEnemy(PlacementCell[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (PlacementCell potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }
}
