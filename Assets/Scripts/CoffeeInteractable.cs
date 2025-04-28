using DefaultNamespace;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CoffeeInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private Transform placeHolder;
    [SerializeField] private GameObject cup;

    public ParticleSystem CoffeeStream;
    public float hSliderValue = 0.0F;
    public PickupCup pickupCup;
    public CupManager cupManager;
    public static bool IsMakingCoffee { get; private set; } = false;
    public static bool CoffeeIsDone { get; private set; } = false;
    public void ResetCoffeeState()
    {
        CoffeeIsDone = false;
        m_interactable = true;
        Debug.Log("Coffee state reset to: " + CoffeeIsDone);
    }

    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Interacted with " + gameObject.name);
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        if (obj != null)
        {
            pickupCup = obj.GetComponent<PickupCup>();

            cupManager = interactionController.GetComponent<CupManager>();

            if (cupManager.cupContents.Contains("lid"))
            {
                Debug.LogWarning("Cannot make coffee with a lid on!");
                return;
            }

            GameObject access = CupInteractable.curr;

            if (access != null && !pickupCup.isPlaced)
            {
                pickupCup.isPlaced = true;
                Debug.Log("Cup placed: " + access.name);
                StartPouring();
                PlaceCup(access);
                m_interactable = false;
            }
            
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
    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pickupCup.coffeeDone = true;
        pickupCup.EnableCoffeePickup();
        pickupCup.AddContent("coffee");
        CoffeeIsDone = true;
        IsMakingCoffee = false;
        Debug.Log("Coffee is done. Ready to serve or add ice.");
    }
    public void StartPouring()
    {
        IsMakingCoffee = true;
        AudioSource coffeeSound = GetComponent<AudioSource>();
        coffeeSound.time = 2.2f;
        coffeeSound.Play();
        var main = CoffeeStream.main;
        main.startDelay = hSliderValue;
        CoffeeStream.Play(true);
        StartCoroutine(EnablePickupAfterDelay(CoffeeStream.main.duration + hSliderValue));
    }
}