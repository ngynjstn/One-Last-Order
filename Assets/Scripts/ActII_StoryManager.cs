using UnityEngine;
using System.Collections;
using System;

public class ActII_StoryManager : MonoBehaviour
{
    [Header("Customer Prefabs")]
    [SerializeField] private GameObject[] customerPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform counterPoint;

    [Header("Timing")]
    [SerializeField] private float initialDelay = 2f;
    [SerializeField] private float timeBetweenCustomers = 3f;

    private int currentCustomerIndex = 0;
    private bool customerInProgress = false;
    private GameObject currentCustomer;

    private void Start()
    {
        StartCoroutine(StartStorySequence());
    }

    private IEnumerator StartStorySequence()
    {
        yield return new WaitForSeconds(initialDelay);
        SpawnNextCustomer();
    }

    private void SpawnNextCustomer()
    {
        if (currentCustomerIndex >= customerPrefabs.Length || customerInProgress)
            return;

        currentCustomer = Instantiate(customerPrefabs[currentCustomerIndex], spawnPoint.position, spawnPoint.rotation);
        customerInProgress = true;

        // Get the customer's NPC component and subscribe to its completion event
        if (currentCustomer.TryGetComponent<CustomerNPC>(out var npc))
        {
            npc.OnCustomerComplete += HandleCustomerComplete;
            npc.SetDestination(counterPoint.position);
        }
    }

    private void HandleCustomerComplete()
    {
        if (currentCustomer != null)
        {
            if (currentCustomer.TryGetComponent<CustomerNPC>(out var npc))
            {
                npc.OnCustomerComplete -= HandleCustomerComplete;
            }
            StartCoroutine(PrepareNextCustomer());
        }
    }

    private IEnumerator PrepareNextCustomer()
    {
        customerInProgress = false;
        currentCustomerIndex++;

        yield return new WaitForSeconds(timeBetweenCustomers);
        SpawnNextCustomer();
    }
}
