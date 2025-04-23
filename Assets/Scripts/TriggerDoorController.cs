using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private string openAnimationName = "DoorOpen"; // 
    [SerializeField] private string closeAnimationName = "DoorClose"; //
    [SerializeField] private bool startOpen = false; // Should the door start open?
    private bool isOpen = false;

    private void Start()
    {
        isOpen = startOpen;
        if (myDoor != null)
        {
            myDoor.Play(isOpen ? openAnimationName : closeAnimationName);
        }
        else
        {
            Debug.LogError($"{gameObject.name} requires an Animator component assigned to 'My Door'.");
            enabled = false; // Disable the script if no Animator is assigned
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (myDoor != null)
            {
                isOpen = !isOpen; // Toggle the door state
                myDoor.Play(isOpen ? openAnimationName : closeAnimationName, 0, 0.0f);
            }
        }
    }
}