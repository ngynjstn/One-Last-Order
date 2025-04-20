using DialogueEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public float gravity = -9.81f;
    private PlayerInput m_playerInput;
    private CharacterController controller;
    private Vector3 velocity;
    private float verticalRotation = 0f;

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
        m_playerInput.SwitchCurrentActionMap("UI"); // Switch to UI action map
        Cursor.lockState = CursorLockMode.None; // Show mouse
    }
    private void ConversationEnd()
    {
        m_playerInput.SwitchCurrentActionMap("Player"); // Switch back to Player action map
        Cursor.lockState = CursorLockMode.Locked; // Hide mouse
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        m_playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked; // Hide mouse
    }
    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!ConversationManager.Instance.IsConversationActive)
            return;

        Vector2 navigation = context.ReadValue<Vector2>();

        if (context.performed)
        {
            if (navigation.y > 0.5f)
                ConversationManager.Instance.SelectPreviousOption();
            else if (navigation.y < -0.5f)
                ConversationManager.Instance.SelectNextOption();
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!ConversationManager.Instance.IsConversationActive)
            return;

        if (context.performed)
        {
            ConversationManager.Instance.PressSelectedOption();
        }
    }

    void Update()
    {
        var move = m_playerInput.actions["move"].ReadValue<Vector2>();
        var look = m_playerInput.actions["look"].ReadValue<Vector2>();
        Move(move);
        Look(look);
    }


    void Move(Vector2 moveInput)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity manually
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -2f; // small downward force to keep grounded
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void Look(Vector2 lookInput)
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
