using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private int _damage = 20;
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private GameObject _hitEffectPrefab;
    [SerializeField] private AudioClip _shotSound;
    private Collider2D _ownerCollider;

    public event Action<Bullet> OnExpired;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _lifeTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(float speed, int damage, float lifetime)
    {
        _speed = speed;
        _damage = damage;
        _lifetime = lifetime;
    }

    private void OnEnable()
    {
        _lifeTimer = _lifetime;
    }

    private void OnDisable()
    {
        OnExpired = null;
        _ownerCollider = null;
    }

    public void Launch(Vector2 direction, Collider2D ownerCollider)
    {
        _direction = direction.normalized;
        _ownerCollider = ownerCollider;


        float bulletAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);

        _rb.linearVelocity = _direction * _speed;

        AudioManager.Instance.PlaySound(_shotSound);

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(0.5f, 0.1f);

        _lifeTimer = _lifetime;
    }

    private void FixedUpdate()
    {
        _lifeTimer -= Time.fixedDeltaTime;

        if (_lifeTimer <= 0f)
        {
            Expire();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == _ownerCollider) return;
        if (other.CompareTag("Bullet")) return;

        if (other.TryGetComponent<Health>(out var health))
        {
            if (_hitEffectPrefab != null)
                Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);

            health.TakeDamage(_damage);
        }

        Expire();
    }

    private void Expire()
    {
        OnExpired?.Invoke(this);
    }
}
