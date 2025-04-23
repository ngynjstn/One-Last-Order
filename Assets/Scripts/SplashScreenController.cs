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

    [Header("Audio Settings")]
    public AudioSource typewriterSoundSource; // Assign in Inspector

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
        //Color startColor = splashImage.color;
        //startColor.a = 1f;
        //splashImage.color = startColor;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        //float counter = 0f;
        //Color currentColor = splashImage.color;

        //while (counter < fadeInDuration)
        //{
        //    counter += Time.deltaTime;
        //    float alpha = Mathf.Lerp(1f, 0f, counter / fadeInDuration);
        //    currentColor.a = alpha;
        //    splashImage.color = currentColor;
        //    yield return null;
        //}

        //// Fade-in complete, start the dialogue
        dialogueActive = true;
        StartDialogue();
        yield return null;
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
                StopCoroutine(TypeLine());
                typewriterSoundSource.Stop();
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
        dialogueText.color = Color.white; // Make text visible from the start
        continueText.color = Color.white; // Make continue text visible from the start
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        canContinue = false;
        continueText.gameObject.SetActive(false);
        typewriterSoundSource.Play();
        dialogueText.text = "";

        foreach (char c in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        typewriterSoundSource.Stop();
        canContinue = true;
        continueText.gameObject.SetActive(true);
    }

    void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            // Dialogue is finished, fade out everything
            dialogueActive = false;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        //float counter = 0f;
        //Color panelColor = splashImage.color;
        //Color textColor = dialogueText.color;
        //Color continueTextColor = continueText.color;
        //float fadeDuration = fadeInDuration;

        //while (counter < fadeDuration)
        //{
        //    counter += Time.deltaTime;
        //    float alpha = Mathf.Lerp(0f, 1f, counter / fadeDuration);
        //    panelColor.a = alpha;
        //    splashImage.color = panelColor;
        //    textColor.a = Mathf.Lerp(1f, 0f, counter / fadeDuration);
        //    dialogueText.color = textColor;
        //    continueTextColor.a = Mathf.Lerp(1f, 0f, counter / fadeDuration);
        //    continueText.color = continueTextColor;
        //    yield return null;
        //}

        //// Fade out complete
        gameObject.SetActive(false);
        yield return null;
    }
}