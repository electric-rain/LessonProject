using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _fillImage;

    private float _fullWidth;

    private void Awake()
    {
        if (_fillImage != null)
            _fullWidth = _fillImage.rectTransform.sizeDelta.x;
    }

    private void OnEnable()
    {
        _health.OnHealthChange += UpdateBar;
    }

    private void OnDisable()
    {
        _health.OnHealthChange -= UpdateBar;
    }

    private void UpdateBar(int cuurent, int max)
    {
        if (_fillImage == null) return;

        float percent = max > 0 ? (float)cuurent / max : 0f;
        percent = Mathf.Clamp01(percent);

        Vector2 size = _fillImage.rectTransform.sizeDelta;
        size.x = _fullWidth * percent;
        _fillImage.rectTransform.sizeDelta = size;
    }
}
