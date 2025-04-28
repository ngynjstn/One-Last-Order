using System.Collections.Generic;
using UnityEngine;

public class CupManager : MonoBehaviour
{
    [SerializeField] public List<string> cupContents = new List<string>();

    ///*public void AddContent(string content)
    //{
    //    cupContents.Add(content);
    //    Debug.Log($"Added {content} to cup.");
    //}
    //public void ClearContent()
    //{
    //    cupContents.Clear();
    //}*/
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public CustomerDrinkWanted ResolveCupContents()
    {
        // latte 
        if (!cupContents.Contains("lid")) {
            return CustomerDrinkWanted.nothing;
        }
        if (cupContents.Contains("coffee") && cupContents.Contains("frother")) {
            return CustomerDrinkWanted.hotcoffee;
        }
        if (cupContents.Contains("coffee") && cupContents.Contains("ice"))
        {
            return CustomerDrinkWanted.coldcoffee;
        }
        if (cupContents.Contains("coffee") && cupContents.Contains("milk"))
        {
            return CustomerDrinkWanted.latte;
        }
        if (cupContents.Contains("water"))
        {
            return CustomerDrinkWanted.water;
        }
        return CustomerDrinkWanted.nothing;
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
