using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public enum OrderType
    {
        HotCoffee,
        ColdCoffee
    }

    [SerializeField] private OrderType currentOrder;
    [SerializeField] private string customerName;
    [SerializeField] private bool orderFulfilled = false;

    // Visual feedback elements
    [SerializeField] private GameObject orderBubble;
    [SerializeField] private TMPro.TextMeshProUGUI orderText;

    public OrderType GetCurrentOrder()
    {
        return currentOrder;
    }

    public void GenerateRandomOrder()
    {
        // Random order (50/50 chance)
        currentOrder = Random.value > 0.5f ? OrderType.HotCoffee : OrderType.ColdCoffee;
        UpdateOrderDisplay();
    }

    public void SetOrder(OrderType order)
    {
        currentOrder = order;
        UpdateOrderDisplay();
    }

    private void UpdateOrderDisplay()
    {
        if (orderText != null)
        {
            string orderDescription = currentOrder == OrderType.HotCoffee ?
                "I'd like a hot coffee please!" :
                "Can I get a cold coffee?";

            orderText.text = $"{customerName}: {orderDescription}";
        }

        if (orderBubble != null)
        {
            orderBubble.SetActive(true);
        }
    }

    public bool CheckOrderFulfillment(bool isHot)
    {
        // Check if coffee temperature matches order
        bool isCorrect = (isHot && currentOrder == OrderType.HotCoffee) ||
                        (!isHot && currentOrder == OrderType.ColdCoffee);

        if (isCorrect)
        {
            orderFulfilled = true;
            if (orderBubble != null)
            {
                // Show thank you message
                if (orderText != null)
                {
                    orderText.text = $"{customerName}: Thank you!";
                }

                // Hide bubble after delay
                Invoke("HideOrderBubble", 2.0f);
            }
        }
        else
        {
            // Wrong order response
            if (orderText != null)
            {
                string wrongOrderText = isHot ?
                    "I wanted it cold..." :
                    "I asked for hot coffee...";
                orderText.text = $"{customerName}: {wrongOrderText}";
            }
        }

        return isCorrect;
    }

    private void HideOrderBubble()
    {
        if (orderBubble != null)
        {
            orderBubble.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateRandomOrder();
    }
}