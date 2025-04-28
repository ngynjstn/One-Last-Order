using DefaultNamespace;
using UnityEngine;

public class LidInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    [SerializeField] public string InteractableHintText => m_interactableHintText;
    // Set to true to continuous make drinks
    [SerializeField] private bool m_interactable = true;
    [SerializeField] public bool IsInteractable => m_interactable;

    public void ResetInteractable()
    {
        m_interactable = true;
        Debug.Log("Cup dispenser reset and ready to dispense a new cup");
    }
    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Interacted with " + gameObject.name);
        GameObject access = CupInteractable.curr;

        if (access != null)
        {
            Debug.Log("Cup placed: " + access.name);
            AddLid();
            Debug.Log(access);
            m_interactable = false;
        }
        else
        {
            Debug.LogWarning("No cup in hand to add lid.");
        }
    }
    public PickupCup pickupCup;
    public MeshRenderer mesh;
    public CupManager cupManager;
    private void AddLid()
    {
        GameObject access = CupInteractable.curr;
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        pickupCup = obj.GetComponent<PickupCup>();
        GameObject obj1 = GameObject.FindGameObjectWithTag("Player");
        cupManager = obj1.GetComponent<CupManager>();
        if (access != null)
        {
            // Find the child named "Lid" inside the current cup
            Transform lidTransform = access.transform.Find("Tumbler");

            if (lidTransform != null)
            {
                lidTransform.gameObject.SetActive(true);
                mesh = obj.GetComponent<MeshRenderer>();
                mesh.enabled = false;
                cupManager.AddContent("lid");
                Debug.Log("Lid activated!");
                Debug.Log(string.Join(", ", cupManager.cupContents));
            }
            else
            {
                Debug.LogWarning("Lid not found on cup.");
            }
        }
        else
        {
            Debug.LogWarning("No current cup to add lid to.");
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
