using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public float fadeDuration = 1f; // Duration of the fade-out effect
    private Image splashImage;

    void Awake()
    {
        // Find the Image component of the splash screen Panel
        splashImage = GetComponent<Image>();
        if (splashImage == null)
        {
            Debug.LogError("Splash Screen Image component not found!");
            enabled = false; // Disable the script if the image is missing
        }

        // Immediately start the fade-out coroutine
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color currentColor = splashImage.color;
        float counter = 0f;

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, counter / fadeDuration); // Linearly interpolate alpha
            currentColor.a = alpha;
            splashImage.color = currentColor;
            yield return null; // Wait for the next frame
        }

        // Optionally, disable or destroy the splash screen object after fading
        gameObject.SetActive(false); // Disable the Panel
        // Destroy(gameObject); // Alternatively, destroy the Panel
    }
}