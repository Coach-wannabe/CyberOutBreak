using UnityEngine;

public class GunOffsetController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _crosshairTransform;
    [SerializeField] private float _offsetDistance = 0.6f;

    private void Update()
    {
        if (_playerTransform == null || _crosshairTransform == null)
            return;

        Vector2 direction = (_crosshairTransform.position - _playerTransform.position).normalized;
        transform.position = _playerTransform.position + (Vector3)(direction * _offsetDistance);
    }
}
