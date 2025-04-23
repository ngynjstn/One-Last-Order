using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class CoffeeInteractable : MonoBehaviour, IInteractable
{
    GameObject machine1;
    

    
    // public AudioSource coffeeSound;

    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private Transform placeHolder;
    [SerializeField] private GameObject cup;

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        PlaceCup(cup);
        StartPouring();
    }
    private void PlaceCup(GameObject cup)
    {
        cup = Instantiate(cup, placeHolder.position, placeHolder.rotation);
        cup.transform.SetParent(placeHolder);
        cup.transform.localPosition = Vector3.zero;
        cup.transform.localRotation = Quaternion.identity;

        // Optional cleanup
        Collider col = cup.GetComponent<Collider>();
        if (col) col.enabled = false;

        Rigidbody rb = cup.GetComponent<Rigidbody>();
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
    public ParticleSystem CoffeeStream;
    public void StartPouring()
    {
        CoffeeStream.Play(true);
    }
}
