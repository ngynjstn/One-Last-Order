using System.Collections.Generic;
using UnityEngine;

public class CupManager : MonoBehaviour
{
    [SerializeField] public List<string> cupContents = new List<string>();

    public void AddContent(string content)
    {
        cupContents.Add(content);
        Debug.Log($"Added {content} to cup.");
    }
    public void ClearContent()
    {
        cupContents.Clear();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public PickupCup pickupCup = null;
    // Update is called once per frame
    void Update()
    {
        
    }
}
