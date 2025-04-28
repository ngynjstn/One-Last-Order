using System.Collections;
using DialogueEditor;
using UnityEngine;

public class AntagonistDialogueTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public NPCConversation antagonistDialogueTwo;
    public void TriggerDialogueTwo()
    {
        StartCoroutine(AntagonistDialogueWait());
    }
    public IEnumerator AntagonistDialogueWait()
    {
        yield return new WaitForSeconds(8f);
        ConversationManager.Instance.StartConversation(antagonistDialogueTwo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
