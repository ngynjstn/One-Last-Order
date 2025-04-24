using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class PickupCup : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to pick up cup";
    public string InteractableHintText => m_interactableHintText;

    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private Transform handTransform;

    private CoffeeInteractable coffeeInteractable;
    public bool coffeeDone = false;
    public void Interact()
    {
        Debug.Log("Attempting to pick up cup from machine.");

        GameObject cup = CupInteractable.curr;

        if (cup != null && handTransform != null)
        {
            PickUp(cup);
            // TODO: set isPlaced to false and m_interactable to true if coffee is given to customer in coffeeInteractable script
            GameObject obj = GameObject.FindGameObjectWithTag("CoffeeMachine");
            coffeeInteractable = obj.GetComponent<CoffeeInteractable>();
            coffeeInteractable.isPlaced = false;
            Debug.Log("Cup picked up: " + cup.name);
            // set to false
            m_interactable = false;
        }
        else
        {
            Debug.LogWarning("No cup to pick up or missing hand transform.");
        }
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_interactable = coffeeDone;
    }
}
