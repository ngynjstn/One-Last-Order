using DefaultNamespace;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MilkInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;
    [SerializeField] private Transform handTransform;
    public CupManager cupManager;
    Vector3 originalPos;
    Quaternion originalRot;
    public PickupCup pickupCup;
    public void ResetMilkState()
    {
        m_interactable = true;
        Debug.Log("Milk state reset to: " + m_interactable);
    }

    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Interacted with " + gameObject.name);
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        //GameObject obj = interactionController.;
        if (obj != null)
        {
            //cupManager = obj.GetComponent<CupManager>();
            cupManager = interactionController.GetComponent<CupManager>();
            if (cupManager.cupContents.Contains("lid"))
            {
                Debug.LogWarning("Cannot pour milk with a lid on!");
                return;
            }
        }

        // Check if frothing is happening
        if (FrotherInteractable.IsFrothing)
        {
            Debug.LogWarning("Cannot make coffee while frothing!");
            return;
        }

        GameObject access = CupInteractable.curr;

        if (access != null)
        {
            Debug.Log("Pouring milk.");
            MoveCarton();
            StartPouring();
            m_interactable = false;
        }
        else
        {
            Debug.LogWarning("No cup in hand.");
        }
    }
    private GameObject carton;
    private void MoveCarton()
    {
        
        carton = GameObject.FindGameObjectWithTag("Carton");
        carton.transform.SetParent(handTransform);
        carton.transform.localPosition = Vector3.zero;
        carton.transform.localRotation = Quaternion.identity;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        pickupCup = obj.GetComponent<PickupCup>();
        pickupCup.AddContent("milk");
        carton.transform.SetParent(null);
        carton.transform.position = originalPos;
        carton.transform.rotation = originalRot;
        Debug.Log("Ready to froth.");
    }
    public void StartPouring()
    {
        AudioSource milkSound = GetComponent<AudioSource>();
        milkSound.time = 2.2f;
        milkSound.Play();
        StartCoroutine(EnablePickupAfterDelay(6));
    }
    void Start()
    {
        carton = GameObject.FindGameObjectWithTag("Carton");
        originalPos = carton.transform.position;
        originalRot = carton.transform.rotation;
    }
}