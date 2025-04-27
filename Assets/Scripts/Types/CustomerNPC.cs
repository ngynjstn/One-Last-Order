using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using UnityEngine.Pool;
using DefaultNamespace;
using DialogueEditor;
using System.Collections.Generic;

public class CustomerNPC : MonoBehaviour, IInteractable
{

    public event Action OnCustomerComplete;
    
    protected NavMeshAgent agent;
    public Animator animator;

    protected bool interactionComplete = false;
    public bool TalkedToCustomerOnce = false;
    public NPCConversation beginningDialogue;
    public NPCConversation hasNoDrinkYet;
    public string playersCurrentDrink = "";
    public bool playerHasFinalDrink = false;
    public bool reachedCounter;
    

    public string InteractableHintText => "Press E to Talk with Customer";

    public bool IsInteractable => m_isInteractable;
    public bool m_isInteractable = true;

    PickupCup pickupCup;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (NavMeshWrapper.ReachedDestinationOrGaveUp(agent) && !reachedCounter)
        {
            reachedCounter = true;
            agent.isStopped = true;
            agent.ResetPath();
            // Handle the case where the customer has reached the counter
            Debug.Log("Customer has reached the counter.");
            FaceThePlayer();
            return;
        }
    }
    public void SetTalkedToCustomerOnce()
    {
        TalkedToCustomerOnce = true;
        //m_isInteractable = false;
    }
    private void FaceThePlayer()
    {
        GameObject m_player = GameObject.FindGameObjectWithTag("Player");
        // Implementation for facing the player
        Vector3 direction = (m_player.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = lookRotation;
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

    public void Interact()
    {
        if (!TalkedToCustomerOnce)
        {
            ConversationManager.Instance.StartConversation(beginningDialogue);
            return;
        }
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        pickupCup = obj.GetComponent<PickupCup>();
        List<string> drink = pickupCup.cupContents;

        if (drink.Contains("coffee") && drink.Contains("frother"))
        {
            playerHasFinalDrink = true;
            Debug.Log("Drink contains what's needed.");
            CompleteInteraction();
        }

        if (!playerHasFinalDrink) {
            // Handle the case where the player has the final drink
            Debug.Log("Player won't take the drink. It's incomplete.");
            // You can add logic here to give the drink to the customer or whatever is needed
            //CompleteInteraction();
            ConversationManager.Instance.StartConversation(hasNoDrinkYet);
            return;
        }


        // Handle the case where the customer has already been talked to
        // Debug.Log("Customer has already been talked to.");

    }
}
