using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _stopDistance = 0.5f;
    [SerializeField] private GameObject _deathEffect;


    private int _scoreReward;
    private Rigidbody2D _rb;
    private Health _health;
    private Transform _player;

    public event Action<IEnemy> OnEnemyDied;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    public void Setup(EnemyData data)
    {
        _moveSpeed = data.MoveSpeed;
        _stopDistance = data.StopDistance;
        _scoreReward = data.ScoreReward;

        if (_health != null) _health.SetMaxHealth(data.MaxHealth);
    }

    private void OnEnable()
    {
        if (PlayerController.Instance != null)
            _player = PlayerController.Instance.transform;

        if (_health != null)
        {
            _health.OnDied += HandleDeath;
        }
    }

    private void OnDisable()
    {
        _player = null;

        if (_health != null) _health.OnDied -= HandleDeath;

        OnEnemyDied = null;
    }

    private void FixedUpdate()
    {
        if (_player == null)
        {
            if (PlayerController.Instance != null)
                _player = PlayerController.Instance.transform;
            else
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        Vector2 toPlayer = (Vector2)_player.position - _rb.position;

        if (toPlayer.sqrMagnitude < _stopDistance * _stopDistance)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        _rb.linearVelocity = toPlayer.normalized * _moveSpeed;
    }

    private void HandleDeath(Health health)
    {
        if (_deathEffect != null)
            Instantiate(_deathEffect, transform.position, Quaternion.identity);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(_scoreReward);

        OnEnemyDied?.Invoke(this);
    }
}
