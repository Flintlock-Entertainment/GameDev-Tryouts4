using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Patroller
{
    [SerializeField]  protected Transform player;
    protected override void SelectNewTarget()
    {
        float maxDist = float.MinValue;
        Debug.Log(player.position);
        foreach (var target in allTargets)
        {
            var targetLocation = target.GetComponent<Transform>();
            var dist = CalculateDistance(targetLocation.position, player.position);
            if (maxDist < dist)
            {
                maxDist = dist;
                currentTarget = target;
            }
        }
        navMeshAgent.SetDestination(currentTarget.transform.position);
        //if (animator) animator.SetBool("Run", true);
        timeToWaitAtTarget = Random.Range(minWaitAtTarget, maxWaitAtTarget);
    }

    private float CalculateDistance(Vector3 point1, Vector3 point2)
    {
        var dx = point1.x - point2.x;
        var dy = point1.y - point2.y;
        var dz = point1.z - point2.z;
        return Mathf.Sqrt(
            dx * dx + dy * dy + dz * dz
            );
    }
}
