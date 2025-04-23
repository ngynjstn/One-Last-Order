using UnityEngine;
using TMPro;
using System.Collections;
using DefaultNamespace;
using UnityEngine.SceneManagement;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to Leave";

    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;


    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Animator fadeAnimator;
    [Header("UI References")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    private void Start()
    {
        // Ensure fade canvas is hidden at the start
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void Interact()
    {
        StartCoroutine(LeaveRoomSequence());
    }

    private IEnumerator LeaveRoomSequence()
    {
        Debug.Log("Leaving room...");

        // Fade to black
        //yield return StartCoroutine(FadeTo(1f));
        fadeAnimator.SetTrigger("Fade_Out");
        yield return new WaitForSeconds(1f);

        // Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name not specified!");

            // If no scene name is provided, fade back in so the player isn't stuck
            //yield return StartCoroutine(FadeTo(0f));
            fadeAnimator.SetTrigger("Fade_In");
            yield return new WaitForSeconds(1f);
        }
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