using DefaultNamespace;
using UnityEngine;

public class MainDoorInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;

    [SerializeField] private Animator doorAnimator;
    public bool IsInteractable => m_interactable;
    private bool doorOpen = false;
    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Interacted with " + gameObject.name);
        doorOpen = !doorOpen;
        if (doorOpen)
        {
            // Code to open the fridge
            Debug.Log("Door is now open.");
            doorAnimator.SetTrigger("Open_Door");
        }
        else
        {
            // Code to close the fridge
            Debug.Log("Door is now closed.");
            doorAnimator.SetTrigger("Close_Door");
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