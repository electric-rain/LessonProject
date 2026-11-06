using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Weapon")]
    [SerializeField] private int _prewarmBullets = 20;
    [SerializeField] private WeaponData[] _availableWeapons;
    private WeaponData _currentWeapon;
    private float _fireTimer;

    private Collider2D _playerCollider;
    private Rigidbody2D _rb;
    private Health _health;
    private Dictionary<GameObject, ObjectPool<Bullet>> _bulletPools =
        new Dictionary<GameObject, ObjectPool<Bullet>>();

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
        _playerCollider = GetComponent<Collider2D>();
        Instance = this;

        foreach (var weapon in _availableWeapons)
        {
            if (weapon != null && weapon.BulletPrefab.GetComponent<Bullet>())
            {
                Bullet bulletComponent = weapon.BulletPrefab.GetComponent<Bullet>();
                _bulletPools[weapon.BulletPrefab] = new ObjectPool<Bullet>(bulletComponent, null, _prewarmBullets);
            }
        }

        if (_availableWeapons.Length > 0)
            _currentWeapon = _availableWeapons[0];
    }

    private void Update()
    {
        _fireTimer -= Time.deltaTime;

        if (_fireTimer < 0) _fireTimer = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);

        if (Input.GetKey(KeyCode.Mouse0) && _fireTimer <= 0)
        {
            Fire();
            _fireTimer = _currentWeapon.FireRate;
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        _rb.linearVelocity = input.normalized * _moveSpeed;
    }

    private void SwitchWeapon(int index)
    {
        if (index >= 0 && index < _availableWeapons.Length)
        {
            _currentWeapon = _availableWeapons[index];
            _fireTimer = 0;
            Debug.Log(_currentWeapon.WeaponName);
        }
    }

    private void Fire()
    {
        Vector2 direction = GetMouseDirection();
        var pool = _bulletPools[_currentWeapon.BulletPrefab];

        for (int i = 0; i < _currentWeapon.PelletsPerShot; i++)
        {
            float randomAngle = UnityEngine.Random.Range(-_currentWeapon.Spread / 2f, _currentWeapon.Spread / 2f);
            Vector2 spreadDirection = RotateVector(direction, randomAngle);

            Bullet bullet = pool.Get();
            bullet.Setup(_currentWeapon.BulletSpeed, _currentWeapon.Damage, _currentWeapon.LifeTime);
            bullet.transform.position = (Vector2)transform.position + spreadDirection * _currentWeapon.SpawnOffset;

            bullet.OnExpired += (expiredBullet) => pool.Return(expiredBullet);

            bullet.Launch(spreadDirection, _playerCollider);
        }
    }

    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    private Vector2 GetMouseDirection()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = (Vector2)mouseWorldPos - (Vector2)transform.position;
        return direction.normalized;
    }

    private void OnEnable()
    {
        _health.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnDied -= HandleDeath;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void HandleDeath(Health health)
    {
        _rb.linearVelocity = Vector2.zero;

        enabled = false;
    }
}