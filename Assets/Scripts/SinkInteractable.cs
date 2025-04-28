using DefaultNamespace;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SinkInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private Transform placeHolder;
    [SerializeField] private GameObject cup;
    [SerializeField] public bool isPlaced = false;
    public static bool IsPouringWater { get; private set; } = false;
    public void ResetSinkState()
    {
        m_interactable = true;
        Debug.Log("Sink state reset to: " + true);
    }
    public CupManager cupManager;
    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Interacted with " + gameObject.name);
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        pickupCup = obj.GetComponent<PickupCup>();
        //GameObject obj1 = GameObject.FindGameObjectWithTag("Player");
        //cupManager = obj1.GetComponent<CupManager>();
        cupManager = interactionController.GetComponent<CupManager>();
        // Check if frothing is happening
        if (FrotherInteractable.IsFrothing)
        {
            Debug.LogWarning("Cannot make coffee while frothing!");
            return;
        }
        if (cupManager.cupContents.Contains("lid"))
        {
            Debug.LogWarning("Cannot use sink with a lid on!");
            return;
        }
        GameObject access = CupInteractable.curr;

        if (access != null && !isPlaced)
        {
            isPlaced = true;
            Debug.Log("Cup placed: " + access.name);
            StartPouring();
            PlaceCup(access);
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
    public ParticleSystem WaterStream;
    public float hSliderValue = 0.0F;
    public PickupCup pickupCup;
    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        pickupCup = obj.GetComponent<PickupCup>();
        GameObject obj1 = GameObject.FindGameObjectWithTag("Player");
        cupManager = obj1.GetComponent<CupManager>();
        pickupCup.coffeeDone = true;
        pickupCup.EnableCoffeePickup();
        pickupCup.AddContent("water");
        IsPouringWater = false;
        isPlaced = false;
        Debug.Log("Coffee is done. Ready for pickup or frothing.");
    }
    public void StartPouring()
    {
        IsPouringWater = true;
        AudioSource coffeeSound = GetComponent<AudioSource>();
        coffeeSound.time = 2.2f;
        coffeeSound.Play();
        var main = WaterStream.main;
        main.startDelay = hSliderValue;
        WaterStream.Play(true);
        StartCoroutine(EnablePickupAfterDelay(WaterStream.main.duration + hSliderValue));
    }
}