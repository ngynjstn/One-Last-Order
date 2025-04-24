using UnityEngine;
using DefaultNamespace;
public class FrotherInteractable : MonoBehaviour
{    
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private Transform placeHolder;
    [SerializeField] private GameObject cup;
    public bool isPlaced = false;
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        GameObject access = CupInteractable.curr;

        if (access != null && !isPlaced)
        {
            PlaceCup(access);
            isPlaced = true;

            Debug.Log("Cup placed: " + access.name);
            StartFrothing();
            m_interactable = false;
        }
        else
        {
            Debug.LogWarning("No cup to place or cup already placed.");
        }
    }
    private void PlaceCup(GameObject cup)
    {
        // Move the existing cup to the machine placeholder
        cup.transform.SetParent(placeHolder);
        cup.transform.position = placeHolder.position;
        cup.transform.rotation = placeHolder.rotation;

        // Align properly
        cup.transform.localPosition = Vector3.zero;
        cup.transform.localRotation = Quaternion.identity;

        Rigidbody rb = GetComponent<Rigidbody>();
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
    /*
    public ParticleSystem CoffeeStream;
    public float hSliderValue = 0.0F;
    */
    public void StartFrothing()
    {
        Debug.Log("Frothing.");
        /*
        AudioSource coffeeSound = GetComponent<AudioSource>();
        coffeeSound.time = 2.2f;
        coffeeSound.Play();
        var main = CoffeeStream.main;
        main.startDelay = hSliderValue;
        CoffeeStream.Play(true);
        */
    }
}
