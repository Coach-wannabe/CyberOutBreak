using UnityEngine;

public class GunAim : MonoBehaviour
{
    [SerializeField] private Transform _gunSpriteTransform;
    [SerializeField] private Transform _crosshair;

    private void Start()
    {
        if (_crosshair == null)
            _crosshair = FindObjectOfType<CrosshairController>()?.transform;
    }

    private void Update()
    {
        if (_crosshair == null || _gunSpriteTransform == null) return;

        Vector2 direction = (_crosshair.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip vertically if aiming left
        Vector3 scale = _gunSpriteTransform.localScale;
        scale.y = angle > 90 || angle < -90 ? -1 : 1;
        _gunSpriteTransform.localScale = scale;
    }
}
