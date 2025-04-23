using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [Header("Fade In Settings")]
    public Animator fadeAnimator;

    public void FadeIn()
    {
        // Start the fade-in animation
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeIn");
        }
        else
        {
            Debug.LogError("Fade Animator not assigned!");
        }
    }
    public void FadeOut()
    {
        // Start the fade-out animation
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeOut");
        }
        else
        {
            Debug.LogError("Fade Animator not assigned!");
        }
    }
    private void Awake()
    {
        // Get the Image component of the splash screen Panel

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
