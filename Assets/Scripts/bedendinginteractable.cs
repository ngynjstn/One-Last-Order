using UnityEngine;
using TMPro;
using System.Collections;
using DefaultNamespace;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    [Header("Camera Look Positions")]
    [SerializeField] private Vector3 lookSideRotation = new Vector3(0, 30, 0); // Left side look (positive Y value)
    [SerializeField] private Vector3 lookUpRotation = new Vector3(30, 40, 0); // Look up and left

    [Header("Player References")]
    [SerializeField] private GameObject playerObject; // Reference to player GameObject

    [Header("Blinking Vignette")]
    [SerializeField] public Volume globalVolume; // Assign your Global Volume here
    [SerializeField] private float blinkSpeed = 0.5f;
    [SerializeField] private float minVignetteIntensity = 0.3f;
    [SerializeField] private float maxVignetteIntensity = 0.7f;

    [Header("Camera Shake")]
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float shakeDuration = 1f;

    [Header("Camera Zoom")]
    [SerializeField] private float zoomDuration = 2f;
    [SerializeField] private float zoomFOV = 30f; // The target field of view for zoom

    private bool isSleeping = false;
    private Quaternion initialRotation;
    private UnityEngine.Rendering.Universal.Vignette vignette;
    private float targetVignetteIntensity;
    private float originalFOV;
    private MeshRenderer m_meshRenderer;
    private void Start()
    {
        m_meshRenderer = playerObject.GetComponent<MeshRenderer>();
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
            originalFOV = bedCamera.fieldOfView;
            bedCamera.gameObject.SetActive(false);
        }

        // Get the Vignette component from the Global Volume
        if (globalVolume != null && globalVolume.profile.TryGet(out vignette))
        {
            targetVignetteIntensity = maxVignetteIntensity; // Initialize target
            StartCoroutine(BlinkVignette());
        }
        else
        {
            Debug.LogError("Global Volume or Vignette override not found!");
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
            //playerObject.SetActive(false);
            m_meshRenderer.enabled = false; // Disable the mesh renderer to hide the player
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
        if (bedCamera != null)
        {
            bedCamera.transform.rotation = initialRotation;
            bedCamera.gameObject.SetActive(true);
            Debug.Log("Bed camera activated.");

            // Start camera shake when bed camera is active
            StartCoroutine(CameraShake(bedCamera.transform));
        }
        else
        {
            Debug.LogError("Bed Camera is not assigned!");
        }

        if (mainCamera != null)
        {
            mainCamera.gameObject.SetActive(false);
            Debug.Log("Main camera disabled.");
        }
        else
        {
            Debug.LogError("Main Camera is not assigned!");
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

        // 12. Add camera zoom effect
        if (bedCamera != null)
        {
            yield return StartCoroutine(ZoomCamera(bedCamera, zoomFOV, zoomDuration));
        }

        // 13. Wait longer while staring at the antagonist (increased duration)
        yield return new WaitForSeconds(jumpscareStareDuration);

        // 14. Fade to black for game end
        yield return StartCoroutine(FadeTo(1f));

        // 15. End game or load credits
        Debug.Log("Game ending sequence complete");
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

    private IEnumerator BlinkVignette()
    {
        while (true)
        {
            if (vignette != null)
            {
                float timeElapsed = 0f;
                float startIntensity = vignette.intensity.value;
                float endIntensity = targetVignetteIntensity;
                float blinkHalfSpeed = blinkSpeed / 2f;

                while (timeElapsed < blinkHalfSpeed)
                {
                    timeElapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(timeElapsed / blinkHalfSpeed);
                    vignette.intensity.value = Mathf.Lerp(startIntensity, endIntensity, t);
                    yield return null;
                }

                targetVignetteIntensity = (targetVignetteIntensity == maxVignetteIntensity) ? minVignetteIntensity : maxVignetteIntensity;
                yield return new WaitForSeconds(blinkHalfSpeed);
            }
            else
            {
                Debug.LogError("Vignette override is null in BlinkVignette!");
                yield break; // Stop the coroutine if vignette is not found
            }
        }
    }

    private IEnumerator CameraShake(Transform cameraTransform)
    {
        Vector3 originalPosition = cameraTransform.localPosition;
        float timeElapsed = 0f;

        while (timeElapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition = originalPosition + new Vector3(x, y, 0f);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPosition; // Reset to original position
    }

    private IEnumerator ZoomCamera(Camera camera, float targetFOV, float duration)
    {
        float startFOV = camera.fieldOfView;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);
            camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        camera.fieldOfView = targetFOV;
    }
}