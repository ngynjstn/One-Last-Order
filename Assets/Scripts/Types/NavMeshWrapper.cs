using UnityEngine;
using UnityEngine.AI;

public static class NavMeshWrapper
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static bool ReachedDestinationOrGaveUp(this NavMeshAgent navMeshAgent) { if (!navMeshAgent.pathPending) { if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) { if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) { return true; } } } return false; }

}
