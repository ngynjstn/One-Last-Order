using DefaultNamespace;
using DialogueEditor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] public float m_interactDistance = 5f;
    [SerializeField] public TextMeshProUGUI m_uiHintTextElement;
    private PlayerInput m_playerInput;
    [SerializeField] public Camera m_playerCamera;

    IInteractable currentInteractableInReticle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
    private void ConversationStart()
    {
        m_uiHintTextElement.gameObject.SetActive(false);
    }
    private void ConversationEnd()
    {
        m_uiHintTextElement.gameObject.SetActive(false);
    }
    public void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ConversationManager.Instance.IsConversationActive)
        {
            m_uiHintTextElement.gameObject.SetActive(false);
            return;
        }
        CheckRaycastForInteractable();
        CheckInteractKeyPressed();
        
    }
    void CheckRaycastForInteractable()
    {
        RaycastHit hit;
        var raycastCamera = m_playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(raycastCamera, out hit, m_interactDistance))
        {
            IInteractable interactable = hit.collider?.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractableInReticle = interactable;
                m_uiHintTextElement.text = currentInteractableInReticle.InteractableHintText;
                m_uiHintTextElement.gameObject.SetActive(true);
            }
            else
            {
                currentInteractableInReticle = null;
                m_uiHintTextElement.gameObject.SetActive(false);
            }
        }
        else
        {
            currentInteractableInReticle = null;
            m_uiHintTextElement.gameObject.SetActive(false);
        }
}
    void CheckInteractKeyPressed()
    {
        if (currentInteractableInReticle != null && m_playerInput.actions["Interact"].triggered)
        {
            currentInteractableInReticle.Interact();
        }
    }
}
