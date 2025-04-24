using UnityEngine;
using DialogueEditor;

public class WarningZone : MonoBehaviour
{
    public string warningMessage;
    private bool hasShownMessage = false;
    public NPCConversation redirectmessage;

    public void OnTriggerEnter(Collider other)
    {
        if (!hasShownMessage && other.CompareTag("Blocker"))
        {
            hasShownMessage = true;
            ConversationManager.Instance.StartConversation(redirectmessage);
            
            //Debug.Log(warningMessage);

            
        }
    }

    
}