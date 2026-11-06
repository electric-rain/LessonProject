using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _fireInterval = 2f;
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private int _prewarmBullets = 10;

    private ObjectPool<Bullet> _bulletPool;
    private Collider2D _collider;
    private Transform _player;
    private float _detectionRangeSqr;
    private Coroutine _fireRoutine;

    private List<Bullet> _activeBulellet = new List<Bullet>();

    private void Awake()
    {
        _bulletPool = new ObjectPool<Bullet>(_bulletPrefab, null, _prewarmBullets);
        _collider = GetComponent<Collider2D>();
        _detectionRangeSqr = _detectionRange * _detectionRange;
    }

    private void OnEnable()
    {
        if (PlayerController.Instance != null)
            _player = PlayerController.Instance.transform;
    }

    private void OnDisable()
    {
        _player = null;

        if (_fireRoutine != null)
            StopCoroutine(_fireRoutine);

        _fireRoutine = null;

        for (int i = _activeBulellet.Count - 1; i >= 0; i--)
        {
            Bullet bullet = _activeBulellet[i];

            if (bullet != null)
            {
                bullet.OnExpired -= OnBulletExpired;
                _bulletPool.Return(bullet);
            }

            _activeBulellet.RemoveAt(i);
        }
    }

    private void Update()
    {
        if (_player == null)
        {
            if (PlayerController.Instance != null)
                _player = PlayerController.Instance.transform;
            else
                return;
        }

        float distanceSqr = ((Vector2)_player.position - (Vector2)transform.position).sqrMagnitude;
        bool inRange = distanceSqr <= _detectionRangeSqr;

        if (inRange && _fireRoutine == null)
            _fireRoutine = StartCoroutine(FireRoutine());

        else if (!inRange && _fireRoutine != null)
        {
            StopCoroutine(_fireRoutine);
            _fireRoutine = null;
        }
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(_fireInterval);
        }
    }

    private void Fire()
    {
        if (_player == null) return;

        Vector2 direction = ((Vector2)_player.position - (Vector2)transform.position).normalized;

        Bullet bullet = _bulletPool.Get();
        bullet.transform.position = (Vector2)transform.position + direction * 0.6f;
        bullet.transform.rotation = Quaternion.identity;

        _activeBulellet.Add(bullet);

        bullet.OnExpired += OnBulletExpired;
        bullet.Launch(direction, _collider);
    }

    private void OnBulletExpired(Bullet bullet)
    {
        bullet.OnExpired -= OnBulletExpired;
        _activeBulellet.Remove(bullet);
        _bulletPool.Return(bullet);
    }
}