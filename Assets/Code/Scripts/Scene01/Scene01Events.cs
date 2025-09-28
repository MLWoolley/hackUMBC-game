using System.Collections;
using UnityEngine;
using TMPro;

public class Scene01Events : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject fadeScreen;
    public GameObject fadeOut;
    public GameObject textBox;
    public GameObject basil; // Basil starts inactive
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText; // Character name display

    [Header("Typing Audio")]
    public AudioSource audioSource;       // The AudioSource component
    public AudioClip highVoiceClip;       // For high-pitched characters
    public AudioClip lowVoiceClip;        // For low-pitched characters

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;
    public float lineDelay = 2f;
    public KeyCode skipKey = KeyCode.Space;

    [System.Serializable]
    public struct DialogueLine
    {
        public string speaker;
        [TextArea(2, 5)]
        public string text;
    }

    // Example sequence
    public DialogueLine[] dialogueLines = new DialogueLine[]
    {
        new DialogueLine { speaker = "Narrator", text = "The hum of the lights fills the air." },
        new DialogueLine { speaker = "Cashier", text = "Welcome! What can I get you?" },
        new DialogueLine { speaker = "Narrator", text = "From the side, Basil steps into view..." },
        new DialogueLine { speaker = "Basil", text = "Sorry, I'll have the usual." },
        new DialogueLine { speaker = "Basil", text = "Make it to go this time." },
        new DialogueLine { speaker = "Cashier", text = "You got it." },
        new DialogueLine { speaker = "Narrator", text = "Basil nods and looks away." }
    };

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Initial setup delay
        yield return new WaitForSeconds(2f);

        textBox.SetActive(true);
        fadeScreen.SetActive(false);

        yield return new WaitForSeconds(2f);

        string currentSpeaker = "";

        for (int i = 0; i < dialogueLines.Length; i++)
        {
            DialogueLine line = dialogueLines[i];

            // Handle Basil appearing before her first line
            if (line.speaker == "Basil" && !basil.activeSelf)
            {
                // Trigger Basil's entrance (your animation should run on enable)
                basil.SetActive(true);
                yield return new WaitForSeconds(2f); // Wait for her animation
            }

            // Update speaker name only if it changed
            if (line.speaker != currentSpeaker)
            {
                currentSpeaker = line.speaker;

                if (currentSpeaker == "Narrator" || string.IsNullOrWhiteSpace(currentSpeaker))
                {
                    nameText.gameObject.SetActive(false);
                }
                else
                {
                    nameText.gameObject.SetActive(true);
                    nameText.text = currentSpeaker;
                }
            }

            // Type out the line
            yield return StartCoroutine(TypeText(line.text));

            // Short delay before next line
            yield return new WaitForSeconds(lineDelay);
        }

        yield return new WaitForSeconds(1f); // brief pause
        fadeOut.SetActive(true);
    }

    IEnumerator TypeText(string textToType)
    {
        dialogueText.text = "";

        foreach (char letter in textToType)
        {
            // Skip typing if user presses skipKey
            if (Input.GetKeyDown(skipKey))
            {
                dialogueText.text = textToType;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }
}
