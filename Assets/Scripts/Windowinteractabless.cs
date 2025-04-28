using UnityEngine;
using System.Collections;
using DefaultNamespace;
using DialogueEditor;

public class WindowInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to close window";
    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [Header("Window Properties")]
    [SerializeField] private Renderer windowGlassRenderer;
    [SerializeField] private Material openWindowGlassMaterial;
    [SerializeField] private Material closedWindowGlassMaterial;
    [SerializeField] private bool isWindowOpen = true;

    [Header("Fade Settings")]
    //[SerializeField] private float fadeDuration = 1f;
    //[SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Animator fadeAnimator;



    [Header("Dialogue")]
    [SerializeField] private NPCConversation dialogueObject;

    // New fields for sound
    [SerializeField] private AudioSource audioSource;  // AudioSource to play sound
    [SerializeField] private AudioClip interactionSound;  // Sound to play on interaction

    public static bool isFadeInUse = false;
    private bool isInteracting = false;

    private void Start()
    {
        // Ensure fade canvas is hidden at the start
        //if (fadeCanvasGroup != null)
        //{
        //    fadeCanvasGroup.alpha = 0f;
        //    fadeCanvasGroup.blocksRaycasts = false;
        //}

        // Set initial window state
        UpdateWindowAppearance();
    }

    public void Interact(InteractionController interactionController)
    {
        // Prevent interaction if already interacting or fade is in use
        if (isInteracting || isFadeInUse)
        {
            Debug.Log("Cannot interact with window - fade already in use");
            return;
        }

        StartCoroutine(ToggleWindowSequence());

        PlayInteractionSound();

        //ConversationManager.Instance.StartConversation(dialogueObject);
    }

    private void PlayInteractionSound()
    {
        if (audioSource != null && interactionSound != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or Interaction Sound is not assigned!");
        }
    }
    public void ActivateWindow()
    {
        // Prevent interaction if already interacting or fade is in use
        if (isInteracting || isFadeInUse)
        {
            Debug.Log("Cannot interact with window - fade already in use");
            return;
        }

        StartCoroutine(ToggleWindowSequence());
    }
    private IEnumerator ToggleWindowSequence()
    {
        isInteracting = true;
        isFadeInUse = true;

        // Clear the prompt immediately, before the fade even starts
        m_interactableHintText = "";
        m_interactable = false; // Disable interaction to prevent multiple triggers

        Debug.Log("Toggling window state...");

        // Fade to black
        //yield return StartCoroutine(FadeTo(1f));
        fadeAnimator.SetTrigger("Fade_Out");
        // Short pause while black
        yield return new WaitForSeconds(2f);

        // Toggle window state
        isWindowOpen = !isWindowOpen;
        UpdateWindowAppearance();

        // Update the prompt text for next interaction
        if (isWindowOpen)
        {
            m_interactableHintText = "Press E to close window";
        }
        else
        {
            m_interactableHintText = "Press E to open window";
        }

        // Fade back in
        //yield return StartCoroutine(FadeTo(0f));
        fadeAnimator.SetTrigger("Fade_In");

        // Release the interaction locks
        isInteracting = false;
        isFadeInUse = false;
    }

    private void UpdateWindowAppearance()
    {
        if (windowGlassRenderer != null)
        {
            Material materialToUse = isWindowOpen ? openWindowGlassMaterial : closedWindowGlassMaterial;

            if (materialToUse != null)
            {
                // Get all materials from the renderer
                Material[] materials = windowGlassRenderer.materials;

                // Change the material (assuming glass is at index 0)
                if (materials.Length > 0)
                {
                    materials[0] = materialToUse;
                    windowGlassRenderer.materials = materials;
                }
                else
                {
                    Debug.LogError("Window glass renderer has no materials!");
                }
            }
            else
            {
                Debug.LogError("Window glass material not assigned!");
            }
        }
        else
        {
            Debug.LogError("Window glass renderer not assigned!");
        }
    }

    //private IEnumerator FadeTo(float targetAlpha)
    //{
    //    //if (fadeCanvasGroup == null)
    //    //{
    //    //    Debug.LogError("Fade canvas group not assigned!");
    //    //    yield break;
    //    //}

    //    // Make sure target alpha is properly clamped
    //    targetAlpha = Mathf.Clamp01(targetAlpha);
    //    float startAlpha = fadeCanvasGroup.alpha;
    //    float elapsedTime = 0;

    //    // Enable the canvas group gameObject if it was disabled
    //    fadeCanvasGroup.gameObject.SetActive(true);

    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float normalizedTime = Mathf.Clamp01(elapsedTime / fadeDuration);
    //        fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
    //        fadeCanvasGroup.blocksRaycasts = fadeCanvasGroup.alpha > 0.5f;
    //        yield return null;
    //    }

    //    // Ensure we reach exact target value
    //    fadeCanvasGroup.alpha = targetAlpha;
    //    fadeCanvasGroup.blocksRaycasts = targetAlpha > 0.5f;

    //    // If we're completely transparent, we can disable the gameObject to save resources
    //    if (targetAlpha <= 0)
    //    {
    //        fadeCanvasGroup.gameObject.SetActive(false);
    //    }
    //}
}