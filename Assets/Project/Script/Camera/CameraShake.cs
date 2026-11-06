using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField] private CinemachineCamera _virtualCamera;

    private CinemachineBasicMultiChannelPerlin _noise;
    private Coroutine _shakeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_virtualCamera != null)
            _noise = _virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity = 1f, float duration = 0.2f)
    {
        if (_noise == null) return;

        if (_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);

        _shakeRoutine = StartCoroutine(ShakeRoutine(intensity, duration));
    }

    private IEnumerator ShakeRoutine(float intensity, float duration)
    {
        _noise.AmplitudeGain = intensity;
        yield return new WaitForSeconds(duration);
        _noise.AmplitudeGain = 0f;
        _shakeRoutine = null;
    }
}