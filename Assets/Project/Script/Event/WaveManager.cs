using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private WaveData[] _waves;
    [SerializeField] private float _delayBetweenWaves = 3f;

    public event Action<int, int> OnWaveStarted;
    public event Action OnAllWavesCompleted;

    private int _currentWaveIndex;
    private int _aliveEnemies;

    private void OnEnable()
    {
        _spawner.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        _spawner.OnEnemyDied -= HandleEnemyDied;
    }

    private void Start()
    {
        var ui = FindAnyObjectByType<WaveUI>();

        if (ui != null)
            ui.Bind(this);

        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        for (_currentWaveIndex = 0; _currentWaveIndex < _waves.Length; _currentWaveIndex++)
        {
            WaveData currentWave = _waves[_currentWaveIndex];

            _aliveEnemies = currentWave.EnemiesToSpawn.Length;

            OnWaveStarted?.Invoke(_currentWaveIndex + 1, _aliveEnemies);

            foreach (EnemyData enemyData in currentWave.EnemiesToSpawn)
            {
                _spawner.SpawnEnemy(enemyData);
                yield return new WaitForSeconds(currentWave.SpawnInterval);
            }

            while (_aliveEnemies > 0)
                yield return null;

            yield return new WaitForSeconds(_delayBetweenWaves);

            OnAllWavesCompleted?.Invoke();

        }

        GameEvents.PlayerIsWin();
    }

    private void HandleEnemyDied()
    {
        _aliveEnemies--;
    }
}