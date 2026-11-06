using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int _damage = 10;

    private Coroutine _damageRoutine;
    private Health _health;

    private void OnDisable()
    {
        if (_damageRoutine != null) StopCoroutine(_damageRoutine);

        _damageRoutine = null;
        _health = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<Health>(out var health)) return;

        _health = health;

        if (_damageRoutine != null)
            StopCoroutine(_damageRoutine);

        _damageRoutine = StartCoroutine(DamageRoutine());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (_damageRoutine != null)
            StopCoroutine(_damageRoutine);

        _damageRoutine = null;
        _health = null;
    }

    private IEnumerator DamageRoutine()
    {
        while (_health != null)
        {
            _health.TakeDamage(_damage);
            yield return new WaitForSeconds(1f);
        }
    }
}
