using System.Collections;
using DialogueEditor;
using UnityEngine;


public class ActIII_StoryManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public NPCConversation dialogueObject;
    [SerializeField] private Animator fadeAnimator;

    private void Start()
    {
        fadeAnimator.SetTrigger("Fade_In");
        StartCoroutine(BeginningDialogue());
    }

    IEnumerator BeginningDialogue()
    {
        // wait a bit before starting the intro dialogue

        yield return new WaitForSeconds(2f);
        ConversationManager.Instance.StartConversation(dialogueObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
