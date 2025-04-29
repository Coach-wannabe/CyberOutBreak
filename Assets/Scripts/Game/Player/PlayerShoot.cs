using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _gunOffset;
    [SerializeField] private float _timeBetweenShots;

    private bool _fireContinuously;
    private float _lastFireTime;
    private CrosshairController _crosshair;

    private void Awake()
    {
        _crosshair = FindObjectOfType<CrosshairController>();
    }

    void Update()
    {
        // Only fire if holding fire button AND enough time has passed
        if (_fireContinuously && Time.time - _lastFireTime >= _timeBetweenShots)
        {
            FireBullet();
            _lastFireTime = Time.time;
        }
    }

    private void FireBullet()
    {
        if (_gunOffset == null || _crosshair == null)
        {
            Debug.LogWarning("Gun offset or Crosshair is not assigned.");
            return;
        }

        GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, Quaternion.identity);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        Vector2 shootDirection = (_crosshair.transform.position - _gunOffset.position).normalized;
        rigidbody.linearVelocity = _bulletSpeed * shootDirection;

        // 👉 Rotate bullet immediately to face the shooting direction
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    public void OnAttack(InputValue inputValue)
    {
        _fireContinuously = inputValue.isPressed;

        if (_fireContinuously)
        {
            FireBullet();
            _lastFireTime = Time.time;
        }
    }
}
