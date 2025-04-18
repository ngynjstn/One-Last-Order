using UnityEngine;
using DefaultNamespace;

public class WindowInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to check the window";
    public string InteractableHintText => m_interactableHintText;

    private bool alreadyClosed = true;

    public void Interact()
    {
        if (alreadyClosed)
        {
            Debug.Log("The window's already closed. I need to fix it eventually.");
        }
        else
        {
            Debug.Log("You closed the window.");
            alreadyClosed = true;
        }
    }
}
