using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeMagnitude = 0.1f; 

    private void Start()
    {
        PlayerController.OnCollision += () => StartCoroutine(ShakeCorutine());
    }

    private IEnumerator ShakeCorutine()
    {
        var originalPosition = transform.localPosition;
        var currentShakeDuration = _shakeDuration;
        while (currentShakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * _shakeMagnitude;
            transform.localPosition = originalPosition + shakeOffset;

            currentShakeDuration -= Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
    }
}