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
    public TextMeshProUGUI nameText;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip highVoiceClip;
    public AudioClip lowVoiceClip;

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

    // Cutscene 1 lines
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

    // Cutscene 2 lines (example placeholder)
    public DialogueLine[] cutscene2Lines = new DialogueLine[]
    {
        new DialogueLine { speaker = "Basil", text = "Finally, some time to relax." },
        new DialogueLine { speaker = "Cashier", text = "See you tomorrow!" }
    };

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    void Update()
    {
        // Press G to start cutscene 2 directly (simulate gameplay done)
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartSecondCutscene();
        }
    }

    void StartSecondCutscene()
    {
        dialogueLines = cutscene2Lines; // Assign second cutscene
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Reset fade screens
        fadeScreen.SetActive(true);   // Show fade in at start
        fadeOut.SetActive(false);     // Hide fade out at start

        // Clear previous dialogue
        dialogueText.text = "";
        nameText.text = "";
        textBox.SetActive(false);     // Hide text box until typing begins

        yield return new WaitForSeconds(2f); // Optional initial delay

        // Show text box and start dialogue
        textBox.SetActive(true);
        fadeScreen.SetActive(false);  // Fade in finished

        yield return new WaitForSeconds(2f);

        string currentSpeaker = "";

        for (int i = 0; i < dialogueLines.Length; i++)
        {
            DialogueLine line = dialogueLines[i];

            // Trigger Basil entrance if needed
            if (line.speaker == "Basil" && !basil.activeSelf)
            {
                basil.SetActive(true);
                yield return new WaitForSeconds(2f); // Wait for animation
            }

            // Update speaker name only if it changed
            if (line.speaker != currentSpeaker)
            {
                currentSpeaker = line.speaker;

                if (currentSpeaker == "Narrator" || string.IsNullOrWhiteSpace(currentSpeaker))
                    nameText.gameObject.SetActive(false);
                else
                {
                    nameText.gameObject.SetActive(true);
                    nameText.text = currentSpeaker;
                }
            }

            // Type the line with voice
            yield return StartCoroutine(TypeText(line.text, currentSpeaker));

            // Delay before next line
            yield return new WaitForSeconds(lineDelay);
        }

        // At the end, show fade out
        yield return new WaitForSeconds(1f);
        fadeOut.SetActive(true);

        // Hide text box so it doesnft show previous line
        textBox.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";
    }



    IEnumerator TypeText(string textToType, string speaker)
    {
        dialogueText.text = "";

        foreach (char letter in textToType)
        {
            if (Input.GetKeyDown(skipKey))
            {
                dialogueText.text = textToType;
                break;
            }

            dialogueText.text += letter;

            if (audioSource != null)
            {
                AudioClip clipToPlay = (speaker == "Basil" || speaker == "Cashier") ? highVoiceClip : lowVoiceClip;
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(clipToPlay);
            }

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
