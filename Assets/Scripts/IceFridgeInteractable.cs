using DefaultNamespace;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class IceFridgeInteractable : MonoBehaviour, IInteractable
{
    // The prompt text that appears when you're looking at an interactable.
    [SerializeField] private string m_interactableHintText = "Press E to interact";
    public string InteractableHintText => m_interactableHintText;
    // Disable m_interactable thru ur script if you want things to be like, one use button type shi
    [SerializeField] private bool m_interactable = true;
    public bool IsInteractable => m_interactable;

    [SerializeField] private GameObject cup;

    public void Interact(InteractionController interactionController)
    {
        Debug.Log("Interacted with " + gameObject.name);

        GameObject access = CupInteractable.curr;

        if (access != null)
        {
            StartPouring();
        }
        else
        {
            Debug.LogWarning("No cup in hand.");
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
    public CupManager cupManager;
    public void StartPouring()
    {
        AudioSource iceSound = GetComponent<AudioSource>();
        iceSound.Play();
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        cupManager = obj.GetComponent<CupManager>();
        cupManager.AddContent("ice");
    }
}