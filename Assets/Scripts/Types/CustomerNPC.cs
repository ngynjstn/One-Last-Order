using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

public class CustomerNPC : MonoBehaviour
{
    public event Action OnCustomerComplete;
    
    protected NavMeshAgent agent;
    protected bool interactionComplete = false;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public virtual void SetDestination(Vector3 position)
    {
        if (agent != null)
            agent.SetDestination(position);
    }

    protected virtual void CompleteInteraction()
    {
        if (!interactionComplete)
        {
            interactionComplete = true;
            OnCustomerComplete?.Invoke();
            // Handle exit behavior
            StartCoroutine(ExitStore());
        }
    }

    protected virtual IEnumerator ExitStore()
    {
        // Implementation for customer leaving the store
        yield return null;
    }
}
