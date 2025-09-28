using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private Scene01Events scene01Events;
    //[SerializeField] private string cutsceneName = "Opening";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (scene01Events != null)
            {
                scene01Events.PlayCutscene();
                Debug.Log("Cutscene triggered");
                gameObject.SetActive(false); // optional: disable trigger
            }
            else
            {
                Debug.LogWarning("CutsceneController not assigned!");
            }
        }
    }
}
