using UnityEngine;
using UnityEngine.Playables;

//[CreateAssetMenu(fileName = "sceneController", menuName = "Scriptable Objects/sceneController")]
public class sceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    [Header("Gameplay References")]
    [SerializeField] private GameObject playerController;
    [SerializeField] private GameObject gameplayUI;

    void Start()
    {
        // Optional: disable gameplay during cutscene
        //if (playerController != null) playerController.SetActive(false);
        //if (gameplayUI != null) gameplayUI.SetActive(false);

        // Subscribe to when cutscene ends
        director.stopped += OnCutsceneEnd;
    }

    private void OnCutsceneEnd(PlayableDirector pd)
    {
        EnableGameplay();
    }

    private void DisableGameplay()
    {
        if (playerController != null) playerController.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(false);
    }

    private void EnableGameplay()
    {
        if (playerController != null) playerController.SetActive(true);
        if (gameplayUI != null) gameplayUI.SetActive(true);
    }
}
