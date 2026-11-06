using UnityEngine;

public class HealthColorChanger : MonoBehaviour
{
    [SerializeField] private Color _fullHealthColor = Color.green;
    [SerializeField] private Color _midHealthColor = Color.yellow;
    [SerializeField] private Color _lowHealthColor = Color.red;

    private Health _health;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _health.OnHealthChange += UpdateColor;
    }

    private void OnDisable()
    {
        _health.OnHealthChange -= UpdateColor;
    }

    private void UpdateColor(int cuurentHealth, int maxHealth)
    {
        _sr.color = GetHealthColor(cuurentHealth, maxHealth);
    }

    private Color GetHealthColor(float current, float max)
    {
        float percent = Mathf.Clamp01(current / max);

        if (percent > 0.5f)
        {
            float t = (percent - 0.5f) / 0.5f;
            return Color.Lerp(_midHealthColor, _fullHealthColor, t);
        }

        float t2 = percent / 0.5f;
        return Color.Lerp(_lowHealthColor, _midHealthColor, t2);
    }
}
