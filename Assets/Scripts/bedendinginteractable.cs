using UnityEngine;
using TMPro;
using System.Collections;
using DefaultNamespace;


public class BedInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactableHintText = "Press E to Sleep";
    public string InteractableHintText => m_interactableHintText;
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;


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
    [SerializeField] private float zoomFOV = 30f; // Target field of view for the zoom during black screen
    [SerializeField] private float zoomInDuration = 1f; // Duration of the zoom-in

    [Header("Camera Look Positions")]
    [SerializeField] private Vector3 lookSideRotation = new Vector3(0, 30, 0); // Left side look (positive Y value)
    [SerializeField] private Vector3 lookUpRotation = new Vector3(30, 40, 0); // Look up and left

    [Header("Player References")]
    [SerializeField] private GameObject playerObject; // Reference to player GameObject

    private bool isSleeping = false;
    private Quaternion initialRotation;
    private float originalFOV;

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

        // Get initial FOV of the main camera
        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
        }
        else
        {
            Debug.LogError("Main Camera not assigned to BedInteractable!");
            enabled = false;
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

        // 5. Zoom in the camera during the black screen
        float zoomStartTime = Time.time;
        while (Time.time < zoomStartTime + zoomInDuration)
        {
            float t = (Time.time - zoomStartTime) / zoomInDuration;
            mainCamera.fieldOfView = Mathf.Lerp(originalFOV, zoomFOV, t);
            yield return null;
        }
        mainCamera.fieldOfView = zoomFOV;

        // 6. Wait in darkness (including the zoom duration)
        yield return new WaitForSeconds(blackScreenDuration);

        // 7. Switch cameras and reset bed camera position
        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (bedCamera != null)
        {
            bedCamera.transform.rotation = initialRotation;
            bedCamera.gameObject.SetActive(true);
        }

        // 8. Fade back in to show the player in bed
        yield return StartCoroutine(FadeTo(0f));

        // 9. Short pause to establish the scene
        yield return new WaitForSeconds(1f);

        // 10. Slowly look to the side (window)
        if (bedCamera != null)
        {
            yield return StartCoroutine(RotateCamera(
                bedCamera.transform,
                Quaternion.Euler(lookSideRotation),
                lookSideDuration));
        }

        // 11. Pause briefly to build tension
        yield return new WaitForSeconds(0.8f);

        // 12. Now look up toward where the antagonist will be
        if (bedCamera != null)
        {
            yield return StartCoroutine(RotateCamera(
                bedCamera.transform,
                Quaternion.Euler(lookUpRotation),
                lookUpDuration));
        }

        // 13. Wait longer while staring at the antagonist (increased duration)
        yield return new WaitForSeconds(jumpscareStareDuration);

        // 14. End game or load credits (You can add your end game logic here)
        Debug.Log("Jumpscare sequence complete");
        WindowInteractable.isFadeInUse = false; // Release the fade lock if not ending game immediately
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