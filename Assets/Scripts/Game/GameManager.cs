using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _timeToWaitBeforeExit;
    [SerializeField] private SceneController _sceneController;

    public void OnPlayerDied()
    {
        Invoke(nameof(Endgame), _timeToWaitBeforeExit);
    }

    private void Endgame()
    {
        _sceneController.LoadScene("Main Menu");
    }
}
