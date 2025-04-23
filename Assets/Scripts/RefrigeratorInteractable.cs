using DefaultNamespace;
using UnityEngine;

public class RefrigeratorInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;

    [SerializeField] private Animator fridgeAnimator;
    public bool IsInteractable => m_interactable;
    private bool fridgeOpen = false;
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        fridgeOpen = !fridgeOpen;
        if (fridgeOpen)
        {
            // Code to open the fridge
            Debug.Log("Fridge is now open.");
            fridgeAnimator.SetTrigger("Open_Fridge");
        }
        else
        {
            // Code to close the fridge
            Debug.Log("Fridge is now closed.");
            fridgeAnimator.SetTrigger("Close_Fridge");
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
