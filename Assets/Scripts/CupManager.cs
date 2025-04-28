using System.Collections.Generic;
using UnityEngine;

public class CupManager : MonoBehaviour
{
    [SerializeField] public List<string> cupContents = new List<string>();

    public void SetCupContents(List<string> newContents)
    {
        cupContents = new List<string>(newContents);
        Debug.Log("Cup contents updated in CupManager.");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public PickupCup pickupCup = null;
    // Update is called once per frame
    void Update()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Cup");
        if (obj != null)
        {
            pickupCup = obj.GetComponent<PickupCup>();
            cupContents = pickupCup.cupContents;
        }
        
        
    }
}
