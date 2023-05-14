using UnityEngine;
using UnityEngine.AI;


/**
 * This component represents an NPC that patrols randomly between targets.
 * The targets are all the objects with a Target component inside a given folder.
 */
[RequireComponent(typeof(NavMeshAgent))]
public class Patroller: MonoBehaviour {
    [Tooltip("Minimum time to wait at target between running to the next target")]
    [SerializeField] protected float minWaitAtTarget = 7f;

    [Tooltip("Maximum time to wait at target between running to the next target")]
    [SerializeField] protected float maxWaitAtTarget = 15f;


    [Tooltip("A game object whose children have a Target component. Each child represents a target.")]
    [SerializeField] protected Transform targetFolder = null;
    protected Target[] allTargets = null;

    [Header("For debugging")]
    [SerializeField] protected Target currentTarget = null;
    [SerializeField] protected float timeToWaitAtTarget = 0;

    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    protected float rotationSpeed = 5f;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        allTargets = targetFolder.GetComponentsInChildren<Target>(false); // false = get components in active children only
        Debug.Log("Found " + allTargets.Length + " active targets.");
        SelectNewTarget();
    }

    protected virtual void SelectNewTarget() {
        currentTarget = allTargets[Random.Range(0, allTargets.Length - 1)];
        Debug.Log("New target: " + currentTarget.name);
        navMeshAgent.SetDestination(currentTarget.transform.position);
        //if (animator) animator.SetBool("Run", true);
        timeToWaitAtTarget = Random.Range(minWaitAtTarget, maxWaitAtTarget);
    }


    private void Update() {
        if (navMeshAgent.hasPath) {
            FaceDestination();
        } else {   // we are at the target
            //if (animator) animator.SetBool("Run", false);
            timeToWaitAtTarget -= Time.deltaTime;
            if (timeToWaitAtTarget <= 0)
                SelectNewTarget();
        }
    }

    private void FaceDestination() {
        Vector3 directionToDestination = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToDestination.x, 0, directionToDestination.z));
        //transform.rotation = lookRotation; // Immediate rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // Gradual rotation
    }


}
