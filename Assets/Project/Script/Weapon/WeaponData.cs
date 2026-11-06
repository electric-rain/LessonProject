using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Visual")]
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Stats")]
    [SerializeField] private string _weaponName;
    [SerializeField] private float _bulletSpeed = 15f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private float _spawnOffset = 0.6f;

    [Header("Shooting")]
    [SerializeField] private float _fireRate = 0.3f;
    [SerializeField] private float _spread = 0f;
    [SerializeField] private int _pelletsPerShot = 1;

    public GameObject BulletPrefab => _bulletPrefab;
    public string WeaponName => _weaponName;
    public float BulletSpeed => _bulletSpeed;
    public int Damage => _damage;
    public float LifeTime => _lifeTime;
    public float FireRate => _fireRate;
    public float Spread => _spread;
    public float SpawnOffset => _spawnOffset;
    public int PelletsPerShot => _pelletsPerShot;
}
