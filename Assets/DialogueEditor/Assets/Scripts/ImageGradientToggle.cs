using System.Collections;
using DialogueEditor;
using UnityEngine;
using UnityEngine.UI;

public class ImageGradientToggle : MonoBehaviour
{
    private Image gradientImg;
    
    [Range(0.1f, 0.5f)]
    public float m_BlinkTransitionTime = 0.2f; //Time for fading in and out (each one the same amount)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        ConversationManager.OnConversationStarted += ConversationStart;
        ConversationManager.OnConversationEnded += ConversationEnd;
    }
    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= ConversationStart;
        ConversationManager.OnConversationEnded -= ConversationEnd;
    }
    void Start()
    {
        gradientImg = GetComponent<Image>();
        
        gradientImg.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ConversationStart()
    {
        StartCoroutine(FadeIn());
    }
    private void ConversationEnd()
    {
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeIn()
    {
        gradientImg.enabled = true;
        //BLINK IN (fade from transparent to opaque)
        float alpha = 0;
        for (float i = 0; i <= m_BlinkTransitionTime; i += Time.deltaTime)
        {
            alpha = (i - 0) / (m_BlinkTransitionTime - 0); //Normalize value between 0 and 1

            // set color with i as alpha
            gradientImg.color = new Color(gradientImg.color.r, gradientImg.color.g, gradientImg.color.b, alpha);
            //print(gradientImg.color);
            yield return null;
        }
        gradientImg.color = new Color(gradientImg.color.r, gradientImg.color.g, gradientImg.color.b, 1); // fix residual values
    }
    IEnumerator FadeOut()
    {
        float alpha = 0;
        //BLINK OUT (fade from opaque to transparent)
        for (float i = m_BlinkTransitionTime; i >= 0; i -= Time.deltaTime)
        {
            alpha = (i - 0) / (m_BlinkTransitionTime - 0); //Normalize value between 0 and 1

            // set color with i as alpha
            gradientImg.color = new Color(gradientImg.color.r, gradientImg.color.g, gradientImg.color.b, alpha);
            yield return null;
        }
        gradientImg.color = new Color(gradientImg.color.r, gradientImg.color.g, gradientImg.color.b, 0); // fix residual values
        gradientImg.enabled = false;

    }
}
