using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;
    private WaveManager _waveManager;

    public void Bind(WaveManager manager)
    {
        if (_waveManager != null)
        {
            _waveManager.OnWaveStarted -= UpdateWaveText;
            _waveManager.OnAllWavesCompleted -= ShowVictory;
        }

        _waveManager = manager;

        if (_waveManager != null)
        {
            _waveManager.OnWaveStarted += UpdateWaveText;
            _waveManager.OnAllWavesCompleted += ShowVictory;
        }
    }

    private void UpdateWaveText(int waveNumber, int enemiesCount)
    {
        _waveText.text = $"Волна {waveNumber} (Врагов: {enemiesCount})";
    }

    private void ShowVictory()
    {
        _waveText.text = "ПОБЕДА!";
        _waveText.color = Color.yellow;
    }
}
