using DefaultNamespace;
using UnityEngine;

public class PickupCup : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to pick up cup";
    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = false;

    // Different pickup states
    private bool coffeePickupReady = false;
    private bool frothingComplete = false;

    // Check both conditions for interactability
    public bool IsInteractable => m_interactable && (coffeePickupReady || frothingComplete);

    [SerializeField] private Transform handTransform;
    private CoffeeInteractable coffeeInteractable;
    public bool coffeeDone = false;
    
    public void Interact()
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
                GameObject obj = GameObject.FindGameObjectWithTag("CoffeeMachine");
                if (obj != null)
                {
                    coffeeInteractable = obj.GetComponent<CoffeeInteractable>();
                    if (coffeeInteractable != null)
                    {
                        coffeeInteractable.isPlaced = false;
                    }
                }
                // Don't reset coffee state if just moving from coffee machine to frother
                // This fixes the warning issue
            }
            // If we're picking up after frothing is complete
            else if (frothingComplete)
            {
                // Reset coffee state only after the entire process is complete
                CoffeeInteractable.ResetCoffeeState();

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

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Start()
    {
        m_interactable = false;
        coffeePickupReady = false;
        frothingComplete = false;
    }

    void Update()
    {
        // Cup is interactable if coffee is done or frothing is complete
        m_interactable = coffeeDone && (coffeePickupReady || frothingComplete);
    }
}