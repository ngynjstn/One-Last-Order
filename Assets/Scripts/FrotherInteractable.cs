using UnityEngine;
using DefaultNamespace;
using System.Collections;
public class FrotherInteractable : MonoBehaviour, IInteractable
{    
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private Transform placeHolder;
    [SerializeField] private GameObject cup;
    public CupManager cupManager; 
    public bool isPlaced = false;
    public static bool IsFrothing { get; private set; } = false;
    public void ResetFrotherState()
    {
        isPlaced = false;
        m_interactable = true;
        Debug.Log("Frother state reset: " + m_interactable);
    }
    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Interacted with " + gameObject.name + ". MilkInCup: " + CoffeeInteractable.CoffeeIsDone);
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        pickupCup = obj.GetComponent<PickupCup>();

        GameObject m_cup = GameObject.FindGameObjectWithTag("Player");
        cupManager = m_cup.GetComponent<CupManager>();
        bool liquid = cupManager.cupContents.Contains("milk") || cupManager.cupContents.Contains("water") || cupManager.cupContents.Contains("coffee");
        if (!liquid)
        {
            Debug.LogWarning("You must add liquid first before frothing!");
            return;
        }
        if (cupManager.cupContents.Contains("lid"))
        {
            Debug.LogWarning("Cannot make coffee with a lid on!");
            return;
        }
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
    public ParticleSystem FrothParticles;
    public PickupCup pickupCup;
    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        pickupCup = obj.GetComponent<PickupCup>();
        GameObject obj1 = GameObject.FindGameObjectWithTag("Player");
        cupManager = obj1.GetComponent<CupManager>();
        pickupCup.coffeeDone = true;
        pickupCup.CompleteFrothing();
        cupManager.AddContent("frother");
        IsFrothing = false;
        Debug.Log("Pickup is now interactable.");
    }
    public void StartFrothing()
    {
        IsFrothing = true;
        Debug.Log("Frothing.");
        AudioSource frothSound = GetComponent<AudioSource>();
        frothSound.Play();
        var main = FrothParticles.main;
        FrothParticles.Play(true);
        StartCoroutine(EnablePickupAfterDelay(FrothParticles.main.duration));
    }

    
}
