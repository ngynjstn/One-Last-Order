using UnityEngine;
using UnityEngine.UI;
using DefaultNamespace;
using System.Collections;

public class DresserInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to change into work clothes";
    public string InteractableHintText => m_interactableHintText;

    [SerializeField] private CanvasGroup fadePanel; // Black panel for fading
    [SerializeField] private Text messageText; // Message text component
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float blackScreenDuration = 1.0f;
    [SerializeField] private float messageDisplayTime = 3.0f;

    private bool isDressed = false;
    private bool isFading = false;

    public bool IsDressed => isDressed;

    private void Start()
    {
        // Ensure panel is invisible at start
        if (fadePanel != null)
        {
            fadePanel.alpha = 0;
        }

        // Ensure message is hidden at start
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        Debug.Log("DresserInteractable.Interact() was called.");

        if (!isDressed && !isFading)
        {
            StartCoroutine(ChangeClothes());
        }
    }

    private IEnumerator ChangeClothes()
    {
        isFading = true;

        // Make sure fade panel is active
        fadePanel.gameObject.SetActive(true);

        // Fade to black (completely black)
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(timer / fadeDuration); // Ensure it goes to full 1.0
            yield return null;
        }

        // Force to completely black (just to be safe)
        fadePanel.alpha = 1.0f;

        // Hold black screen
        yield return new WaitForSeconds(blackScreenDuration);

        // Prepare the message text
        if (messageText != null)
        {
            messageText.text = "You have changed into work clothes";
            messageText.gameObject.SetActive(true);
        }

        // Start fading back in
        timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        // Force to completely transparent (just to be safe)
        fadePanel.alpha = 0.0f;

        // Keep message visible for specified time
        yield return new WaitForSeconds(messageDisplayTime);

        // Hide the message
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }

        isDressed = true;
        m_interactableHintText = ""; // Remove the interaction hint text
        isFading = false;
    }
}