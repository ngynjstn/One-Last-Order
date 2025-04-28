using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIntro : MonoBehaviour
{
    [Header("Fade In Settings")]
    public float fadeInDuration = 1f;

    private Image splashImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Get the Image component of the splash screen Panel
        splashImage = GetComponent<Image>();
        if (splashImage == null)
        {
            Debug.LogError("Splash Screen Image component not found!");
            enabled = false;
            return;
        }

        // Start the fade-in
        Color startColor = splashImage.color;
        startColor.a = 1f;
        splashImage.color = startColor;
        StartCoroutine(FadeIn());


    }

    IEnumerator FadeIn()
    {
        float counter = 0f;
        Color currentColor = splashImage.color;

        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, counter / fadeInDuration);
            currentColor.a = alpha;
            splashImage.color = currentColor;
            yield return null;
        }

    }
    IEnumerator FadeOut()
    {
        float counter = 0f;
        Color panelColor = splashImage.color;
        float fadeDuration = fadeInDuration;

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, counter / fadeDuration);
            panelColor.a = alpha;
            splashImage.color = panelColor;
            yield return null;
        }

        // Fade out complete
        gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
