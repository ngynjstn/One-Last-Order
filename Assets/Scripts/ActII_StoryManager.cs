using UnityEngine;
using System.Collections;
using System;
using DialogueEditor;

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
    [SerializeField] private Animator fadeAnimator;

    private int currentCustomerIndex = 0;
    private bool customerInProgress = false;
    private GameObject currentCustomer;

    [Header("Dialogues")]
    public NPCConversation beginningDialogue;
    public NPCConversation gotoCounterDialogue;
    public NPCConversation lookedAtRecipeSheetDialogue;
    public NPCConversation antagonistDialogueTwo;
    public NPCConversation exitStoreDialogue;


    [Header("Game objects necessary to subscribe to events")]
    [SerializeField] public LightToggleInteractable sign;

    private void Start()
    {
        fadeAnimator.SetTrigger("Fade_In");
        WaitForSeconds wait = new WaitForSeconds(5f);
        StartCoroutine(StartStorySequence());

    }
    /// ///BEGINNING TASK SEQUENCES
    public IEnumerator StartStorySequence()
    {
        yield return new WaitForSeconds(initialDelay);
        ConversationManager.Instance.StartConversation(beginningDialogue);
    }
    public void subscribetolight()
    {
        sign.LightToggledOn += signGotTurnedOn;
    }
    public void signGotTurnedOn() {
        ConversationManager.Instance.StartConversation(gotoCounterDialogue);
        sign.LightToggledOn -= signGotTurnedOn;
    }
    public void recipeSignLookedAt()
    {
        SpawnNextCustomer();
    }

    /// <summary>
    /// STORY DIALOGUE
    /// </summary>
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
                npc.SetDestination(spawnPoint.position);
            }

            StartCoroutine(PrepareNextCustomer());
        }
    }

    private IEnumerator PrepareNextCustomer()
    {
        customerInProgress = false;
        currentCustomerIndex++;
        if (currentCustomerIndex >= 8)
        {
            yield return new WaitForSeconds(5f);
            ConversationManager.Instance.StartConversation(exitStoreDialogue);
            yield return 0;
        }

        yield return new WaitForSeconds(timeBetweenCustomers);
        SpawnNextCustomer();
    }


    // ANTAGONIST SPECIFIC DIALOGUE
    public void TriggerDialogueTwo()
    {
        StartCoroutine(AntagonistDialogueWait());
    }
    public IEnumerator AntagonistDialogueWait()
    {
        yield return new WaitForSeconds(8f);
        ConversationManager.Instance.StartConversation(antagonistDialogueTwo);
    }
}
