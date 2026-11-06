using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private AudioClip _hurtSound;
    [SerializeField] private AudioClip _healSound;

    public bool IsFullHealth => _currentHealth >= _maxHealth;

    private int _currentHealth;

    public event Action<int, int> OnHealthChange;
    public event Action<Health> OnDied;

    private void Awake() => _currentHealth = _maxHealth;

    private void Start() => OnHealthChange?.Invoke(_currentHealth, _maxHealth);

    public void SetMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;
        _currentHealth = _maxHealth;
        OnHealthChange?.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth = Mathf.Clamp(_currentHealth - damageAmount, 0, _maxHealth);
        OnHealthChange?.Invoke(_currentHealth, _maxHealth);

        AudioManager.Instance.PlaySound(_hurtSound);

        if (CameraShake.Instance != null)
        {
            Debug.Log(gameObject.name + " take damage");
            CameraShake.Instance.Shake(1f, 0.3f);
        }

        if (_currentHealth <= 0)
        {
            OnDied?.Invoke(this);

            if (gameObject.CompareTag("Player"))
            {
                Debug.Log("Tag is player");
                GameEvents.PlayerDied(this);
            }
        }
    }

    public void Heal(int healAmount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        OnHealthChange?.Invoke(_currentHealth, _maxHealth);

        AudioManager.Instance.PlaySound(_healSound);
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        OnHealthChange?.Invoke(_currentHealth, _maxHealth);
    }
}
