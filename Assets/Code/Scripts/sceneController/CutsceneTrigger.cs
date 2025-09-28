using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneEvents sceneEvents;
    //[SerializeField] private string cutsceneName = "Opening";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (sceneEvents != null)
            {
                //sceneEvents.PlaySequence();
                Debug.Log("Cutscene triggered");
                //gameObject.SetActive(false); // optional: disable trigger
            }
            else
            {
                Debug.LogWarning("CutsceneController not assigned!");
            }
        }
    }
}
