using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class CupInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;
    [SerializeField] private GameObject cup;
    [SerializeField] private Transform handTransform;

    private GameObject currentCup;

    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        /*float pickUpDistance = 2f;
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance))
        {
            Debug.Log(raycastHit.transform);
        }*/
        if (cup != null && handTransform != null)
        {
            SpawnCup();
            m_interactable = false; // Optional: prevent multiple spawns
        }
    }
    private void SpawnCup()
    {
        currentCup = Instantiate(cup, handTransform.position, handTransform.rotation);
        currentCup.transform.SetParent(handTransform);
        currentCup.transform.localPosition = Vector3.zero;
        currentCup.transform.localRotation = Quaternion.identity;

        // Optional cleanup
        Collider col = currentCup.GetComponent<Collider>();
        if (col) col.enabled = false;

        Rigidbody rb = currentCup.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
