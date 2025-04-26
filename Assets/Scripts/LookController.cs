using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LookController : MonoBehaviour
{
    [Header("Look Target")]
    [SerializeField] private Transform lookTarget;  // The position to look at
    [SerializeField] private float lookDuration = 2f;  // How long the look takes
    [SerializeField] private float rotationSpeed = 2f;  // Rotation speed multiplier

    [Header("Player Reference")]
    [SerializeField] private Transform playerCamera;  // Assign the player's camera transform

    private bool hasTriggered = false;
    private Quaternion initialRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            initialRotation = playerCamera.rotation;
            StartCoroutine(RotateToLook());
        }
    }

    private IEnumerator RotateToLook()
    {
        // Add at the start of RotateToLook():
        PlayerInput playerInput = playerCamera.GetComponentInParent<PlayerInput>();
        playerInput.enabled = false;

        // Add at the end of RotateToLook():
        playerInput.enabled = true;

        float elapsedTime = 0f;
        Vector3 directionToTarget = lookTarget.position - playerCamera.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        while (elapsedTime < lookDuration)
        {
            playerCamera.rotation = Quaternion.Slerp(
                initialRotation,
                targetRotation,
                (elapsedTime / lookDuration) * rotationSpeed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure perfect alignment at the end
        playerCamera.rotation = targetRotation;
    }

    private void OnDrawGizmos()
    {
        if (lookTarget != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, lookTarget.position);
            Gizmos.DrawWireSphere(lookTarget.position, 0.25f);
        }
    }
}