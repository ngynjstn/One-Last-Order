using UnityEngine;
using DefaultNamespace;

public class CustomerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to serve customer";
    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private CustomerOrder customerOrder;
    [SerializeField] private Transform servicePoint; // Where to place the cup when serving
    [SerializeField] private float serviceTime = 3.0f; // How long before cup is "consumed"

    private bool isBeingServed = false;

    public void Interact()
    {
        Debug.Log("Attempting to serve customer");

        if (isBeingServed)
        {
            Debug.LogWarning("Already serving this customer!");
            return;
        }

        // Check if player has a ready-to-serve coffee
        GameObject cupObj = CupInteractable.curr;
        if (cupObj != null)
        {
            PickupCup cupScript = cupObj.GetComponent<PickupCup>();
            if (cupScript != null && cupScript.readyToServe)
            {
                // Place the cup at the service point
                PlaceCup(cupObj);

                // Check if order is correct
                bool orderCorrect = customerOrder.CheckOrderFulfillment(cupScript.isHotCoffee);

                if (orderCorrect)
                {
                    Debug.Log("Customer is happy with their order!");
                    // Play happy animation/sound if you have one

                    // Add score or progress as needed
                    GameManager.Instance.AddScore(10);
                }
                else
                {
                    Debug.Log("Wrong order served!");
                    // Play unhappy animation/sound if you have one
                }

                // Start consumption process
                StartCoroutine(ConsumeDrink(cupObj, serviceTime));
                isBeingServed = true;
            }
            else
            {
                Debug.LogWarning("You don't have a ready coffee to serve!");
            }
        }
        else
        {
            Debug.LogWarning("No cup to serve!");
        }
    }

    private void PlaceCup(GameObject cup)
    {
        // Place the cup at service point
        cup.transform.SetParent(servicePoint);
        cup.transform.position = servicePoint.position;
        cup.transform.rotation = servicePoint.rotation;
        cup.transform.localPosition = Vector3.zero;
        cup.transform.localRotation = Quaternion.identity;
    }

    private System.Collections.IEnumerator ConsumeDrink(GameObject cup, float time)
    {
        yield return new WaitForSeconds(time);

        // After consumption
        PickupCup cupScript = cup.GetComponent<PickupCup>();
        if (cupScript != null)
        {
            cupScript.ResetCup(); // This will destroy the cup and reset states
        }

        // Generate new order
        customerOrder.GenerateRandomOrder();
        isBeingServed = false;
    }

    void Start()
    {
        if (customerOrder == null)
        {
            customerOrder = GetComponent<CustomerOrder>();
        }
    }
}