using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _spawnAreaSize = new Vector2(10f, 10f);

    public event Action OnEnemyDied;

    public void SpawnEnemy(EnemyData data)
    {
        Vector2 randomOffset = new Vector2(
            UnityEngine.Random.Range(-_spawnAreaSize.x / 2f, _spawnAreaSize.x / 2f),
            UnityEngine.Random.Range(-_spawnAreaSize.y / 2f, _spawnAreaSize.y / 2f));
        Vector2 spawnPos = (Vector2)transform.position + randomOffset;

        GameObject enemyObject = Instantiate(data.Prefab, spawnPos, Quaternion.identity);

        if (enemyObject.TryGetComponent<IEnemy>(out var enemy))
        {
            enemy.Setup(data);
            enemy.OnEnemyDied += HandleEnemyDied;
        }
        else
        {
            Destroy(enemyObject);
        }
    }

    private void HandleEnemyDied(IEnemy enemy)
    {
        enemy.OnEnemyDied -= HandleEnemyDied;

        if (enemy is MonoBehaviour mono)
            Destroy(mono.gameObject);

        OnEnemyDied?.Invoke();
    }
}
