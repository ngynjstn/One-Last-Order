using UnityEngine;
using DefaultNamespace;
using DialogueEditor;

public class TVInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to turn off the TV";
    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private Renderer screenRenderer;
    [SerializeField] private Material offMaterial;
    private bool isTurnedOff = false;

    [SerializeField] private NPCConversation dialogueObject;

    private void Start()
    {
        // If screenRenderer wasn't assigned in the inspector, try to get it
        if (screenRenderer == null)
        {
            screenRenderer = GetComponent<Renderer>();

            // If still null, try to find it in children
            if (screenRenderer == null)
            {
                screenRenderer = GetComponentInChildren<Renderer>();
                Debug.Log("Found renderer in children: " + (screenRenderer != null));
            }
        }

        Debug.Log("Start method - Screen renderer is " + (screenRenderer != null ? "assigned" : "NOT assigned"));
    }
    public void TurnOffTv()
    {
        Debug.Log("TVInteractable.Interact() was called.");

        // Try to get the renderer again if it's null
        if (screenRenderer == null)
        {
            screenRenderer = GetComponent<Renderer>();
            if (screenRenderer == null)
            {
                screenRenderer = GetComponentInChildren<Renderer>();
            }

            if (screenRenderer == null)
            {
                Debug.LogError("screenRenderer is not assigned!");
                return;
            }
        }

        if (!isTurnedOff)
        {
            Material[] materials = screenRenderer.materials;
            Debug.Log($"Materials array length: {materials.Length}");

            if (materials.Length > 1)
            {
                materials[1] = offMaterial;
                Debug.Log("TV screen material replaced at slot [1]");
            }
            else
            {
                materials[0] = offMaterial;
                Debug.Log("TV screen material replaced at slot [0]");
            }

            screenRenderer.materials = materials;
            isTurnedOff = true;

            // Remove the interaction hint text after turning off
            m_interactableHintText = "";
            m_interactable = false;
        }
    }
    public void Interact()
    {
        ConversationManager.Instance.StartConversation(dialogueObject);
    }
}