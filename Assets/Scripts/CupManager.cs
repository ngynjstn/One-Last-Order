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
}
