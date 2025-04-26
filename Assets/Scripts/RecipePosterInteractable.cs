using DefaultNamespace;
using DialogueEditor;
using UnityEngine;

public class RecipePosterInteractable : MonoBehaviour, IInteractable
{
    public string m_interactableHintText = "Press E to confirm recipes";
    public string InteractableHintText => m_interactableHintText;
    public bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    public NPCConversation m_conversation;
    public void Interact()
    {
        ConversationManager.Instance.StartConversation(m_conversation);
        m_interactable = false;
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
