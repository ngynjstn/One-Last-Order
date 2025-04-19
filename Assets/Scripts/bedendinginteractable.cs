using UnityEngine;
using TMPro;
using System.Collections;
using DefaultNamespace;

public class BedInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to Sleep";
    public string InteractableHintText => m_interactableHintText;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("UI References")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Header("Jumpscare Settings")]
    [SerializeField] private GameObject jumpscareObject; // The antagonist
    [SerializeField] private Camera mainCamera; // Regular gameplay camera
    [SerializeField] private Camera bedCamera; // Camera positioned on the bed
    [SerializeField] private AudioSource scareSound; // Jumpscare sound
    [SerializeField] private float blackScreenDuration = 2f; // How long to stay black
    [SerializeField] private float lookSideDuration = 2f; // How long to look to the side
    [SerializeField] private float lookUpDuration = 1.5f; // How long to look up at antagonist
    [SerializeField] private float jumpscareStareDuration = 5f; // How long to stare at antagonist

    [Header("Camera Look Positions")]
    [SerializeField] private Vector3 lookSideRotation = new Vector3(0, 30, 0); // Left side look (positive Y value)
    [SerializeField] private Vector3 lookUpRotation = new Vector3(30, 30, 0); // Look up and left

    [Header("Player References")]
    [SerializeField] private GameObject playerObject; // Reference to player GameObject

    private bool isSleeping = false;
    private Quaternion initialRotation;

    private void Start()
    {
        // Ensure everything is properly set up at start
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }

        // Make sure jumpscare objects are inactive
        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }

        // Make sure the bed camera is disabled at start
        if (bedCamera != null)
        {
            initialRotation = bedCamera.transform.rotation;
            bedCamera.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        // Check if another interactable is using the fade
        if (WindowInteractable.isFadeInUse)
        {
            Debug.Log("Cannot interact with bed - fade already in use");
            return;
        }

        if (!isSleeping)
        {
            isSleeping = true;
            m_interactableHintText = ""; // Hide prompt
            WindowInteractable.isFadeInUse = true; // Claim the fade
            StartCoroutine(SleepAndJumpscareSequence());
        }
    }

    private IEnumerator SleepAndJumpscareSequence()
    {
        Debug.Log("Going to sleep...");

        // 1. Fade to black
        yield return StartCoroutine(FadeTo(1f));

        // 2. Hide player character
        if (playerObject != null)
        {
            playerObject.SetActive(false);
        }

        // 3. Activate the antagonist during the black screen
        if (jumpscareObject != null)
            jumpscareObject.SetActive(true);

        // 4. Play the scare sound earlier during black screen
        if (scareSound != null)
            scareSound.Play();

        // 5. Wait in darkness
        yield return new WaitForSeconds(blackScreenDuration);

        // 6. Switch cameras and reset bed camera position
        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (bedCamera != null)
        {
            bedCamera.transform.rotation = initialRotation;
            bedCamera.gameObject.SetActive(true);
        }

        // 7. Fade back in to show the player in bed
        yield return StartCoroutine(FadeTo(0f));

        // 8. Short pause to establish the scene
        yield return new WaitForSeconds(1f);

        // 9. Slowly look to the side (window)
        if (bedCamera != null)
        {
            yield return StartCoroutine(RotateCamera(
                bedCamera.transform,
                Quaternion.Euler(lookSideRotation),
                lookSideDuration));
        }

        // 10. Pause briefly to build tension
        yield return new WaitForSeconds(0.8f);

        // 11. Now look up toward where the antagonist will be
        if (bedCamera != null)
        {
            yield return StartCoroutine(RotateCamera(
                bedCamera.transform,
                Quaternion.Euler(lookUpRotation),
                lookUpDuration));
        }

        // 12. Wait longer while staring at the antagonist (increased duration)
        yield return new WaitForSeconds(jumpscareStareDuration);

        // 13. Fade to black for game end
        yield return StartCoroutine(FadeTo(1f));

        // 14. End game or load credits
        Debug.Log("Game ending sequence complete");
        // We don't release the fade lock since this is the end of the game
        // SceneManager.LoadScene("Credits");
        // Or: Application.Quit();
    }

    private IEnumerator RotateCamera(Transform cameraTransform, Quaternion targetRotation, float duration)
    {
        float timeElapsed = 0;
        Quaternion startRotation = cameraTransform.rotation;

        while (timeElapsed < duration)
        {
            cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.rotation = targetRotation;
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeCanvasGroup.blocksRaycasts = fadeCanvasGroup.alpha > 0.5f;
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
        fadeCanvasGroup.blocksRaycasts = targetAlpha > 0.5f;
    }
}
