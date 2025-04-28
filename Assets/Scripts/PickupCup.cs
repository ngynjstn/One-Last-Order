using DefaultNamespace;
using System.Collections.Generic;
using UnityEngine;

public class PickupCup : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to pick up cup";
    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = false;

    // Different pickup and serving states
    private bool coffeePickupReady = false;
    private bool frothingComplete = false;
    public bool isHotCoffee = false; // Track if coffee is hot (frothed) or cold
    public bool isPlaced = false;

    // Check both conditions for interactability
    public bool IsInteractable => m_interactable && (coffeePickupReady || frothingComplete);

    [SerializeField] private Transform handTransform;
    private CoffeeInteractable coffeeInteractable;
    public bool coffeeDone = false;
    public bool readyToServe = false; // Coffee is ready to be served
    [SerializeField] public List<string> cupContents = new List<string>();
    public void AddContent(string content)
    {
        cupContents.Add(content);
        Debug.Log($"Added {content} to cup.");
    }
    public void ClearContent()
    {
        cupContents.Clear();
    }
    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Attempting to pick up cup from machine.");

        // Check if we can pick up (either after coffee is done or after frothing)
        if (!coffeePickupReady && !frothingComplete)
        {
            Debug.LogWarning("Cannot pick up cup yet - neither coffee nor frothing is complete!");
            return;
        }

        GameObject cup = CupInteractable.curr;
        if (cup != null && handTransform != null)
        {
            PickUp(cup);

            // If we're picking up from the coffee machine
            if (coffeePickupReady && !frothingComplete)
            {
                isPlaced = false;
                // Coffee is cold by default if not frothed
                isHotCoffee = false;
                readyToServe = true;
                Debug.Log("Cold coffee ready to serve");
            }
            // If we're picking up after frothing is complete
            else if (frothingComplete)
            {
                // Reset coffee state only after the entire process is complete
                // CoffeeInteractable.ResetCoffeeState();

                // Find and reset frother
                GameObject frother = GameObject.FindGameObjectWithTag("Frother");
                if (frother != null)
                {
                    FrotherInteractable frothScript = frother.GetComponent<FrotherInteractable>();
                    if (frothScript != null)
                    {
                        frothScript.ResetFrotherState();
                    }
                }

                // Mark as hot coffee and ready to serve
                isHotCoffee = true;
                readyToServe = true;
                Debug.Log("Hot coffee ready to serve");
            }

            Debug.Log("Cup picked up: " + cup.name);
            // Reset states
            m_interactable = false;
            coffeePickupReady = false;
        }
        else
        {
            Debug.LogWarning("No cup to pick up or missing hand transform.");
        }
    }

    // Enable pickup after coffee is done
    public void EnableCoffeePickup()
    {
        coffeePickupReady = true;
        Debug.Log("Coffee done, cup can now be picked up from coffee machine");
    }

    // Mark frothing as complete
    public void CompleteFrothing()
    {
        frothingComplete = true;
        Debug.Log("Frothing complete, cup can now be picked up");
    }

    public void SetHand(Transform hand)
    {
        handTransform = hand;
    }

    private void PickUp(GameObject cup)
    {
        cup.transform.SetParent(handTransform);
        cup.transform.position = handTransform.position;
        cup.transform.rotation = handTransform.rotation;
        cup.transform.localPosition = Vector3.zero;
        cup.transform.localRotation = Quaternion.identity;
        isPlaced = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    // Reset the cup after serving or discarding
    public void ResetCup()
    {
        readyToServe = false;
        isHotCoffee = false;
        frothingComplete = false;
        coffeePickupReady = false;
        coffeeDone = false;

        // Allow the player to get a new cup
        GameObject cupDispenser = GameObject.FindGameObjectWithTag("CupDispenser");
        if (cupDispenser != null)
        {
            CupInteractable dispScript = cupDispenser.GetComponent<CupInteractable>();
            if (dispScript != null)
            {
                // Make cup dispenser interactable again
                dispScript.ResetCupInteractable();
            }
        }

        // Destroy this cup
        Destroy(gameObject);
    }

    void Start()
    {
        m_interactable = false;
        coffeePickupReady = false;
        frothingComplete = false;
        readyToServe = false;
        isHotCoffee = false;
    }

    void Update()
    {
        // Cup is interactable if coffee is done or frothing is complete
        m_interactable = coffeeDone && (coffeePickupReady || frothingComplete);
    }
}