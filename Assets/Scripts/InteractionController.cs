using DefaultNamespace;
using DialogueEditor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float m_interactDistance = 5f;
    [SerializeField] private TextMeshProUGUI m_uiHintTextElement;
    [SerializeField] private Camera m_playerCamera;
    private PlayerInput m_playerInput;
    private IInteractable currentInteractableInReticle;

    private void Awake() => m_playerInput = GetComponent<PlayerInput>();

    private void Update()
    {
        if (ConversationManager.Instance.IsConversationActive)
        {
            return;
        }

        UpdateInteractionState();
    }

    private void UpdateInteractionState()
    {
        var interactable = GetInteractableInView();
        UpdateInteractionUI(interactable);
        HandleInteraction(interactable);
    }

    private IInteractable GetInteractableInView()
    {
        var raycastCamera = m_playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        return Physics.Raycast(raycastCamera, out RaycastHit hit, m_interactDistance)
            ? hit.collider?.GetComponent<IInteractable>()
            : null;
    }

    private void UpdateInteractionUI(IInteractable interactable)
    {
        bool isValidInteractable = interactable != null && interactable.IsInteractable;

        currentInteractableInReticle = isValidInteractable ? interactable : null;
        m_uiHintTextElement.gameObject.SetActive(isValidInteractable);

        if (isValidInteractable)
        {
            m_uiHintTextElement.text = interactable.InteractableHintText;
        }
    }

    private void HandleInteraction(IInteractable interactable)
    {
        if (interactable != null &&
            interactable.IsInteractable &&
            m_playerInput.actions["Interact"].triggered)
        {
            interactable.Interact(this);
        }
    }

    private void HideInteractionUI()
    {
        currentInteractableInReticle = null;
        m_uiHintTextElement.gameObject.SetActive(false);
    }

    // Event handlers
    private void OnEnable()
    {
        ConversationManager.OnConversationStarted += ConversationStart;
        ConversationManager.OnConversationEnded += ConversationEnd;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= ConversationStart;
        ConversationManager.OnConversationEnded -= ConversationEnd;
    }

    private void ConversationStart() => HideInteractionUI();
    private void ConversationEnd() => HideInteractionUI();
}
