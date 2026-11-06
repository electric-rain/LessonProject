using System.Collections;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healthAmount = 20;
    [SerializeField] private Vector2 _spawnAreaSize = new Vector2(5f, 5f);

    private SpriteRenderer _sr;
    private Collider2D _collider;

    private Vector2 _startPosition;
    private float _recoveryTime = 3f;
    private bool _isActive;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _isActive = true;
        _startPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive) return;
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<Health>(out var health)) return;
        if (health.IsFullHealth) return;

        health.Heal(_healthAmount);

        _isActive = false;
        StartCoroutine(RecoveryRoutine());
    }

    private IEnumerator RecoveryRoutine()
    {
        _sr.enabled = false;
        _collider.enabled = false;

        yield return new WaitForSeconds(_recoveryTime);

        SpawnOne();

        _isActive = true;
        _collider.enabled = true;
        _sr.enabled = true;
    }

    private void SpawnOne()
    {
        Vector2 randomOffset = new Vector2(
           Random.Range(-_spawnAreaSize.x / 2f, _spawnAreaSize.x / 2f),
           Random.Range(-_spawnAreaSize.y / 2f, _spawnAreaSize.y / 2f));

        Vector2 spawnPosition = _startPosition + randomOffset;

        transform.position = spawnPosition;
        transform.rotation = Quaternion.identity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = Application.isPlaying ? (Vector3)_startPosition : transform.position;

        Gizmos.DrawWireCube(center, _spawnAreaSize);
    }
}
