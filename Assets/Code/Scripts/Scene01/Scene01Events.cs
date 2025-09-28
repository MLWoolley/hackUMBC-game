using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Only needed if you're using UI Text

public class IntroSequence : MonoBehaviour
{
    public GameObject fadeScreen;   // The fade-in screen object
    public GameObject textBox;      // The text box UI element
    public TextMeshProUGUI dialogueText;    // Text component inside textBox
    public GameObject basil;        // Basil character object

    void Start()
    {
        // Start the sequence when the scene begins
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Step 1: Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Step 2: Enable the text box and remove fade screen
        textBox.SetActive(true);
        fadeScreen.SetActive(false);

        // Step 3: Wait 2 more seconds
        yield return new WaitForSeconds(2f);

        // Step 4: Display text
        dialogueText.text = "This is a test";

        // Step 5: Wait 2 more seconds
        yield return new WaitForSeconds(2f);

        // Step 6: Turn on Basil
        basil.SetActive(true);
    }
}
