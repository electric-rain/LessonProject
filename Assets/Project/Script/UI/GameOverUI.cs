using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _winPanel;

    private void Awake()
    {
        GameEvents.OnPlayerDied += ShowGameOver;
        GameEvents.OnPlayerWin += ShowWinScreen;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= ShowGameOver;
        GameEvents.OnPlayerWin -= ShowWinScreen;
    }

    private void ShowGameOver(Health health)
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ShowWinScreen()
    {
        _winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}