using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class CupInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    [SerializeField] public string InteractableHintText => m_interactableHintText;
    // Set to true to continuous make drinks
    [SerializeField] private bool m_interactable = true;
    [SerializeField] public bool IsInteractable => m_interactable;
    [SerializeField] private GameObject cup;
    [SerializeField] private Transform handTransform;
    [SerializeField] private GameObject currentCup;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] public static GameObject curr;

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        if (cup != null && handTransform != null)
        {
            curr = SpawnCup();
            Debug.Log(curr);
            m_interactable = false; // Optional: prevent multiple spawns
        }
    }
    private GameObject SpawnCup()
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
        return currentCup;
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
