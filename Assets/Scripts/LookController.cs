using UnityEngine;

public class TeleportAndLook : MonoBehaviour
{
    [Tooltip("The Transform of the object where the player will be teleported.")]
    public Transform teleportTarget;

    [Tooltip("The Transform of the object the camera will look at after teleporting.")]
    public Transform lookAtTarget;

    [Tooltip("The specific Camera GameObject to control. If left empty, it will try to find the MainCamera.")]
    public GameObject cameraToControl;

    [Tooltip("The tag of the player GameObject.")]
    public string playerTag = "Look";

    [Tooltip("Should this script disable itself after triggering once?")]
    public bool disableAfterTrigger = true;

    private Camera mainCameraComponent; // To store the Camera component
    private bool hasTriggered = false; // Flag to track if the teleport has happened

    void Start()
    {
        // Get the Camera component. Prioritize the assigned one, then try to find the MainCamera.
        if (cameraToControl != null)
        {
            mainCameraComponent = cameraToControl.GetComponent<Camera>();
            if (mainCameraComponent == null)
            {
                Debug.LogError("The assigned 'Camera To Control' GameObject does not have a Camera component on " + gameObject.name);
                enabled = false;
                return;
            }
        }
        else
        {
            Camera tempCamera = Camera.main;
            if (tempCamera != null)
            {
                mainCameraComponent = tempCamera;
            }
            else
            {
                Debug.LogError("No MainCamera found in the scene and no Camera assigned in the Inspector on " + gameObject.name);
                enabled = false;
                return;
            }
        }

        // Basic error checking for the teleport target.
        if (teleportTarget == null)
        {
            Debug.LogError("Teleport Target is not assigned on " + gameObject.name);
            enabled = false; // Disable the script if the target is missing.
        }

        if (lookAtTarget == null)
        {
            Debug.LogWarning("Look At Target is not assigned on " + gameObject.name + ". Camera will not be locked.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the specified player tag and if it hasn't triggered yet.
        if (other.CompareTag(playerTag) && !hasTriggered)
        {
            // Teleport the player to the target position.
            other.transform.position = teleportTarget.position;

            // If a lookAtTarget is assigned and we have a valid camera, make the camera look at it.
            if (lookAtTarget != null && mainCameraComponent != null)
            {
                mainCameraComponent.transform.LookAt(lookAtTarget);
            }

            // Set the flag to indicate that the trigger has occurred.
            hasTriggered = true;

            // Disable the script if the option is enabled.
            if (disableAfterTrigger)
            {
                enabled = false;
            }
        }
    }
}