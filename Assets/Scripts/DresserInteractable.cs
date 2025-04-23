using UnityEngine;
using TMPro;
using System.Collections;
using DefaultNamespace;
using DialogueEditor;

public class DresserInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to change into work clothes";
    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;
    public NPCConversation dialogueObject;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    //private float fadeDuration = 1f;
    [SerializeField] private Animator fadeAnimator;

    [Header("UI References")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private TextMeshProUGUI clothesMessageText;

    [Header("Message Settings")]
    [SerializeField] private float messageDisplayDuration = 2f;
    [SerializeField] private string clothesChangedMessage = "You have changed into your work clothes";

    private bool hasChangedClothes = false;

    private void Start()
    {
        // Ensure everything is hidden at the start
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }

        if (clothesMessageText != null)
        {
            clothesMessageText.gameObject.SetActive(false);
        }
    }

    // Implementing the Interact method from IInteractable interface
    public void Interact()
    {
        //if (!hasChangedClothes)
        //{
        //    hasChangedClothes = true;
        //    StartCoroutine(ChangeClothesSequence());
        //}
        // Start the conversation with the NPC
        ConversationManager.Instance.StartConversation(dialogueObject);
    }
    public void ChangeIntoClothes()
    {
        if (!hasChangedClothes)
        {
            hasChangedClothes = true;
            StartCoroutine(ChangeClothesSequence());
        }
    }

    private IEnumerator ChangeClothesSequence()
    {
        Debug.Log("Changing clothes...");

        // 1. Fade to black smoothly
        //yield return StartCoroutine(FadeTo(1f));
        fadeAnimator.SetTrigger("Fade_Out");

        // 2. Short pause while black
        yield return new WaitForSeconds(2f);

        // 3. Fade back in
        //yield return StartCoroutine(FadeTo(0f));
        fadeAnimator.SetTrigger("Fade_In");

        // 4. Show message after fade in
        clothesMessageText.gameObject.SetActive(true);
        clothesMessageText.text = clothesChangedMessage;

        // 5. Display message for specified duration
        yield return new WaitForSeconds(messageDisplayDuration);

        // 6. Hide message
        clothesMessageText.gameObject.SetActive(false);

        // 7. Clear the interaction hint text to hide the prompt
        // This is the same approach used in TVInteractable
        m_interactableHintText = "";
        m_interactable = false; // Disable interaction to prevent multiple triggers

        Debug.Log("Clothes changed complete.");
    }

    //private IEnumerator FadeTo(float targetAlpha)
    //{
    //    float startAlpha = fadeCanvasGroup.alpha;
    //    float time = 0;

    //    while (time < fadeDuration)
    //    {
    //        time += Time.deltaTime;
    //        float t = time / fadeDuration;
    //        fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

    //        // Enable blocking raycasts when fading in, disable when fading out
    //        fadeCanvasGroup.blocksRaycasts = fadeCanvasGroup.alpha > 0.5f;

    //        yield return null;
    //    }

    //    // Ensure we reach exact target value
    //    fadeCanvasGroup.alpha = targetAlpha;
    //    fadeCanvasGroup.blocksRaycasts = targetAlpha > 0.5f;
    //}
}