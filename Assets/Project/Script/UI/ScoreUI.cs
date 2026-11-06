using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private ScoreManager _scoreManager;

    public void Start()
    {
        _scoreText.text = $"Счет - 0";
    }

    public void Bind(ScoreManager scoreManager)
    {
        if (_scoreManager != null)
            _scoreManager.OnScoreChanged -= UpdateScoreText;

        _scoreManager = scoreManager;

        if (_scoreManager != null)
            _scoreManager.OnScoreChanged += UpdateScoreText;
    }

    private void UpdateScoreText(int score)
    {
        _scoreText.text = $"Счет - {score}";
    }
}