using UnityEngine;
using TMPro; // Required for TextMesh Pro
using System.Collections;
using DialogueEditor;
using DefaultNamespace;
using System;

public class LightToggleInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to Toggle Light"; // Updated hint text
    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;
    //public NPCConversation dialogueObject; // Remains, assuming you still want dialogue

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Animator fadeAnimator;

    [Header("UI References")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private TextMeshProUGUI lightMessageText; // Renamed

    [Header("Message Settings")]
    //[SerializeField] private float messageDisplayDuration = 2f;
    [SerializeField] private string lightToggledOnMessage = "Light Turned On";   // Updated messages
    [SerializeField] private string lightToggledOffMessage = "Light Turned Off";  // Updated messages

    [Header("Light")]
    [SerializeField] private Light targetLight; // Assign the light you want to toggle here

    public Action LightToggledOn; // Event to notify when the light is toggled
    public Action LightToggledOff; // Event to notify when the light is toggled
    private bool isLightOn = false;

    private bool hasToggledLight = false; // Added to prevent repeated toggles if needed

    private void Start()
    {
        // Ensure everything is hidden at the start
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }

        if (lightMessageText != null) // Renamed
        {
            lightMessageText.gameObject.SetActive(false);
        }
        if (targetLight == null)
        {
            Debug.LogError("Target Light not assigned on " + gameObject.name);
            enabled = false;
        }
    }

    // Implementing the Interact method from IInteractable interface
    public void Interact(InteractionController interactionController)
    {
        // Start the conversation with the NPC.  This part remains as is.
        //ConversationManager.Instance.StartConversation(dialogueObject);
        ToggleLight(); //call the toggle light function.
    }

    public void ToggleLight() // Renamed and repurposed
    {
        if (!hasToggledLight) // Removed the hasChangedClothes check.  Added a hasToggledLight, if you want to only allow it to happen once.
        {
            hasToggledLight = true; //set to true, so it only happens once.  Remove this line if you want the light to toggle every time.
            StartCoroutine(LightToggleSequence()); // Renamed coroutine
        }

    }

    private IEnumerator LightToggleSequence() // Renamed coroutine
    {
        Debug.Log("Toggling light..."); // Updated debug message

        // 1. Fade to black smoothly
        //fadeAnimator.SetTrigger("Fade_Out");

        // 2. Short pause while black
        yield return new WaitForSeconds(2f);

        // 3. Toggle the light
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled; // Toggle the light's state
            isLightOn = targetLight.enabled;
            if (isLightOn)
            {
                LightToggledOn?.Invoke(); // Invoke the event to notify subscribers
                Debug.Log("Light turned on.");
            }
            else
            {
                LightToggledOff?.Invoke(); // Invoke the event to notify subscribers
                Debug.Log("Light turned off.");
            }

            // 4. Fade back in
            //fadeAnimator.SetTrigger("Fade_In");

            // 5. Show message after fade in
            //lightMessageText.gameObject.SetActive(true);
            //lightMessageText.text = isLightOn ? lightToggledOnMessage : lightToggledOffMessage; // Use the correct message

            // 6. Display message for specified duration
            // yield return new WaitForSeconds(messageDisplayDuration);

            // 7. Hide message
            //lightMessageText.gameObject.SetActive(false);

            // 8. Clear the interaction hint text and disable interaction.  Keep this, assuming you want this behavior.
            m_interactableHintText = "";
            //m_interactable = false; // Disable interaction to prevent multiple triggers
            Debug.Log("Light toggle complete."); //Updated debug message
        }
    }
}