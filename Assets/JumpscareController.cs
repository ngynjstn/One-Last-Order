using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JumpscareController : MonoBehaviour
{
    public Image jumpscareImage;
    public AudioClip jumpscareClip1;
    public AudioSource jumpsource;
    public GameObject homelessManPrefab; // Assign the homeless man prefab in the Inspector
    public Transform spawnPoint;       // Assign the spawn point transform in the Inspector

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.gameObject.CompareTag("JumpScare"))
        {
            hasTriggered = true;
            jumpscareImage.enabled = true;
            jumpsource.PlayOneShot(jumpscareClip1);
            StartCoroutine(SpawnHomelessMan());
            StartCoroutine(DisableJumpscareObjects()); // Keep this to disable the jumpscare figures
        }
    }

    private IEnumerator SpawnHomelessMan()
    {
        yield return new WaitForSeconds(2); // Wait the same duration as the jumpscare display

        if (homelessManPrefab != null && spawnPoint != null)
        {
            GameObject spawnedHomelessMan = Instantiate(homelessManPrefab, spawnPoint.position, spawnPoint.rotation);
            // You might want to add further setup for the spawned character here,
            // like getting a reference to their dialogue script, etc.
            Debug.Log("Homeless man spawned!");
        }
        else
        {
            Debug.LogError("Homeless Man Prefab or Spawn Point not assigned in the Inspector!");
        }
    }

    private IEnumerator DisableJumpscareObjects()
    {
        yield return new WaitForSeconds(2);
        jumpscareImage.enabled = false;

        GameObject[] jumpscareObjects = GameObject.FindGameObjectsWithTag("JumpScare");
        foreach (GameObject obj in jumpscareObjects)
        {
            obj.SetActive(false);
        }

        // Optionally disable this trigger
        // gameObject.SetActive(false);
    }
}