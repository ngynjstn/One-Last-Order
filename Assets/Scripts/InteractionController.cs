using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] public float m_interactDistance = 5f;
    [SerializeField] public TextMeshProUGUI m_uiHintTextElement;
    [SerializeField] public PlayerInput m_playerInput;
    [SerializeField] public Camera m_playerCamera;

    IInteractable currentInteractableInReticle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractableInReticle.Interact();
                }
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
