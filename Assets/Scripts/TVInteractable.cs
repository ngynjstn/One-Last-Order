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

    [Header("TV Sound")]
    [SerializeField] private AudioSource tvSoundSource; // Assign the AudioSource here
    [SerializeField] private AudioClip tvSoundClip;    // Assign the TV sound clip here

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

        // Get or create the AudioSource
        if (tvSoundSource == null)
        {
            tvSoundSource = GetComponent<AudioSource>();
            if (tvSoundSource == null)
            {
                tvSoundSource = gameObject.AddComponent<AudioSource>();
                Debug.Log("Added AudioSource component to TVInteractable");
            }
        }

        // Set up the AudioSource and play the sound
        if (tvSoundSource != null && tvSoundClip != null)
        {
            tvSoundSource.clip = tvSoundClip;
            tvSoundSource.loop = true; // Make the sound loop continuously
            tvSoundSource.playOnAwake = true;
            tvSoundSource.Play(); // Start playing the sound
        }
        else if (tvSoundSource == null)
        {
            Debug.LogError("No AudioSource component found or created for the TV sound.");
        }
        else if (tvSoundClip == null)
        {
            Debug.LogError("No AudioClip assigned to tvSoundSource on " + gameObject.name);
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

            // Stop the TV sound when turned off
            if (tvSoundSource != null)
            {
                tvSoundSource.Stop();
            }

            // Remove the interaction hint text after turning off
            m_interactableHintText = "";
            m_interactable = false;
        }
    }

    public void Interact(InteractionController interactionController)
    {
        ConversationManager.Instance.StartConversation(dialogueObject);
    }
}
