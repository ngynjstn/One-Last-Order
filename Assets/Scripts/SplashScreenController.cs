using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    [Header("Fade In Settings")]
    public float fadeInDuration = 1f;

    [Header("Dialogue Settings")]
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public float textSpeed = 0.05f;
    public float delayAfterLine = 0.5f;
    public string continuePromptText = "Press Space or Enter to Continue...";
    public float finalFadeOutDelay = 1f; // Delay before the final fade out starts

    private Image splashImage;
    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI continueText;

    private int index = 0;
    private bool canContinue = false;
    private bool dialogueActive = false;

    void Awake()
    {
        // Get the Image component of the splash screen Panel
        splashImage = GetComponent<Image>();
        if (splashImage == null)
        {
            Debug.LogError("Splash Screen Image component not found!");
            enabled = false;
            return;
        }

        // Create UI elements for the dialogue dynamically
        GameObject dialogueTextGO = new GameObject("DialogueText", typeof(TextMeshProUGUI));
        dialogueTextGO.transform.SetParent(transform, false);
        dialogueText = dialogueTextGO.GetComponent<TextMeshProUGUI>();
        dialogueText.alignment = TextAlignmentOptions.Center;
        dialogueText.color = Color.white;
        dialogueText.fontSize = 24;
        RectTransform dialogueRect = dialogueText.rectTransform;
        dialogueRect.anchorMin = new Vector2(0.1f, 0.3f);
        dialogueRect.anchorMax = new Vector2(0.9f, 0.7f);
        dialogueRect.offsetMin = Vector2.zero;
        dialogueRect.offsetMax = Vector2.zero;
        dialogueText.text = "";
        dialogueText.color = new Color(1f, 1f, 1f, 0f); // Start with transparent text

        GameObject continueTextGO = new GameObject("ContinueText", typeof(TextMeshProUGUI));
        continueTextGO.transform.SetParent(transform, false);
        continueText = continueTextGO.GetComponent<TextMeshProUGUI>();
        continueText.alignment = TextAlignmentOptions.Center;
        continueText.color = Color.white;
        continueText.fontSize = 18;
        RectTransform continueRect = continueText.rectTransform;
        continueRect.anchorMin = new Vector2(0.1f, 0.1f);
        continueRect.anchorMax = new Vector2(0.9f, 0.2f);
        continueRect.offsetMin = Vector2.zero;
        continueRect.offsetMax = Vector2.zero;
        continueText.text = continuePromptText;
        continueText.gameObject.SetActive(false);

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

        // Fade-in complete, start the dialogue
        dialogueActive = true;
        StartDialogue();
    }

    void Update()
    {
        if (dialogueActive && canContinue && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            NextLine();
        }
        else if (dialogueActive && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (dialogueText.text == dialogueLines[index])
            {
                NextLine();
            }
            else
            {
                StopCoroutine(TypeText());
                dialogueText.text = dialogueLines[index];
                canContinue = true;
                continueText.gameObject.SetActive(true);
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        dialogueText.text = string.Empty;
        StartCoroutine(TypeText());
        StartCoroutine(FadeInText()); // Start fading in the first line
    }

    IEnumerator FadeInText()
    {
        float fadeTime = textSpeed * dialogueLines[index].Length; // Approximate fade time
        float counter = 0f;
        Color textColor = dialogueText.color;

        while (counter < fadeTime)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, counter / fadeTime);
            textColor.a = alpha;
            dialogueText.color = textColor;
            yield return null;
        }
    }

    IEnumerator TypeText()
    {
        canContinue = false;
        continueText.gameObject.SetActive(false);
        dialogueText.text = "";
        dialogueText.color = new Color(1f, 1f, 1f, 0f); // Reset text alpha for each line

        foreach (char c in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        canContinue = true;
        continueText.gameObject.SetActive(true);
        StartCoroutine(FadeInText()); // Ensure full visibility after typing
    }

    void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            StartCoroutine(TypeText());
            StartCoroutine(FadeInText());
        }
        else
        {
            // Last line, start fading out everything
            dialogueActive = false;
            StartCoroutine(FinalFadeOut());
        }
    }

    IEnumerator FinalFadeOut()
    {
        yield return new WaitForSeconds(finalFadeOutDelay); // Wait before starting the fade out

        float counter = 0f;
        Color panelColor = splashImage.color;
        Color textColor = dialogueText.color;

        float fadeDuration = fadeInDuration; // Use the initial fade duration for consistency

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, counter / fadeDuration);
            panelColor.a = alpha;
            splashImage.color = panelColor;
            textColor.a = Mathf.Lerp(1f, 0f, counter / fadeDuration); // Fade out text
            dialogueText.color = textColor;
            continueText.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, counter / fadeDuration)); // Fade out continue text
            yield return null;
        }

        // Fade out complete, you can now load the next scene or disable this object
        gameObject.SetActive(false);
    }
}