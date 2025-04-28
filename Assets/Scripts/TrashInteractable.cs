using DefaultNamespace;
using UnityEngine;

public class TrashInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;
    public CoffeeInteractable coffeeInteractable;
    public FrotherInteractable frotherInteractable;
    public CupInteractable cupInteractable;
    public SinkInteractable sinkInteractable;
    public LidInteractable lidInteractable;
    public MilkInteractable milkInteractable;
    public PickupCup pickupCup;
    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Interacted with " + gameObject.name);
        AudioSource trashSound = gameObject.GetComponent<AudioSource>();
        trashSound.time = 1.4f;
        trashSound.Play();
        ResetAll();
        GameObject cup = CupInteractable.curr;
        Destroy(cup);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetAll()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        pickupCup = obj.GetComponent<PickupCup>();
        cupInteractable.ResetCupInteractable();
        coffeeInteractable.ResetCoffeeState();
        frotherInteractable.ResetFrotherState();
        lidInteractable.ResetInteractable();
        sinkInteractable.ResetSinkState();
        milkInteractable.ResetMilkState();
        pickupCup.ClearContent();
    }
}