using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Patroller
{
    protected override void SelectNewTarget()
    {
        currentTarget = allTargets[5];
        navMeshAgent.SetDestination(currentTarget.transform.position);
        //if (animator) animator.SetBool("Run", true);
        timeToWaitAtTarget = Random.Range(minWaitAtTarget, maxWaitAtTarget);
    }
}
