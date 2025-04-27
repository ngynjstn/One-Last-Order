using DefaultNamespace;
using UnityEngine;

public class IceFridgeInteractable : MonoBehaviour, IInteractable
{
    public string InteractableHintText => "Press E to add ice to the cup";

    public bool IsInteractable => m_interactable;
    private bool m_interactable = true;

    public void Interact()
    {
        throw new System.NotImplementedException();

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
